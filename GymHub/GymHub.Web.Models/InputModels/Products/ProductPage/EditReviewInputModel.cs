using GymHub.Web.Models.ViewModels;
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
            ProductRatingViewModel = productRatingViewModel;
            Text = text;
            ProductId = productId;
            CommentId = commentId;
            IsParrentComment = isParentComment;
            CommentCounter = commentCounter;
        }

        //Used for validation purposes
        public int CommentCounter { get; set; }

        public ProductRatingViewModel ProductRatingViewModel { get; set; }

        public bool HasReviewed => ProductRatingViewModel != null;

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
