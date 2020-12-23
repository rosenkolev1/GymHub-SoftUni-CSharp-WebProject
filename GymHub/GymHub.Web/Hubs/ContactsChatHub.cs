using GymHub.Services;
using GymHub.Services.ServicesFolder.ContactsChatService;
using GymHub.Web.Models.InputModels;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace GymHub.Web.Hubs
{
    public class ContactsChatHub : Hub
    {
        private readonly IUserService userService;
        private readonly IContactsChatService contactsChatService;

        public ContactsChatHub(IUserService userService, IContactsChatService contactsChatService)
        {
            this.userService = userService;
            this.contactsChatService = contactsChatService;
        }

        public async Task Send(string messageText, string receiverId)
        {
            var sender = this.userService.GetUserByUsername(this.Context.User.Identity.Name);
            var receiver = this.userService.GetUser(receiverId);

            //TODO: Add validation

            var messageInputModel = new MessageInputModel
            {
                Message = messageText,
                SentOn = DateTime.UtcNow,
                SenderId = sender.Id,
                ReceiverId = receiverId,
                Sender = sender,
                Receiver = receiver
            };

            await this.contactsChatService.AddMessages(messageInputModel);

            await this.Clients.All.SendAsync("NewMessage", messageInputModel);
        }
    }
}
