using AutoMapper;
using GymHub.Data.Models;
using GymHub.Web.Models.InputModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymHub.Automapper.AutomapperProfiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, EditCategoryInputModel>();
        }
    }
}
