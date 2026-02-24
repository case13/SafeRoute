using AutoMapper;
using SafeRoute.Domain.Entities;
using SafeRoute.Shared.Dtos.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeRoute.Application.Mappings
{
    public class RuleViolationProfile : Profile
    {
        public RuleViolationProfile()
        {
            CreateMap<RuleViolation, ReadRuleViolationDto>();
            CreateMap<CreateRuleViolationDto, RuleViolation>();
            CreateMap<UpdateRuleViolationDto, RuleViolation>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ProjectId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());
        }
    }
}
