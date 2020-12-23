using GymHub.Data.Models;
using GymHub.Web.Models.InputModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymHub.Web.Models.ViewModels
{
    public class ChatViewModel
    {
        public List<MessageViewModel> Messages { get; set; }

        public MessageInputModel MessageInputModel { get; set; }

        public int MessagesCount { get; set; }

        public string CurrentUserId { get; set; }

        public string TargetUserId { get; set; }

        public User CurrentUser { get; set; }

        public User TargetUser { get; set; }

        public List<User> AllUsers { get; set; } 

        public string UserSearch { get; set; }

        public string UnreadMessagesCount { get; set; }
    }
}
