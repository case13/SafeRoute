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

        public async Task<LoginResultDto> LoginAsync(LoginRequestDto dto)
        {
            var usuario = await _usuarioRepository
                .GetByEmailForLoginAsync(dto.Email);

            if (usuario == null)
                throw new UnauthorizedAccessException("Usuário ou senha inválidos");

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
                RefreshToken = refreshToken.Token
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
