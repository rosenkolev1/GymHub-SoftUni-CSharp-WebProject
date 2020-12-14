using AutoMapper;
using GymHub.Common;
using GymHub.Services.ServicesFolder.CategoryService;
using GymHub.Web.AuthorizationPolicies;
using GymHub.Web.Helpers.NotificationHelpers;
using GymHub.Web.Models.InputModels;
using GymHub.Web.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace GymHub.Web.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoryService categoryService;
        private readonly IMapper mapper;

        public CategoriesController(ICategoryService categoryService, IMapper mapper)
        {
            this.categoryService = categoryService;
            this.mapper = mapper;
        }

        [Authorize(Policy = nameof(AuthorizeAsAdminHandler))]
        public async Task<IActionResult> All()
        {
            var allCategories = this.categoryService.GetAllCategories(true);

            var allCategoriesViewModel = this.mapper.Map<List<CategoryViewModel>>(allCategories);

            //THIS IS FOR DEBUGGING PURPOSES CURRENTLY. MAYBE REMOVE THIS LATER
            TempData.Remove(GlobalConstants.InputModelFromPOSTRequest);
            TempData.Remove(GlobalConstants.InputModelFromPOSTRequestType);
            TempData.Remove(GlobalConstants.ErrorsFromPOSTRequest);

            return this.View(allCategoriesViewModel);
        }

        [Authorize(Policy = nameof(AuthorizeAsAdminHandler))]
        public async Task<IActionResult> Create()
        {
            if(TempData[GlobalConstants.ErrorsFromPOSTRequest] != null)
            {
                ModelStateHelper.MergeModelStates(TempData, this.ModelState);
                var inputModel = JsonSerializer.Deserialize<AddCategoryInputModel>(TempData[GlobalConstants.InputModelFromPOSTRequest].ToString());
            }

            return this.View();
        }

        [Authorize(Policy = nameof(AuthorizeAsAdminHandler))]
        [HttpPost]
        public async Task<IActionResult> Create(AddCategoryInputModel inputModel)
        {
            if(this.categoryService.CategoryNameExists(inputModel.Name, true) == true)
            {
                this.ModelState.AddModelError("Name", "This category name has been already taken. Choose a different one");
            }

            if(this.ModelState.IsValid == false)
            {
                TempData[GlobalConstants.ErrorsFromPOSTRequest] = ModelStateHelper.SerialiseModelState(this.ModelState);
                TempData[GlobalConstants.InputModelFromPOSTRequest] = JsonSerializer.Serialize(inputModel);

                return this.RedirectToAction(nameof(Create));
            }

            await this.categoryService.AddAsync(inputModel.Name);

            return this.RedirectToAction(nameof(All));
        }

        [Authorize(Policy = nameof(AuthorizeAsAdminHandler))]
        public async Task<IActionResult> Edit(string categoryId)
        {
            EditCategoryInputModel inputModel;

            if (TempData[GlobalConstants.ErrorsFromPOSTRequest] != null)
            {
                ModelStateHelper.MergeModelStates(TempData, this.ModelState);
                inputModel = JsonSerializer.Deserialize<EditCategoryInputModel>(TempData[GlobalConstants.InputModelFromPOSTRequest].ToString());
            }
            else
            {
                //Check if category exists
                if(this.categoryService.CategoryExists(categoryId) == false)
                {
                    return this.NotFound();
                }

                var category = this.categoryService.GetCategoryById(categoryId);
                inputModel = this.mapper.Map<EditCategoryInputModel>(category);
            }

            return this.View(inputModel);
        }

        [Authorize(Policy = nameof(AuthorizeAsAdminHandler))]
        [HttpPost]
        public async Task<IActionResult> Edit(EditCategoryInputModel inputModel)
        {
            if (this.categoryService.CategoryExists(inputModel.Id, true) == false)
            {
                this.ModelState.AddModelError("Id", "This category doesn't exist.");
            }

            if (this.categoryService.CategoryNameExists(inputModel.Name, inputModel.Id, true) == true)
            {
                this.ModelState.AddModelError("Name", "This category name has been already taken. Choose a different one");
            }

            if (this.ModelState.IsValid == false)
            {
                TempData[GlobalConstants.ErrorsFromPOSTRequest] = ModelStateHelper.SerialiseModelState(this.ModelState);
                TempData[GlobalConstants.InputModelFromPOSTRequest] = JsonSerializer.Serialize(inputModel);

                return this.RedirectToAction(nameof(Edit), new { categoryId = inputModel.Id});
            }

            await this.categoryService.EditAsync(inputModel.Id, inputModel.Name);

            return this.RedirectToAction(nameof(All));
        }

        [Authorize(Policy = nameof(AuthorizeAsAdminHandler))]
        [HttpPost]
        public async Task<IActionResult> Delete(string categoryId)
        {
            if (this.categoryService.CategoryExists(categoryId) == false)
            {
                //Set notification
                NotificationHelper.SetNotification(TempData, NotificationType.Error, "Category doesn't exist");

                return this.RedirectToAction(nameof(All));
            }

            var products = this.categoryService.GetProductsForCategory(categoryId);

            if (products.Count > 0)
            {
                //Set notification
                NotificationHelper.SetNotification(TempData, NotificationType.Error, "This category could not be deleted because it is assigned to one or more products. Either remove all the products which use it or edit the category instead.");

                return this.RedirectToAction(nameof(All));
            }

            await this.categoryService.RemoveAsync(categoryId);

            return this.RedirectToAction(nameof(All));
        }

        [Authorize(Policy = nameof(AuthorizeAsAdminHandler))]
        [HttpPost]
        public async Task<IActionResult> Restore(string categoryId)
        {
            if (this.categoryService.CategoryExists(categoryId, true) == false)
            {
                return this.View("/Views/Shared/Error.cshtml");
            }

            await this.categoryService.RestoreAsync(categoryId);

            return this.RedirectToAction(nameof(All));
        }
    }
}
