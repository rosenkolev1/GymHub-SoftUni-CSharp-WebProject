using System;
using System.ComponentModel.DataAnnotations;

namespace GymHub.Data
{
    public interface IDeletableEntity
    {
        [Required]
        public bool IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
    }
}
