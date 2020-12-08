using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymHub.Web.Models.ViewModels
{
    public class CategoryViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime? DeletedOn { get; set; }
        public bool IsDeleted { get; set; }
    }
}
