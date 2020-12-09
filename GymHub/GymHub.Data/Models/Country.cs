using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymHub.Data.Models
{
    public class Country
    {
        public Country()
        {
            this.Id = Guid.NewGuid().ToString();
        }
        [Key]
        public string Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }
    }
}
