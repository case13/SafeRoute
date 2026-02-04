using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using SafeRoute.Application.Services.Interfaces;
using SafeRoute.Domain.Currents.Interfaces;
using SafeRoute.Domain.Entities;
using SafeRoute.Domain.Repositories.Interfaces;
using SafeRoute.Shared.Dtos.Common;
using SafeRoute.Shared.Dtos.Project;
using SafeRoute.Shared.Dtos.User;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SafeRoute.Application.Services.Implementations
{
    public class ProjectService
        : BaseService<Project, ReadProjectDto, CreateProjectDto, UpdateProjectDto>,
          IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly ICurrentCompany _currentCompany;

        public ProjectService(
            IProjectRepository projectRepository,
            ICurrentCompany currentCompany,
            IMapper mapper)
            : base(projectRepository, mapper)
        {
            _projectRepository = projectRepository;
            _currentCompany = currentCompany;
        }

        public async Task<PagedResultDto<ReadProjectDto>> GetPagedAsync(
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
                    "externalid" => query.Where(x => x.ExternalId.ToLower().Contains(filterText)),
                    _ => query
                };
            }
            var totalCount = await query.CountAsync();

            var items = await query
                .OrderBy(p => p.Name) // ou p.Id, como preferir
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ProjectTo<ReadProjectDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return new PagedResultDto<ReadProjectDto>
            {
                Items = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }

        public override async Task<ReadProjectDto> CreateAsync(CreateProjectDto dto)
        {
            try
            {
                var companyId = _currentCompany.CompanyId;
                if (companyId <= 0)
                    return new ReadProjectDto();

                var externalId = string.IsNullOrWhiteSpace(dto.ExternalId)
                    ? null
                    : dto.ExternalId.Trim();

                if (!string.IsNullOrWhiteSpace(externalId))
                {
                    var exists = await _projectRepository.ExistsByExternalIdAsync(externalId);
                    if (exists)
                        return new ReadProjectDto();
                }

                var entity = _mapper.Map<Project>(dto);
                if (entity == null)
                    return new ReadProjectDto();

                entity.CompanyId = companyId;
                entity.ExternalId = externalId;

                await _projectRepository.AddAsync(entity);
                await _projectRepository.SaveChangesAsync();

                return _mapper.Map<ReadProjectDto>(entity);
            }
            catch (Exception)
            {
                return new ReadProjectDto();
            }
        }

        public override async Task<ReadProjectDto> UpdateAsync(int id, UpdateProjectDto dto)
        {
            try
            {
                var entity = await _projectRepository.Query(asNoTracking: false)
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (entity == null)
                    return new ReadProjectDto();

                var externalId = string.IsNullOrWhiteSpace(dto.ExternalId)
                    ? null
                    : dto.ExternalId.Trim();

                if (!string.IsNullOrWhiteSpace(externalId))
                {
                    var exists = await _projectRepository.Query()
                        .AnyAsync(x => x.ExternalId == externalId && x.Id != id);

                    if (exists)
                        return new ReadProjectDto();
                }

                _mapper.Map(dto, entity);
                entity.ExternalId = externalId;

                await _projectRepository.UpdateAsync(entity);
                await _projectRepository.SaveChangesAsync();

                return _mapper.Map<ReadProjectDto>(entity);
            }
            catch (Exception)
            {
                return new ReadProjectDto();
            }
        }
    }
}
