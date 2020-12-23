using GymHub.Services;
using GymHub.Services.ServicesFolder.ContactsChatService;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GymHub.Web.ViewComponents
{
    [ViewComponent(Name = "ContactsNewMessagesNumber")]
    public class ContactsNewMessagesNumber : ViewComponent
    {
        private readonly IContactsChatService contactsChatService;
        private readonly IUserService userService;

        public ContactsNewMessagesNumber(IContactsChatService contactsChatService, IUserService userService)
        {
            this.contactsChatService = contactsChatService;
            this.userService = userService;
        }


        public IViewComponentResult Invoke()
        {


            var currentUser = this.userService.GetUserByUsername(this.User.Identity.Name);
            int numberOfUnseenMessagesCurrentUser = this.contactsChatService.GetNumberOfUnseenForReceiver(currentUser);

            return this.View(numberOfUnseenMessagesCurrentUser);
        }
    }
}
