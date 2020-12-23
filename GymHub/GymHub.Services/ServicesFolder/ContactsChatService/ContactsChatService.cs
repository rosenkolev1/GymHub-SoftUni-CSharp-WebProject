using GymHub.Common;
using GymHub.Data.Data;
using GymHub.Data.Models;
using GymHub.Services.ServicesFolder.RoleService;
using GymHub.Web.Models.InputModels;
using GymHub.Web.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymHub.Services.ServicesFolder.ContactsChatService
{
    public class ContactsChatService : IContactsChatService
    {
        private readonly ApplicationDbContext context;
        private readonly IUserService userService;

        public ContactsChatService(ApplicationDbContext context, IUserService userService)
        {
            this.context = context;
            this.userService = userService;
        }

        public async Task AddMessages(MessageInputModel inputModel)
        {
            var newMessage = new ContactsChatMessage
            {
                Sender = inputModel.Sender,
                SentOn = inputModel.SentOn,
                Receiver = inputModel.Receiver,
                Text = inputModel.Message
            };

            await this.context.ContactsChatMessages.AddAsync(newMessage);
            await this.context.SaveChangesAsync();
        }

        //TODO: REMOVE CurrentUserId and TargetUserId from the chat view model if they are not needed
        public ChatViewModel GetChatInfo(User currentUser, User targetUser)
        {
            var targetUserName = targetUser.UserName;
            var adminRoleId = this.context.Roles.First(x => x.Name == GlobalConstants.AdminRoleName).Id;

            var messages = this.context.ContactsChatMessages
                .Where(x =>
                (x.Sender.UserName == currentUser.UserName && x.Receiver.UserName == targetUserName) || (x.Sender.UserName == targetUserName && x.Receiver.UserName == currentUser.UserName))
                .Select(x => new MessageViewModel
                {
                    SentOn = x.SentOn,
                    BelongsToSender = x.Sender.UserName == currentUser.UserName,
                    Text = x.Text,
                    SenderName = x.Sender.UserName,
                    SenderIsAdmin = x.Sender.Roles.Select(y => y.RoleId).Contains(adminRoleId)
                })
                .OrderBy(x => x.SentOn)
                .ToList();

            var chatViewModel = new ChatViewModel
            {
                MessagesCount = messages.Count,
                CurrentUserId = currentUser.Id,
                Messages = messages,
                MessageInputModel = new MessageInputModel(),
                TargetUserId = targetUser.Id,
                TargetUser = targetUser,
                CurrentUser = currentUser
            };

            return chatViewModel;
        }

        public List<User> GetUsersForAdmin(User adminUser)
        {
            return this.context.Users
                .Where(x => x != adminUser && x.ContactsChatMessagesSent.Any(y => y.ReceiverId == adminUser.Id))
                .ToList();
        }
    }
}
