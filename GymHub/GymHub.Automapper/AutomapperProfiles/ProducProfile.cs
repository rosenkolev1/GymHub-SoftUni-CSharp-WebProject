using AutoMapper;
using GymHub.Data.Models;
using GymHub.Web.Models.InputModels;
using GymHub.Web.Models.ViewModels;

namespace GymHub.Automapper.AutomapperProfiles
{
    public class ProducProfile : Profile
    {
        public ProducProfile()
        {
            CreateMap<AddProductInputModel, Product>();
            CreateMap<Product, ProductViewModel>();
        }
    }
}
