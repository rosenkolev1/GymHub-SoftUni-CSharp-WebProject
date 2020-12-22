using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymHub.Data.Models
{
    public class ContactsChatMessage
    {
        public ContactsChatMessage()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Key]
        public string Id { get; set; }

        [Required]
        public string Text { get; set; }
        [Required]
        public DateTime SentOn { get; set; }

        //ForeignKeys

        [ForeignKey(nameof(Sender))]
        [Required]
        public string SenderId { get; set; }
        public virtual User Sender { get; set; }

        [ForeignKey(nameof(Receiver))]
        [Required]
        public string ReceiverId { get; set; }
        public virtual User Receiver { get; set; }
    }
}
