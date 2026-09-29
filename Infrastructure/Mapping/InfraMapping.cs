using Application.Features.Authentication.DTOs;
using AutoMapper;
using Infrastructure.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Mapping
{
    public class InfraMapping : Profile
    {
        public InfraMapping()
        {
            CreateMap<RegisterDto, ApplicationUser>();

            CreateMap<ApplicationUser, UserDto>()
            .ForCtorParam("UserId",
                opt => opt.MapFrom(src => src.PublicId))
            .ForCtorParam("DisplayName",
            opt => opt.MapFrom(src => src.FirstName + " " + src.LastName))
            .ForCtorParam("Email",
            opt => opt.MapFrom(src => src.Email));

            //CreateMap<ApplicationUser, UserInfo()
            //.ForCtorParam("Project",
            //    opt => opt.MapFrom(src => src.Project))
            ////.ForCtorParam("Roles",opt => opt.ignore())
            //.ForCtorParam("DisplayName",
            //opt => opt.MapFrom(src => src.FirstName + " " + src.LastName))
            //.ForCtorParam("Roles", opt => opt.MapFrom([]);

            //.ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.FirstName + " " + src.LastName));

        }
    }
}
