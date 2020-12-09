using GymHub.Web.Models.InputModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GymHub.Web.Controllers
{
    [Authorize]
    public class SalesController : Controller
    {
        public SalesController()
        {

        }

        public async Task<IActionResult> Checkout(List<BuyProductInputModel> inputModels)
        {
            return this.View(inputModels);
        } 
    }
}
