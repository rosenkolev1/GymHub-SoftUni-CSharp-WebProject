using GymHub.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace GymHub.Web.Models.InputModels
{
    public class EditReviewInputModel
    {
        public EditReviewInputModel()
        {

        }
        public EditReviewInputModel(ProductRatingViewModel productRatingViewModel, string text, string productId, string commentId, bool isParentComment, int commentCounter)
        {
            this.ProductRatingViewModel = productRatingViewModel;
            this.Text = text;
            this.ProductId = productId;
            this.CommentId = commentId;
            this.IsParrentComment = isParentComment;
            this.CommentCounter = commentCounter;
        }

        //Used for validation purposes
        public int CommentCounter { get; set; }

        public ProductRatingViewModel ProductRatingViewModel { get; set; }

        public bool HasReviewed => this.ProductRatingViewModel != null;

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
