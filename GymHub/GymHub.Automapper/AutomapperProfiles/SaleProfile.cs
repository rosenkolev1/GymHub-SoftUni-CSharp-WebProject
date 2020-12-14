using AutoMapper;
using GymHub.Data.Models;
using GymHub.Web.Models.InputModels;

namespace GymHub.Automapper.AutomapperProfiles
{
    public class SaleProfile : Profile
    {
        public SaleProfile()
        {
            CreateMap<CheckoutInputModel, Sale>();
        }
    }
}
