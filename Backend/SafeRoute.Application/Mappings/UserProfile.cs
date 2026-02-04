using AutoMapper;
using SafeRoute.Domain.Entities;
using SafeRoute.Shared.Dtos.Company;
using SafeRoute.Shared.Dtos.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeRoute.Application.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, ReadUserDto>().ReverseMap();

            CreateMap<CreateUserDto, User>()
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
            .ForMember(dest => dest.PasswordSalt, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.IsActive, opt => opt.Ignore())
            .ForMember(dest => dest.Email,
                opt => opt.MapFrom(src => src.Email.Trim().ToLower()));

            CreateMap<UpdateUserDto, User>()
               .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}
