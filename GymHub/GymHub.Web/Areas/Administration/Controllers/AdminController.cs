using GymHub.Web.AuthorizationPolicies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GymHub.Web.Areas.Administration.Controllers
{
    [Authorize(Policy = nameof(AuthorizeAsAdminHandler))]
    [Area("Administration")]
    public class AdminController : Controller
    {
        public IActionResult AdminControls()
        {
            return this.View();
        }
    }
}
