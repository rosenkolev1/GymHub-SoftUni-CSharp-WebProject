﻿using AutoMapper;
using GymHub.Web.Models.ViewModels;
using GymHub.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace GymHub.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService productService;
        private readonly IMapper mapper;
        public ProductsController(IProductService productService, IMapper mapper)
        {
            this.productService = productService;
            this.mapper = mapper;
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
            var viewModel = mapper.Map<ProductInfoViewModel>(product);
            return this.View(viewModel);
        }

    }
}
