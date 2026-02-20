using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using SafeRoute.Application.Services.Interfaces;
using SafeRoute.Domain.Entities;
using SafeRoute.Domain.Repositories.Interfaces;
using SafeRoute.Shared.Dtos.Common;
using SafeRoute.Shared.Dtos.User;
using SafeRoute.Shared.Enums.Status;
using SafeRoute.Shared.Enums.Tipos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeRoute.Application.Services.Implementations
{
    public class UserService
        : BaseService<
            User,
            ReadUserDto,
            CreateUserDto,
            UpdateUserDto>,
          IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IPasswordHasherService _passwordHasher;


        public UserService(
            IUserRepository repository,
            IMapper mapper,
            IPasswordHasherService passwordHasher)
            : base(repository, mapper)
        {
            _repository = repository;
            _passwordHasher = passwordHasher;
        }

        public async Task<User?> LoginAsync(string email, string senha)
        {
            var user = await _repository.GetByEmailAsync(email.Trim().ToLower());

            if (user == null)
                return null;

            if (!user.IsActive || user.UserStatus != StatusBasicEnum.Ativo)
                return null;

            var senhaValida = _passwordHasher.Verify(
                senha,
                user.PasswordHash,
                user.PasswordSalt
            );

            if (!senhaValida)
                return null;

            return user;
        }


        public override async Task<ReadUserDto> CreateAsync(CreateUserDto dto)
        {
            try
            {
                var existente = await _repository.GetByEmailAsync(dto.Email);
                if (existente != null)
                    throw new Exception("Email já cadastrado.");
                
                var user = _mapper.Map<User>(dto);
                var (hash, salt) = _passwordHasher.HashPassword(dto.Password);

                user.PasswordHash = hash;
                user.PasswordSalt = salt;

                user.CreatedAt = DateTime.UtcNow;
                user.IsActive = true;

                await _repository.AddAsync(user);
                await _repository.SaveChangesAsync();

                return _mapper.Map<ReadUserDto>(user);
            }
            catch
            {
                return new ReadUserDto();
            }
        }


        public async Task<ReadUserDto> GetByEmailAsync(string email)
        {
            try
            {
                var usuario = await _repository.GetByEmailAsync(email);
                if (usuario == null)
                    return new ReadUserDto();

                return _mapper.Map<ReadUserDto>(usuario);
            }
            catch (Exception)
            {
                return new ReadUserDto();
            }
        }

        public async Task<PagedResultDto<ReadUserDto>> GetPagedAsync(
            int pageNumber,
            int pageSize,
            string? filterColumn,
            string? filterText)
        {
            var query = _repository.Query(asNoTracking: true);
            if (!string.IsNullOrWhiteSpace(filterColumn) &&
                !string.IsNullOrWhiteSpace(filterText))
            {
                filterText = filterText.Trim().ToLower();
                query = filterColumn.ToLower() switch
                {
                    "nome" => query.Where(x => x.Name.ToLower().Contains(filterText)),
                    "email" => query.Where(x => x.Email.ToLower().Contains(filterText)),
                    _ => query
                };
            }
            var totalCount = await query.CountAsync();

            var items = await query
                .OrderBy(p => p.Name) // ou p.Id, como preferir
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ProjectTo<ReadUserDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return new PagedResultDto<ReadUserDto>
            {
                Items = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }

    }
}
