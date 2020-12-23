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

        public Task AddMessages(MessageInputModel inputModel);

        public List<User> GetUsersForAdmin(User adminUser);
    }
}
