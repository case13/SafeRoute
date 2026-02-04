using AutoMapper;
using SafeRoute.Domain.Entities;
using SafeRoute.Shared.Dtos.Company;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeRoute.Application.Mappings
{
    public class CompanyProfile : Profile
    {
        public CompanyProfile()
        {
            CreateMap<Company, ReadCompanyDto>();
            CreateMap<CreateCompanyDto, Company>();
            CreateMap<UpdateCompanyDto, Company>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}
