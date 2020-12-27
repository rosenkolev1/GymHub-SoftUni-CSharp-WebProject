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

            if(sender == null || receiver == null)
            {
                throw new Exception("The sender or the receiver doesn't exist");
            }

            var messageInputModel = new MessageInputModel
            {
                Message = messageText,
                SentOn = DateTime.UtcNow,
                SenderId = sender.Id,
                ReceiverId = receiverId,
            };

            try
            {
                var newMessage = await this.contactsChatService.AddMessages(messageInputModel);

                messageInputModel.MessageId = newMessage.Id;

                await this.Clients.Caller.SendAsync("NewMessage", messageInputModel);

                var receiverClientProxy = this.Clients.User(receiverId);
                await receiverClientProxy.SendAsync("NewMessage", messageInputModel);
            }
            catch
            {
                throw new Exception("Something went wrong with the method internally");
            }
        }
    }
}
