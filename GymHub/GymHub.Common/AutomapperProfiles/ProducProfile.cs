using AutoMapper;
using GymHub.Data.Models;
using GymHub.Web.Models.InputModels;
using GymHub.Web.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymHub.Common.AutomapperProfiles
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
