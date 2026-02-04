using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using SafeRoute.Application.Services.Interfaces;
using SafeRoute.Domain.Entities;
using SafeRoute.Domain.Repositories.Interfaces;
using SafeRoute.Shared.Dtos.Common;
using SafeRoute.Shared.Dtos.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SafeRoute.Application.Services.Implementations
{
    public class RuleViolationService
        : BaseService<RuleViolation, ReadRuleViolationDto, CreateRuleViolationDto, UpdateRuleViolationDto>,
          IRuleViolationService
    {
        private readonly IRuleViolationRepository _repository;
        private readonly IProjectRepository _projectRepository;

        public RuleViolationService(
            IRuleViolationRepository repository,
            IProjectRepository projectRepository,
            IMapper mapper)
            : base(repository, mapper)
        {
            _repository = repository;
            _projectRepository = projectRepository;
        }

        public async Task<PagedResultDto<ReadRuleViolationDto>> GetPagedAsync(
            int projectId,
            int pageNumber,
            int pageSize,
            string? filterColumn,
            string? filterText)
        {
            // Validação única de segurança:
            // se o projeto não for da empresa atual, o ProjectRepository não encontra.
            var project = await _projectRepository.GetByIdAsync(projectId);
            if (project == null || project.Id == 0)
            {
                return new PagedResultDto<ReadRuleViolationDto>
                {
                    Items = new List<ReadRuleViolationDto>(),
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalCount = 0
                };
            }

            var query = _repository.QueryByProject(projectId, asNoTracking: true);

            if (!string.IsNullOrWhiteSpace(filterColumn) &&
                !string.IsNullOrWhiteSpace(filterText))
            {
                var col = filterColumn.Trim().ToLower();
                var text = filterText.Trim().ToLower();

                query = col switch
                {
                    "rulecode" => query.Where(x => x.RuleCode.ToLower().Contains(text)),
                    "elementid" => query.Where(x => x.ElementExternalId.ToLower().Contains(text)),
                    "elementtype" => query.Where(x => x.ElementType.ToLower().Contains(text)),
                    "message" => query.Where(x => x.Message.ToLower().Contains(text)),
                    _ => query
                };
            }

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderByDescending(x => x.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ProjectTo<ReadRuleViolationDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return new PagedResultDto<ReadRuleViolationDto>
            {
                Items = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }

        public override async Task<ReadRuleViolationDto> CreateAsync(CreateRuleViolationDto dto)
        {
            try
            {
                // Valida ProjectId uma vez (segurança)
                var project = await _projectRepository.GetByIdAsync(dto.ProjectId);
                if (project == null || project.Id == 0)
                    return new ReadRuleViolationDto();

                var entity = _mapper.Map<RuleViolation>(dto);
                if (entity == null)
                    return new ReadRuleViolationDto();

                entity.ElementExternalId = dto.ElementExternalId.Trim();
                entity.ElementType = dto.ElementType.Trim();
                entity.RuleCode = dto.RuleCode.Trim();
                entity.Message = dto.Message.Trim();

                await _repository.AddAsync(entity);
                await _repository.SaveChangesAsync();

                return _mapper.Map<ReadRuleViolationDto>(entity);
            }
            catch (Exception)
            {
                return new ReadRuleViolationDto();
            }
        }

        public override async Task<ReadRuleViolationDto> UpdateAsync(int id, UpdateRuleViolationDto dto)
        {
            try
            {
                // Atenção: aqui usamos GetById do repo (que protege por company via Project)
                var entity = await _repository.GetByIdAsync(id);
                if (entity == null)
                    return new ReadRuleViolationDto();

                // Precisamos do entity trackeado para update
                var tracked = await _repository.Query(asNoTracking: false)
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (tracked == null)
                    return new ReadRuleViolationDto();

                _mapper.Map(dto, tracked);

                await _repository.UpdateAsync(tracked);
                await _repository.SaveChangesAsync();

                return _mapper.Map<ReadRuleViolationDto>(tracked);
            }
            catch (Exception)
            {
                return new ReadRuleViolationDto();
            }
        }

        public async Task<int> CreateBatchAsync(List<CreateRuleViolationDto> dtos)
        {
            try
            {
                if (dtos == null || dtos.Count == 0)
                    return 0;

                // Você pode decidir aceitar lote apenas de um projeto.
                // Aqui eu valido todos os projetos distintos enviados.
                var projectIds = dtos.Select(x => x.ProjectId).Distinct().ToList();

                foreach (var pid in projectIds)
                {
                    var project = await _projectRepository.GetByIdAsync(pid);
                    if (project == null || project.Id == 0)
                        return 0;
                }

                var entities = _mapper.Map<List<RuleViolation>>(dtos);

                foreach (var e in entities)
                {
                    e.ElementExternalId = e.ElementExternalId.Trim();
                    e.ElementType = e.ElementType.Trim();
                    e.RuleCode = e.RuleCode.Trim();
                    e.Message = e.Message.Trim();
                }

                await _repository.AddRangeAsync(entities);
                await _repository.SaveChangesAsync();

                return entities.Count;
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}
