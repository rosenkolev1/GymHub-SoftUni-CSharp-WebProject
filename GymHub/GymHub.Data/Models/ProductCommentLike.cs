using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GymHub.Data.Models
{
    public class ProductCommentLike
    {
        [ForeignKey(nameof(ProductComment))]
        public string ProductCommentId{ get; set; }
        public virtual ProductComment ProductComment { get; set; }
        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
        public virtual User User { get; set; }
    }
}
