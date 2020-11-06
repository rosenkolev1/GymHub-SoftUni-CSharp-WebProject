using AutoMapper;
using GymHub.Data.Models;
using GymHub.Web.Models.InputModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymHub.Common.AutomapperProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<RegisterUserInputModel, User>()
                .ForMember(dest => 
                    dest.RegisteredOn,
                    opt => opt.NullSubstitute(DateTime.UtcNow));
        }
    }
}
