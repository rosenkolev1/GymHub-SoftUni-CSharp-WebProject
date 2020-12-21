using GymHub.Common;
using GymHub.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GymHub.Web.Controllers.BaseControllers
{
    public class BaseSaleController : Controller
    {
        protected readonly List<SaleFilterOptionViewModel> allSaleFilterOptions = new List<SaleFilterOptionViewModel>          
        {
            new SaleFilterOptionViewModel {FilterName = GlobalConstants.IncludeConfirmed, FilterValue = true },
            new SaleFilterOptionViewModel {FilterName = GlobalConstants.IncludePending, FilterValue = true },
            new SaleFilterOptionViewModel { FilterName = GlobalConstants.IncludeDeclined, FilterValue = true },
            new SaleFilterOptionViewModel { FilterName = GlobalConstants.IncludeRefunded, FilterValue = true },
        };
    }
}
