using GymHub.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymHub.Web.Models.ViewModels
{
    public class ProductCommentPartialViewModel
    {
        public ProductCommentPartialViewModel(ProductComment comment, ProductRatingViewModel userProductRating)
        {
            this.Comment = comment;
            this.UserProductRating = userProductRating;
        }
        public ProductCommentPartialViewModel(ProductComment comment, ProductRatingViewModel userProductRating, int repliesCount)
        {
            this.Comment = comment;
            this.UserProductRating = userProductRating;
            this.RepliesCount = repliesCount;
        }
        public ProductComment Comment { get; set; }
        public ProductRatingViewModel UserProductRating { get; set; }
        public int RepliesCount { get; set; }
    }
}
