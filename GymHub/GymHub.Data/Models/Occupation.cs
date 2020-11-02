using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GymHub.Data.Models
{
    public class Occupation
    {
        public Occupation()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Key]
        public string Id { get; set; }

        //Collections
        public virtual ICollection<UserOccupation> Users { get; set; }

        //Simple properties
        [Required]
        public string Name { get; set; }
    }
}
