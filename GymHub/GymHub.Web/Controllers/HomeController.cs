using GymHub.Common;
using GymHub.Services;
using GymHub.Services.Messaging;
using GymHub.Web.Helpers.NotificationHelpers;
using GymHub.Web.Models.InputModels;
using GymHub.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;

namespace GymHub.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SendGridEmailSender sendGridEmailSender;
        private readonly IUserService userService;

        public HomeController(ILogger<HomeController> logger, SendGridEmailSender sendGridEmailSender, IUserService userService)
        {
            _logger = logger;
            this.sendGridEmailSender = sendGridEmailSender;
            this.userService = userService;
        }

        public IActionResult Index()
        {


            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        public IActionResult Contacts()
        {


            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Contacts(string id)
        {
            return this.Redirect(nameof(Contacts));
        }
    }
}
