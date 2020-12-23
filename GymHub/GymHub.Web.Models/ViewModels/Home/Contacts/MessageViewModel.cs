using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymHub.Web.Models.ViewModels
{
    public class MessageViewModel
    {
        public DateTime? SentOn { get; set; }

        public string Text { get; set; }

        public bool BelongsToSender { get; set; }

        public bool SenderIsAdmin { get; set; }

        public string SenderName { get; set; }

        public string SenderId { get; set; }
        
        public bool HasBeenSeenByReceiver { get; set; }
    }
}
