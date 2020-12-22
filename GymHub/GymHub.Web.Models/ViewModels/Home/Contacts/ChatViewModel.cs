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

        public int SentMessagesCount { get; set; }
    }
}
