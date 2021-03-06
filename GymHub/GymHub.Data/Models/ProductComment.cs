﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymHub.Data.Models
{
    public class ProductComment : IDeletableEntity
    {
        public ProductComment(string id)
        {
            this.Id = id;
        }
        public ProductComment()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Key]
        public string Id { get; set; }

        //Collections
        public virtual ICollection<ProductCommentLike> CommentLikes { get; set; }

        //Foreign Keys
        [ForeignKey("User")]
        [Required]
        public string UserId { get; set; }
        public virtual User User { get; set; }
        [ForeignKey("ParentComment")]
        public string ParentCommentId { get; set; }
        public virtual ProductComment ParentComment { get; set; }
        [ForeignKey("Product")]
        [Required]
        public string ProductId { get; set; }
        public virtual Product Product { get; set; }

        [ForeignKey("ProductRating")]
        public string ProductRatingId { get; set; }

        public virtual ProductRating ProductRating { get; set; }


        //Simple properties
        [Required]
        public string Text { get; set; }
        [Required]
        public DateTime CommentedOn { get; set; }
        [Required]
        public bool IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
