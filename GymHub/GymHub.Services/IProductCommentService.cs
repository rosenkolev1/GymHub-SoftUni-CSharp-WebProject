using GymHub.Data.Models;
using GymHub.Data.Models.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GymHub.Services
{
    public interface IProductCommentService
    {
        public IOrderedEnumerable<KeyValuePair<ProductComment, List<ProductComment>>> OrderParentsChildrenComments
            (
            IOrderedEnumerable<KeyValuePair<ProductComment, List<ProductComment>>> parentsChildrenComments,
            ProductCommentsOrderingOptions commentsOrderingOptions
            );
        public Task AddAsync(ProductComment productComment);
        public bool CommentExists(ProductComment productComment, bool hardCheck = false);
        public Task<List<ProductComment>> GetAllChildCommentsAsync(ProductComment productComment);
        public ProductComment GetProductComment(string commentId, bool hardCheck = false);
        public bool CommentExists(string commentId, bool hardCheck = false);
        public bool CommentMatchesUserAndProduct(string commentId, string userId, string productId);
        public Task EditCommentTextAsync(ProductComment comment, string text);
        public Task RemoveAsync(string commentId);
        public bool CommentBelongsToUser(string commentId, string userId);
        public Task LoadCommentLikesAsync(ProductComment comment);
        public bool UserHasLikedComment(string commentId, string userId);
        public int GetCommentLikesCount(string commentId);
        public Task LikeCommentAsync(string commentId, string userId);
        public Task UnlikeCommentAsync(string commentId, string userId);
    }
}
