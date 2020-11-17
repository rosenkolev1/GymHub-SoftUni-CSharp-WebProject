using GymHub.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymHub.Web.Models.ViewModels
{
    public class ProductCommentPartialViewModel
    {
        public ProductCommentPartialViewModel(ProductComment comment, ProductRatingViewModel userProductRating, int commentCounter)
        {
            this.Comment = comment;
            this.UserProductRating = userProductRating;
            this.CommentCounter = commentCounter;
        }
        public ProductCommentPartialViewModel(ProductComment comment, ProductRatingViewModel userProductRating, string currentUserId, int commentCounter, int repliesCount = 0)
        {
            this.Comment = comment;
            this.UserProductRating = userProductRating;
            this.RepliesCount = repliesCount;
            this.CurrentUserId = currentUserId;
            this.CommentCounter = commentCounter;
        }
        public int CommentCounter { get; set; }
        public ProductComment Comment { get; set; }
        public ProductRatingViewModel UserProductRating { get; set; }
        public int RepliesCount { get; set; }
        public bool BelongsToCurrentUser => this.Comment.UserId == this.CurrentUserId;
        public string CurrentUserId { get; set; }
    }
}
