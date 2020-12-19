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
            CreateMap<Product, ProductViewModel>();

            CreateMap<Product, ProductInfoViewModel>()
                .ForMember(x => x.MainImage, opt => opt.Ignore())
                .ForMember(x => x.AdditionalImages, opt => opt.Ignore());

            CreateMap<AddProductInputModel, Product>()
                .ForMember(x => x.Images, opt => opt.Ignore())
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<Product, EditProductInputModel>();

        }
    }
}
