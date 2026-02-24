using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using SafeRoute.Application.Services.Interfaces;
using SafeRoute.Domain.Entities;
using SafeRoute.Domain.Repositories.Interfaces;
using SafeRoute.Shared.Dtos.Common;
using SafeRoute.Shared.Dtos.Company;

namespace SafeRoute.Application.Services.Implementations
{
    public class CompanyService : BaseService<Company, ReadCompanyDto, CreateCompanyDto, UpdateCompanyDto>, ICompanyService
    {
        private readonly ICompanyRepository _repository;
        private readonly IMapper _mapper;

        public CompanyService(ICompanyRepository repository, IMapper mapper): base(repository, mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PagedResultDto<ReadCompanyDto>> GetPagedAsync(
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
                    "registry" => query.Where(x => x.Registry.ToLower().Contains(filterText)),
                    "legalname" => query.Where(x => x.LegalName.ToLower().Contains(filterText)),
                    "name" => query.Where(x => x.Name.ToLower().Contains(filterText)),
                    "statuscompany" => query.Where(x => x.StatusCompany.ToString().ToLower().Contains(filterText)),
                    _ => query
                };
            }
            var totalCount = await query.CountAsync();

            var items = await query
                .OrderBy(p => p.Name) // ou p.Id, como preferir
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ProjectTo<ReadCompanyDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return new PagedResultDto<ReadCompanyDto>
            {
                Items = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }

        public async Task<ReadCompanyDto> GetByRegistryAsync(string registry)
        {
            try
            {
                var company = await _repository.GetByRegistryAsync(registry);
                if (company == null)
                    return null;
                return _mapper.Map<ReadCompanyDto>(company);
            }
            catch (Exception)
            {
                return new ReadCompanyDto();
            }
        }

    }
}
