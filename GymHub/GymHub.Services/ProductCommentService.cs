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
            var something = this.context.ProductsComments.IgnoreAllQueryFilter(hardCheck);

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
                .FirstOrDefault(x => x.Id == commentId);
            await this.DeleteEntity(removedComment);

            //Remove parent rating
            if (removedComment.ProductRating != null)
            {
                var ratingFromComment = removedComment.ProductRating;
                await this.DeleteEntity(ratingFromComment);
            }

            var repliesToRemovedComment = await GetAllChildCommentsAsync(removedComment);
            foreach (var reply in repliesToRemovedComment)
            {
                await this.DeleteEntity(reply);
            }

            await this.context.SaveChangesAsync();
        }

        public bool CommentBelongsToUser(string commentId, string userId)
        {
            return this.context.ProductsComments.FirstOrDefault(x => x.Id == commentId)?.UserId == userId && userId != null;
        }

    }
}
