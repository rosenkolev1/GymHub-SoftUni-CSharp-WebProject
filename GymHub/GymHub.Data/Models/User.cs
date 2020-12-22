using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymHub.Data.Models
{
    public class User : IdentityUser<string>, IDeletableEntity
    {
        public User()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        //Foreign Keys
        [Required]
        [ForeignKey("Gender")]
        public string GenderId { get; set; }
        public virtual Gender Gender { get; set; }

        //Collections
        public virtual ICollection<UserOccupation> Occupations { get; set; }
        public virtual ICollection<ProductCart> ProductsCart { get; set; }
        public virtual ICollection<Sale> Sales { get; set; }
        public virtual ICollection<ProductComment> ProductsComments { get; set; }
        public virtual ICollection<ProductRating> ProductsRatings { get; set; }
        public virtual ICollection<UserImage> Pictures { get; set; }
        public virtual ICollection<ProductCommentLike> ProductsCommentsLikes { get; set; }
        public virtual ICollection<ContactsChatMessage> ContactsChatMessagesSent { get; set; }
        public virtual ICollection<ContactsChatMessage> ContactsChatMessagesReceived { get; set; }

        //Identity Collections
        public virtual ICollection<IdentityUserRole<string>> Roles { get; set; }
        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }
        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }

        //Simple properties
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(50)]
        public string MiddleName { get; set; }
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }
        public DateTime? RegisteredOn { get; set; }
        public string ProfilePicture { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        public string Description { get; set; }
        [Required]
        public bool IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
    }
}
