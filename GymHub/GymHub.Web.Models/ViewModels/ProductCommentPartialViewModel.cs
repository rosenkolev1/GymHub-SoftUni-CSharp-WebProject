using GymHub.Data.Models;
using GymHub.Web.Models.InputModels;
using System.Collections.Generic;
using System.Linq;

namespace GymHub.Web.Models.ViewModels
{
    public class ProductCommentPartialViewModel
    {
        public ProductCommentPartialViewModel(ProductComment comment, ReplyCommentInputModel replyCommentInputModels, ProductRatingViewModel userProductRating, int commentCounter)
        {
            this.Comment = comment;
            this.UserProductRating = userProductRating;
            this.CommentCounter = commentCounter;
            this.ReplyCommentInputModel = replyCommentInputModels;
        }

        public ProductCommentPartialViewModel(ProductComment comment, ReplyCommentInputModel replyCommentInputModels, ProductRatingViewModel userProductRating, string currentUserId, int commentCounter, int repliesCount = 0)
        {
            this.Comment = comment;
            this.UserProductRating = userProductRating;
            this.RepliesCount = repliesCount;
            this.CurrentUserId = currentUserId;
            this.CommentCounter = commentCounter;
            this.ReplyCommentInputModel = replyCommentInputModels;
        }


        public int CommentCounter { get; set; }
        public ProductComment Comment { get; set; }
        public ProductRatingViewModel UserProductRating { get; set; }
        public bool HasReviewed => UserProductRating != null;
        public int RepliesCount { get; set; }
        public bool BelongsToCurrentUser => this.Comment.UserId == this.CurrentUserId;
        public string CurrentUserId { get; set; }
        public ReplyCommentInputModel ReplyCommentInputModel { get; set; }
    }
}
