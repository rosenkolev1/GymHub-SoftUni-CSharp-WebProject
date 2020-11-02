using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymHub.Data.Models
{
    public class UserOccupation
    {
        //Foreign Keys
        [Required]
        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual User User { get; set; }
        [Required]
        [ForeignKey("Occupation")]
        public string OccupationId { get; set; }
        public virtual Occupation Occupation { get; set; }

        //Simple properties
        [Required]
        public bool IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
    }
}