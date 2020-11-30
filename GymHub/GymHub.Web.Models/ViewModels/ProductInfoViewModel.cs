using GymHub.Data.Models;
using GymHub.Data.Models.Enums;
using GymHub.Web.Models.InputModels;
using System.Collections.Generic;

namespace GymHub.Web.Models.ViewModels
{
    public class ProductInfoViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string MainImage { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        //Warranty in days
        public int Warranty { get; set; }
        public int QuantityInStock { get; set; }
        public string Model { get; set; }
        public bool ReviewedByCurrentUser { get; set; }
        public string CurrentUserId { get; set; }
        public List<ProductComment> ProductComments { get; set; }
        public List<ProductRating> ProductRatings { get; set; }
        public ProductRatingViewModel ProductRating { get; set; }
        public Dictionary<User, ProductRatingViewModel> UsersProductRatings { get; set; } = new Dictionary<User, ProductRatingViewModel>();
        public Dictionary<ProductComment, List<ProductComment>> ParentsChildrenComments { get; set; } = new Dictionary<ProductComment, List<ProductComment>>();
        public ReplyCommentInputModel ReplyCommentInputModel { get; set; }
        public ProductCommentsOrderingOptions CommentsOrderingOptions { get; set; }
        public string ToReplyComment { get; set; }
        public int NumberOfCommentsPages { get; set; }
        public int CurrentCommentsPage { get; set; }
    }
}
