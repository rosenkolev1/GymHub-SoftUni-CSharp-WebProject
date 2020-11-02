using GymHub.Web.Models.InputModels;
using GymHub.Web.Models.ViewModels;
using System.Collections.Generic;

namespace GymHub.Web.Services
{
    public interface IProductService
    {
        public void Add(string name, string mainImage, decimal price, string description, int warranty, int quantityInStock);
        public void Add(AddProductInputModel inputModel);
        public List<ProductViewModel> GetAllProducts();
        public bool ProductExists(string id);
    }
}
