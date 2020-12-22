using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymHub.Web.Models.ViewModels
{
    public class MessageViewModel
    {
        public DateTime SendOn { get; set; }

        public string Text { get; set; }

        public bool BelongToUser { get; set; }
    }
}
