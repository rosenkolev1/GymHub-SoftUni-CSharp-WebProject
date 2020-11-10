﻿using GymHub.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace GymHub.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService productService;
        public ProductsController(IProductService productService)
        {
            this.productService = productService;
        }

        [Authorize]
        public async Task<IActionResult> All()
        {
            var products = await this.productService.GetAllProductsAsync();
            return this.View(products);
        }

        [Authorize]
        public async Task<IActionResult> ProductPage(string productId)
        {
            var product = await this.productService.GetProductByIdAsync(productId);
            return this.View(product);
        }

    }
}
