using GymHub.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymHub.Web.Models.InputModels
{
    public class MessageInputModel
    {
        [Required]
        public string Message { get; set; }

        [Required]
        public DateTime SentOn { get; set; } = DateTime.UtcNow;

        public string SenderId { get; set; }

        public string ReceiverId { get; set; }

        public User Sender { get; set; }
        public User Receiver { get; set; }
    }
}
