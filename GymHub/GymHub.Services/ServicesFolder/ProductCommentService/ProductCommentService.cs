using GymHub.Data.Data;
using GymHub.Data.Models;
using GymHub.Data.Models.Enums;
using GymHub.Services.Common;
using GymHub.Services.ServicesFolder.ProductService;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GymHub.Services.ServicesFolder.ProductCommentService
{
    public class ProductCommentService : DeleteableEntityService, IProductCommentService
    {
        private readonly IProductService productService;

        public ProductCommentService(ApplicationDbContext context)
            : base(context)

        {

        }

        public ProductCommentService(ApplicationDbContext context, IProductService productService)
            : base(context)
        {
            this.productService = productService;
        }

        public async Task AddAsync(ProductComment productComment)
        {
            await context.AddAsync(productComment);
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commentId">The id of the comment to chech</param>
        /// <param name="hardCheck">If true then it also checks deleted entities</param>
        /// <returns></returns>
        public bool CommentExists(ProductComment productComment, bool hardCheck = false)
        {
            if (context.ProductsComments.IgnoreAllQueryFilters(hardCheck).Any(x => x.Id == productComment.Id) == true)
                return true;

            if (context.ProductsComments.IgnoreAllQueryFilters(hardCheck).Any(
                x => x.ParentCommentId == productComment.ParentCommentId && productComment.ParentCommentId == null && x.ProductId == productComment.ProductId && x.UserId == productComment.UserId) == true)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commentId">The id of the comment to chech</param>
        /// <param name="hardCheck">If true then it also checks deleted entities</param>
        /// <returns></returns>
        public bool CommentExists(string commentId, bool hardCheck = false)
        {
            return context.ProductsComments
                .IgnoreAllQueryFilters(hardCheck)
                .Any(x => x.Id == commentId);
        }

        public async Task<List<ProductComment>> GetAllChildCommentsAsync(ProductComment productComment)
        {
            var children = context.ProductsComments
                .Where(x => x.ParentCommentId == productComment.Id)
                .ToList();

            var currentChildrenCount = children.Count;

            for (int i = 0; i < currentChildrenCount; i++)
            {
                var child = children[i];
                var childrenToChildren = await GetAllChildCommentsAsync(child);
                children.AddRange(childrenToChildren);
            }
            return children;
        }

        public ProductComment GetProductComment(string commentId, bool hardCheck = false)
        {
            return context.ProductsComments.IgnoreAllQueryFilters(hardCheck).FirstOrDefault(x => x.Id == commentId);
        }

        public async Task EditCommentTextAsync(ProductComment comment, string text)
        {
            if (comment != null) comment.Text = text;
            comment.ModifiedOn = DateTime.UtcNow;
            await context.SaveChangesAsync();
        }

        public bool CommentMatchesUserAndProduct(string commentId, string userId, string productId)
        {
            var comment = context.ProductsComments.FirstOrDefault(x => x.Id == commentId);

            if (comment == null) return false;

            return comment.UserId == userId && comment.ProductId == productId;
        }

        public async Task RemoveAsync(string commentId)
        {
            //Remove parent comment
            var removedComment = context.ProductsComments
                .Include(x => x.ProductRating)
                .Include(x => x.CommentLikes)
                .FirstOrDefault(x => x.Id == commentId);

            await DeleteEntityAsync(removedComment);

            //Remove parent rating
            if (removedComment.ProductRating != null)
            {
                var ratingFromComment = removedComment.ProductRating;
                await DeleteEntityAsync(ratingFromComment);
            }

            //Remove comments' likes
            foreach (var commentLike in removedComment.CommentLikes)
            {
                await DeleteEntityAsync(commentLike);
            }

            //Remove child comments if it has any
            var repliesToRemovedComment = await GetAllChildCommentsAsync(removedComment);
            foreach (var reply in repliesToRemovedComment)
            {
                await DeleteEntityAsync(reply);
            }

            await context.SaveChangesAsync();
        }

        public bool CommentBelongsToUser(string commentId, string userId)
        {
            return context.ProductsComments.FirstOrDefault(x => x.Id == commentId)?.UserId == userId && userId != null;
        }

        public async Task LoadCommentLikesAsync(ProductComment comment)
        {
            await context.Entry(comment)
                .Collection(x => x.CommentLikes)
                .LoadAsync();
        }

        public bool UserHasLikedComment(string commentId, string userId)
        {
            return context.ProductCommentLikes.Any(x => x.ProductCommentId == commentId && x.UserId == userId);
        }

        public int GetCommentLikesCount(string commentId)
        {
            return context.ProductCommentLikes.Count(x => x.ProductCommentId == commentId);
        }

        public async Task LikeCommentAsync(string commentId, string userId)
        {
            //
            var existingProductComment = context.ProductCommentLikes
                .IgnoreAllQueryFilters(true)
                .FirstOrDefault(x => x.ProductCommentId == commentId && x.UserId == userId);

            if (existingProductComment == null)
            {
                existingProductComment = new ProductCommentLike { ProductCommentId = commentId, UserId = userId };
                await context.ProductCommentLikes.AddAsync(existingProductComment);
            }
            else
            {
                existingProductComment.IsDeleted = false;
                existingProductComment.DeletedOn = null;
            }

            await context.SaveChangesAsync();
        }

        public async Task UnlikeCommentAsync(string commentId, string userId)
        {

            await DeleteEntityAsync(context.ProductCommentLikes.FirstOrDefault(x => x.ProductCommentId == commentId && x.UserId == userId));
        }

        public IOrderedEnumerable<KeyValuePair<ProductComment, List<ProductComment>>> OrderParentsChildrenComments
            (IOrderedEnumerable<KeyValuePair<ProductComment, List<ProductComment>>> parentsChildrenComments, ProductCommentsOrderingOptions commentsOrderingOptions)
        {
            if (commentsOrderingOptions == ProductCommentsOrderingOptions.Likes)
            {
                return parentsChildrenComments
                     .ThenByDescending(kv => GetCommentLikesCount(kv.Key.Id));

            }
            else if (commentsOrderingOptions == ProductCommentsOrderingOptions.HighestRating)
            {
                return parentsChildrenComments
                     .ThenByDescending(kv => kv.Key.ProductRating.Rating);
            }
            else if (commentsOrderingOptions == ProductCommentsOrderingOptions.LowestRating)
            {
                return parentsChildrenComments
                     .ThenBy(kv => kv.Key.ProductRating.Rating);
            }
            else if (commentsOrderingOptions == ProductCommentsOrderingOptions.Newest)
            {
                return parentsChildrenComments
                     .ThenByDescending(kv => kv.Key.CommentedOn);
            }
            else if (commentsOrderingOptions == ProductCommentsOrderingOptions.Oldest)
            {
                return parentsChildrenComments
                     .ThenBy(kv => kv.Key.CommentedOn);
            }
            else
            {
                throw new ArgumentException("CommentsOrderingOptions parameter has invalid value");
            }
        }
    }
}
