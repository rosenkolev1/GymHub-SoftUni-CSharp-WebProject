using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GymHub.Data.Models
{
    public class Gender
    {
        public Gender()
        {
            this.Id = Guid.NewGuid().ToString();
        }
        [Key]
        public string Id { get; set; }

        //Collections
        public virtual ICollection<User> Users { get; set; }

        //Simple properties
        [Required]
        public string Name { get; set; }
    }
}