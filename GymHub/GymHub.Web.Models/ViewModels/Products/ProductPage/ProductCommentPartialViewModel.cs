using GymHub.Data.Models;
using GymHub.Web.Models.InputModels;

namespace GymHub.Web.Models.ViewModels
{
    public class ProductCommentPartialViewModel
    {
        public ProductCommentPartialViewModel(ProductComment comment, ReplyCommentInputModel replyCommentInputModels, ProductRatingViewModel userProductRating, int commentCounter)
        {
            Comment = comment;
            UserProductRating = userProductRating;
            CommentCounter = commentCounter;
            ReplyCommentInputModel = replyCommentInputModels;
        }

        public ProductCommentPartialViewModel(ProductComment comment, ReplyCommentInputModel replyCommentInputModels, ProductRatingViewModel userProductRating, string currentUserId, int commentCounter, int repliesCount = 0)
        {
            Comment = comment;
            UserProductRating = userProductRating;
            RepliesCount = repliesCount;
            CurrentUserId = currentUserId;
            CommentCounter = commentCounter;
            ReplyCommentInputModel = replyCommentInputModels;
        }


        public int CommentCounter { get; set; }
        public ProductComment Comment { get; set; }
        public ProductRatingViewModel UserProductRating { get; set; }
        public bool HasReviewed => UserProductRating != null;
        public int RepliesCount { get; set; }
        public bool BelongsToCurrentUser => Comment.UserId == CurrentUserId;
        public string CurrentUserId { get; set; }
        public ReplyCommentInputModel ReplyCommentInputModel { get; set; }
    }
}
