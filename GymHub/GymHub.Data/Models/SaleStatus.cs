using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymHub.Data.Models
{
    public class SaleStatus
    {
        public SaleStatus()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Key]
        public string Id { get; set; }

        [Required]

        public string Name { get; set; }

        //Collections
        public virtual ICollection<Sale> Sales { get; set; }
    }
}
