using GymHub.Data.Models;
using GymHub.Web.Models.InputModels;
using GymHub.Web.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymHub.Services.ServicesFolder.ContactsChatService
{
    public interface IContactsChatService
    {
        public ChatViewModel GetChatInfo(User currentUser, User targetUser);

        public Task<ContactsChatMessage> AddMessages(MessageInputModel inputModel);

        public List<User> GetUsersForAdmin(User adminUser);

        public ContactsChatMessage GetMessageById(string messageId);

        public Task MarkAsSeenAsync(ContactsChatMessage message);

        public Task MarkAllForReceiverAsSeenAsync(User sender, User receiver);

        public int GetNumberOfUnseenForReceiver(User sender, User receiver);

        public int GetNumberOfUnseenForReceiver(User receiver);

        public User GetRandomUserWhoHasMessagedAdmin();
    }
}
