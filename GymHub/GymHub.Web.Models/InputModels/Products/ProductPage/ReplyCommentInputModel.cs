using System.ComponentModel.DataAnnotations;

namespace GymHub.Web.Models.InputModels
{
    public class ReplyCommentInputModel
    {
        public ReplyCommentInputModel()
        {

        }
        public ReplyCommentInputModel(string productId, string parentCommentId, int commentCounter)
        {
            ProductId = productId;
            ParentCommentId = parentCommentId;
            CommentCounter = commentCounter;
        }

        //Used for validation purposes
        public int CommentCounter { get; set; }

        [Required]
        public string Text { get; set; }

        [Required(ErrorMessage = "Product is required")]
        public string ProductId { get; set; }

        [Required(ErrorMessage = "Comment is required")]
        public string ParentCommentId { get; set; }
    }
}
