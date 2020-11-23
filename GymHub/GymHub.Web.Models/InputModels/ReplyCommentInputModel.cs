using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GymHub.Web.Models.InputModels
{
    public class ReplyCommentInputModel
    {
        public ReplyCommentInputModel()
        {

        }
        public ReplyCommentInputModel(string productId, string commentId, int commentCounter)
        {
            this.ProductId = productId;
            this.CommentId = commentId;
            this.CommentCounter = commentCounter;
        }

        //Used for validation purposes
        public int CommentCounter { get; set; }

        [Required]
        public string Text { get; set; }

        [Required(ErrorMessage = "Product is required")]
        public string ProductId { get; set; }

        [Required(ErrorMessage = "Comment is required")]
        public string CommentId { get; set; }
    }
}
