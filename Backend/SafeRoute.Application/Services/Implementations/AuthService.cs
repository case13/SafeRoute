using SafeRoute.Application.Services.Interfaces;
using SafeRoute.Domain.Repositories.Interfaces;
using SafeRoute.Shared.Dtos.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeRoute.Application.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _usuarioRepository;
        private readonly ITokenService _tokenService;
        private readonly IPasswordHasherService _passwordHasherService;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public AuthService(
            IUserRepository usuarioRepository,
            ITokenService tokenService,
            IPasswordHasherService passwordHasher,
            IRefreshTokenRepository refreshTokenRepository)
        {
            _usuarioRepository = usuarioRepository;
            _tokenService = tokenService;
            _passwordHasherService = passwordHasher;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task ChangePasswordAsync(int userId, ChangePasswordRequestDto dto)
        {
            if (dto == null)
                throw new Exception("Dados inválidos.");

            var user = await _usuarioRepository.GetByIdAsync(userId);
            if (user == null)
                throw new UnauthorizedAccessException("Usuário inválido.");

            if (string.IsNullOrWhiteSpace(user.PasswordHash) || string.IsNullOrWhiteSpace(user.PasswordSalt))
                throw new Exception("Usuário ainda não possui senha definida.");

            var ok = _passwordHasherService.Verify(dto.CurrentPassword, user.PasswordHash, user.PasswordSalt);
            if (!ok)
                throw new UnauthorizedAccessException("Senha atual inválida.");

            var (hash, salt) = _passwordHasherService.HashPassword(dto.NewPassword);

            user.PasswordHash = hash;
            user.PasswordSalt = salt;

            await _usuarioRepository.UpdateAsync(user);
            await _usuarioRepository.SaveChangesAsync();
        }

        public async Task SetPasswordAsync(SetPasswordDto dto)
        {
            if (dto == null)
                throw new ArgumentException("Dados inválidos.");

            var email = (dto.Email ?? string.Empty).Trim();

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email é obrigatório.");

            if (string.IsNullOrWhiteSpace(dto.Password))
                throw new ArgumentException("Senha é obrigatória.");

            if (dto.Password != dto.ConfirmPassword)
                throw new ArgumentException("As senhas não conferem.");

            var usuario = await _usuarioRepository.GetByEmailForLoginAsync(email);

            if (usuario == null)
                throw new UnauthorizedAccessException("Usuário não encontrado.");

            if (!usuario.IsActive)
                throw new UnauthorizedAccessException("Usuário inativo.");

            // Impede redefinir senha por esse fluxo
            if (!string.IsNullOrWhiteSpace(usuario.PasswordHash))
                throw new UnauthorizedAccessException("Senha já definida.");

            var (hash, salt) = _passwordHasherService.HashPassword(dto.Password);

            usuario.PasswordHash = hash;
            usuario.PasswordSalt = salt;

            await _usuarioRepository.UpdateAsync(usuario);
            await _usuarioRepository.SaveChangesAsync();
        }


        public async Task<LoginResultDto> LoginAsync(LoginRequestDto dto)
        {
            var usuario = await _usuarioRepository
                .GetByEmailForLoginAsync(dto.Email);

            if (usuario == null)
                throw new UnauthorizedAccessException("Usuário ou senha inválidos");

            // PRIMEIRO ACESSO: ainda não tem senha definida
            if (string.IsNullOrWhiteSpace(usuario.PasswordHash) || string.IsNullOrWhiteSpace(usuario.PasswordSalt))
            {
                return new LoginResultDto
                {
                    UserId = usuario.Id,
                    Name = usuario.Name,
                    Email = usuario.Email,
                    RequiresPasswordSetup = true,
                    AccessToken = string.Empty,
                    RefreshToken = string.Empty
                };
            }

            var passwordOk = _passwordHasherService.Verify(
                dto.Password,
                usuario.PasswordHash,
                usuario.PasswordSalt
            );

            if (!passwordOk)
                throw new UnauthorizedAccessException("Usuário ou senha inválidos");

            var accessToken = _tokenService.GenerateAccessToken(usuario);
            var refreshToken = _tokenService.GenerateRefreshToken();

            refreshToken.UserId = usuario.Id;

            await _refreshTokenRepository.AddAsync(refreshToken);

            return new LoginResultDto
            {
                UserId = usuario.Id,
                Name = usuario.Name,
                Email = usuario.Email,
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token,
                RequiresPasswordSetup = false
            };
        }

        public async Task<LoginResultDto> RefreshAsync(string refreshToken)
        {
            var token = await _refreshTokenRepository
                .GetActiveByTokenAsync(refreshToken);

            if (token == null)
                throw new UnauthorizedAccessException("Refresh token inválido");

            if (token.ExpiresAt < DateTime.UtcNow)
                throw new UnauthorizedAccessException("Refresh token expirado");

            var user = token.User;

            var newAccessToken = _tokenService.GenerateAccessToken(user);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            token.Revoke();
            await _refreshTokenRepository.AddAsync(newRefreshToken);

            return new LoginResultDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken.Token
            };
        }
    }
}
