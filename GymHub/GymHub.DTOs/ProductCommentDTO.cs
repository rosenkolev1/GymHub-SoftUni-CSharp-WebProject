using System;

namespace GymHub.DTOs
{
    public class ProductCommentDTO
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string ParentCommentId { get; set; }
        public string ProductModel { get; set; }
        public string Text { get; set; }
        public DateTime CommentedOn { get; set; }
    }
}
