using AutoMapper;
using GymHub.Services;
using GymHub.Web.Models.ViewModels;
using GymHub.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GymHub.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService productService;
        private readonly IProductCommentService productCommentService;
        private readonly IMapper mapper;
        public ProductsController(IProductService productService, IMapper mapper, IProductCommentService productCommentService)
        {
            this.productService = productService;
            this.mapper = mapper;
            this.productCommentService = productCommentService;
        }

        [Authorize]
        public async Task<IActionResult> All()
        {
            var products = await this.productService.GetAllProductsAsync();
            foreach (var product in products)
            {
                product.ShortDescription = this.productService.GetShordDescription(product.Description, 40);
            }
            return this.View(products);
        }

        [Authorize]
        public async Task<IActionResult> ProductPage(string productId)
        {
            var product = await this.productService.GetProductByIdAsync(productId);
            var viewModel = mapper.Map<ProductInfoViewModel>(product);

            //Short description
            viewModel.ShortDescription = this.productService.GetShordDescription(viewModel.Description, 40);

            //Overall product rating
            viewModel.ProductRating = new ProductRatingViewModel(this.productService.GetAverageRating(viewModel.ProductRatings));

            //Product rating for each user
            foreach (var userProductRating in viewModel.ProductRatings)
            {
                viewModel.UsersProductRatings.Add(userProductRating.User, new ProductRatingViewModel(userProductRating.Rating));
            }

            //Product comments
            foreach (var comment in viewModel.ProductComments)
            {
                if(comment.ParentCommentId == null)
                {
                    var childComments = await this.productCommentService.GetAllChildCommentsAsync(comment);
                    viewModel.ParentsChildrenComments.Add(comment, childComments);
                }
            }
            viewModel.ParentsChildrenComments.OrderBy(kv => kv.Key.CommentedOn);
            foreach (var kv in viewModel.ParentsChildrenComments)
            {
                var childrenComments = kv.Value;
                childrenComments.OrderBy(x => x.CommentedOn);
            }

            return this.View(viewModel);
        }

    }
}
