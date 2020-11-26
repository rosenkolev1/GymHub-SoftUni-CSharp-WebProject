using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymHub.Data.Models
{
    public class ProductCommentLike : IDeletableEntity
    {
        [ForeignKey(nameof(ProductComment))]
        public string ProductCommentId { get; set; }
        public virtual ProductComment ProductComment { get; set; }
        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
        public virtual User User { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
    }
}
