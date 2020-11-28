using GymHub.Data;
using GymHub.Data.Data;
using GymHub.Data.Models;
using GymHub.Services.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GymHub.Services
{
    public class ProductCommentService : DeleteableEntityService, IProductCommentService
    {
        //private readonly ApplicationDbContext context;
        private readonly ProductService productService;

        public ProductCommentService(ApplicationDbContext context)
            :base(context)

        {
            
        }

        public ProductCommentService(ApplicationDbContext context, ProductService productService)
            :base(context)
        {
            this.productService = productService;
        }

        public async Task AddAsync(ProductComment productComment)
        {
            await this.context.AddAsync(productComment);
            await this.context.SaveChangesAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commentId">The id of the comment to chech</param>
        /// <param name="hardCheck">If true then it also checks deleted entities</param>
        /// <returns></returns>
        public bool CommentExists(ProductComment productComment, bool hardCheck = false)
        {
            if (this.context.ProductsComments.IgnoreAllQueryFilter(hardCheck).Any(x => x.Id == productComment.Id) == true)
                return true;

            if (this.context.ProductsComments.IgnoreAllQueryFilter(hardCheck).Any(
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
            return this.context.ProductsComments
                .IgnoreAllQueryFilter(hardCheck)
                .Any(x => x.Id == commentId);
        }

        public async Task<List<ProductComment>> GetAllChildCommentsAsync(ProductComment productComment)
        {
            var children = this.context.ProductsComments
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
            return this.context.ProductsComments.IgnoreAllQueryFilter(hardCheck).FirstOrDefault(x => x.Id == commentId);
        }

        public async Task EditCommentTextAsync(ProductComment comment, string text)
        {
            if (comment != null) comment.Text = text;
            comment.ModifiedOn = DateTime.UtcNow;
            await this.context.SaveChangesAsync();
        }

        public bool CommentMatchesUserAndProduct(string commentId, string userId, string productId)
        {
            var comment = this.context.ProductsComments.FirstOrDefault(x => x.Id == commentId);

            if (comment == null) return false;

            return comment.UserId == userId && comment.ProductId == productId;
        }

        public async Task RemoveAsync(string commentId)
        {
            //Remove parent comment
            var removedComment = this.context.ProductsComments
                .Include(x => x.ProductRating)
                .Include(x => x.CommentLikes)
                .FirstOrDefault(x => x.Id == commentId);

            await this.DeleteEntityAsync(removedComment);

            //Remove parent rating
            if (removedComment.ProductRating != null)
            {
                var ratingFromComment = removedComment.ProductRating;
                await this.DeleteEntityAsync(ratingFromComment);
            }

            //Remove comments' likes
            foreach (var commentLike in removedComment.CommentLikes)
            {
                await this.DeleteEntityAsync(commentLike);
            }

            //Remove child comments if it has any
            var repliesToRemovedComment = await GetAllChildCommentsAsync(removedComment);
            foreach (var reply in repliesToRemovedComment)
            {
                await this.DeleteEntityAsync(reply);
            }

            await this.context.SaveChangesAsync();
        }

        public bool CommentBelongsToUser(string commentId, string userId)
        {
            return this.context.ProductsComments.FirstOrDefault(x => x.Id == commentId)?.UserId == userId && userId != null;
        }

        public async Task LoadCommentLikesAsync(ProductComment comment)
        {
            await this.context.Entry(comment)
                .Collection(x => x.CommentLikes)
                .LoadAsync();
        }

        public bool UserHasLikedComment(string commentId, string userId)
        {
            return this.context.ProductCommentLikes.Any(x => x.ProductCommentId == commentId && x.UserId == userId);
        }

        public int GetCommentLikesCount(string commentId)
        {
            return this.context.ProductCommentLikes.Count(x => x.ProductCommentId == commentId);
        }

        public async Task LikeCommentAsync(string commentId, string userId)
        {
            //
            var existingProductComment = this.context.ProductCommentLikes
                .IgnoreAllQueryFilter(true)
                .FirstOrDefault(x => x.ProductCommentId == commentId && x.UserId == userId);

            if (existingProductComment == null)
            {
                existingProductComment = new ProductCommentLike { ProductCommentId = commentId, UserId = userId };
                await this.context.ProductCommentLikes.AddAsync(existingProductComment);
            }
            else
            {
                existingProductComment.IsDeleted = false;
                existingProductComment.DeletedOn = null;
            }

            await this.context.SaveChangesAsync();
        }

        public async Task UnlikeCommentAsync (string commentId, string userId)
        {

            await this.DeleteEntityAsync(this.context.ProductCommentLikes.FirstOrDefault(x => x.ProductCommentId == commentId && x.UserId == userId));
        }
    }
}
