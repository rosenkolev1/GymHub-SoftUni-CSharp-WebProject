using AutoMapper;
using GymHub.Data.Models;
using GymHub.DTOs;
using GymHub.Web.Models.InputModels;
using System;

namespace GymHub.Automapper.AutomapperProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<RegisterUserInputModel, User>()
                .ForMember(dest => 
                    dest.RegisteredOn,
                    opt => opt.NullSubstitute(DateTime.UtcNow));

            CreateMap<UserDTO, User>()
                .ForMember(dest =>
                    dest.RegisteredOn,
                    opt => opt.NullSubstitute(DateTime.UtcNow)); 
        }
    }
}
