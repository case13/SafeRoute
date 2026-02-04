using SafeRoute.Application.Services.Interfaces;
using SafeRoute.Domain.Currents.Interfaces;
using SafeRoute.Domain.Entities;
using SafeRoute.Domain.Repositories.Interfaces;
using SafeRoute.Shared.Dtos.Common;
using SafeRoute.Shared.Dtos.Ingestion.Project;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SafeRoute.Application.Services.Implementations
{
    public class ProjectIngestionService : IProjectIngestionService
    {
        private readonly IRuleViolationRepository _repository;
        private readonly IProjectRepository _projectRepository;
        private readonly ICurrentCompany _currentCompany;
        private readonly IRulesEngineService _rulesEngineService;

        public ProjectIngestionService(
            IRuleViolationRepository repository,
            IProjectRepository projectRepository,
            ICurrentCompany currentCompany,
            IRulesEngineService rulesEngineService)
        {
            _repository = repository;
            _projectRepository = projectRepository;
            _currentCompany = currentCompany;
            _rulesEngineService = rulesEngineService;
        }

        public async Task<ReplaceResultDto> ReplaceByProjectAsync(IngestProjectElementsRequestDto dto)
        {
            if (dto == null)
                return new ReplaceResultDto();

            var projectId = await ResolveProjectIdAsync(dto);
            if (projectId <= 0)
                return new ReplaceResultDto();

            var generated = await _rulesEngineService.EvaluateAsync(dto);

            var insertedBySeverity = new Dictionary<int, int>();
            foreach (var v in generated)
            {
                var key = (int)v.Severity;

                if (!insertedBySeverity.ContainsKey(key))
                    insertedBySeverity[key] = 0;

                insertedBySeverity[key]++;
            }

            var deleted = await _repository.DeleteByProjectIdAsync(projectId);

            var entities = new List<RuleViolation>();
            foreach (var v in generated)
            {
                entities.Add(new RuleViolation
                {
                    ProjectId = projectId,
                    ElementExternalId = (v.ElementExternalId ?? string.Empty).Trim(),
                    ElementType = (v.ElementType ?? string.Empty).Trim(),
                    RuleCode = (v.RuleCode ?? string.Empty).Trim(),
                    Message = (v.Message ?? string.Empty).Trim(),
                    Severity = v.Severity
                });
            }

            if (entities.Count > 0)
                await _repository.AddRangeAsync(entities);

            await _repository.SaveChangesAsync();

            return new ReplaceResultDto
            {
                ProjectId = projectId,
                Deleted = deleted,
                Inserted = entities.Count,
                InsertedBySeverity = insertedBySeverity
            };
        }

        private async Task<int> ResolveProjectIdAsync(IngestProjectElementsRequestDto dto)
        {
            if (dto.ProjectId.HasValue && dto.ProjectId.Value > 0)
            {
                var project = await _projectRepository.GetByIdAsync(dto.ProjectId.Value);
                return project != null ? project.Id : 0;
            }

            if (string.IsNullOrWhiteSpace(dto.ProjectExternalId))
                return 0;

            var externalId = dto.ProjectExternalId.Trim();

            var existing = await _projectRepository.GetByExternalIdAsync(externalId);
            if (existing != null && existing.Id > 0)
                return existing.Id;

            var companyId = _currentCompany.CompanyId;
            if (companyId <= 0)
                return 0;

            if (string.IsNullOrWhiteSpace(dto.ProjectName))
                return 0;

            var created = new Project
            {
                Name = dto.ProjectName.Trim(),
                ExternalId = externalId,
                CompanyId = companyId
            };

            await _projectRepository.AddAsync(created);
            await _projectRepository.SaveChangesAsync();

            return created.Id;
        }
    }
}
