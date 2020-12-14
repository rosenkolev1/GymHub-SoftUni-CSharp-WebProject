using AutoMapper;
using GymHub.Data.Models;
using GymHub.Web.Models.InputModels;
using GymHub.Web.Models.ViewModels;

namespace GymHub.Automapper.AutomapperProfiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, EditCategoryInputModel>();

            CreateMap<Category, CategoryViewModel>();
        }
    }
}
