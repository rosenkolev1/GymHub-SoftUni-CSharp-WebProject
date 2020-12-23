using GymHub.Common;
using GymHub.Data.Models;
using GymHub.Services;
using GymHub.Services.Messaging;
using GymHub.Services.ServicesFolder.ContactsChatService;
using GymHub.Web.Helpers.NotificationHelpers;
using GymHub.Web.Models.InputModels;
using GymHub.Web.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace GymHub.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SendGridEmailSender sendGridEmailSender;
        private readonly IUserService userService;
        private readonly IContactsChatService contactsChatService;
        private readonly UserManager<User> userManager;

        public HomeController(ILogger<HomeController> logger, SendGridEmailSender sendGridEmailSender, IUserService userService, IContactsChatService contactsChatService,
            UserManager<User> userManager)
        {
            _logger = logger;
            this.sendGridEmailSender = sendGridEmailSender;
            this.userService = userService;
            this.contactsChatService = contactsChatService;
            this.userManager = userManager;
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

        public async Task<IActionResult> Contacts(string targetUserId, string userSearch)
        {
            var currentUserIsAdmin = this.User.IsInRole(GlobalConstants.AdminRoleName) == true;
            User targetUser = null;

            if (currentUserIsAdmin == false) targetUser = this.userService.GetAdminUser();
            else
            {
                targetUser = this.userService.GetUser(targetUserId);
            }

            if (targetUser == null && currentUserIsAdmin) targetUser = this.contactsChatService.GetRandomUserWhoHasMessagedAdmin();

            var currentUser = this.userService.GetUserByUsername(this.User.Identity.Name);

            var chatViewModel = this.contactsChatService.GetChatInfo(currentUser, targetUser);

            if (currentUserIsAdmin)
            {
                chatViewModel.AllUsers = this.contactsChatService.GetUsersForAdmin(currentUser)
                    .OrderByDescending(x => x == targetUser).ToList();

                if (string.IsNullOrWhiteSpace(userSearch) == false)
                {
                    chatViewModel.AllUsers = chatViewModel.AllUsers
                    .OrderByDescending(x => x.UserName.Contains(userSearch) || x.FirstName.Contains(userSearch) || x.LastName.Contains(userSearch))
                    .ToList();

                    chatViewModel.UserSearch = userSearch;
                }
            }

            //Mark all the messages from the target user as read
            await this.contactsChatService.MarkAllForReceiverAsSeenAsync(targetUser, currentUser);

            return this.View(chatViewModel);
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> LoadMessage(MessageViewModel messageViewModel)
        {
            var sender = this.userService.GetUser(messageViewModel.SenderId);
            messageViewModel.SenderName = sender.UserName;
            messageViewModel.SenderIsAdmin = await this.userManager.IsInRoleAsync(sender, GlobalConstants.AdminRoleName);

            return this.PartialView("/Views/Home/_ContactsChatMessagePartial.cshtml", messageViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> MarkAsSeen(string messageId)
        {
            var message = this.contactsChatService.GetMessageById(messageId);

            //TODO add validation here and consequently to the javascript as well
            if (message == null)
            {
                return this.NoContent();
            }

            await this.contactsChatService.MarkAsSeenAsync(message);

            return this.Json("Success");
        }
    }
}
