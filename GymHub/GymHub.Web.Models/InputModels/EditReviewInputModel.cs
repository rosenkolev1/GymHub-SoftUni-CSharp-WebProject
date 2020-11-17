using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GymHub.Web.Models.InputModels
{
    public class EditReviewInputModel
    {
        public EditReviewInputModel()
        {

        }
        public EditReviewInputModel(int rating, string text, string productId, string commentId, bool isParentComment, int commentCounter)
        {
            this.Rating = rating;
            this.Text = text;
            this.ProductId = productId;
            this.CommentId = commentId;
            this.IsParrentComment = isParentComment;
            this.CommentCounter = commentCounter;
        }

        //Used for validation purposes
        public int CommentCounter { get; set; } 

        [Range(1, 10, ErrorMessage = "Rating should be between 1 and 10.")]
        public int? Rating { get; set; }
        public int FullStarsCount => this.Rating != null ? (int)Math.Floor((double)Rating) : 0;
        public int EmptyStarsCount => this.Rating != null ? 10 - (int)Math.Ceiling((double)Rating) : 0;

        [Required]
        public string Text { get; set; }

        [Required(ErrorMessage = "Product is required")]
        public string ProductId { get; set; }

        [Required(ErrorMessage = "Comment is required")]
        public string CommentId { get; set; }

        [Required]
        public bool IsParrentComment { get; set; }
    }
}
