using GymHub.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GymHub.Services
{
    public interface IProductCommentService
    {
        public Task AddAsync(ProductComment productComment);
        public Task<bool> CommentExists(ProductComment productComment);
        public Task<List<ProductComment>> GetAllChildCommentsAsync(ProductComment productComment);
        public ProductComment GetProductComment(string commentId);
        public bool CommentExists(string commentId);
        public bool CommentMatchesUserAndProduct(string commentId, string userId, string productId);
        public Task EditCommentTextAsync(ProductComment comment, string text);
        public Task RemoveAsync(string commentId);
    }
}
