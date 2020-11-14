using GymHub.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GymHub.Services
{
    public interface IProductCommentService
    {
        public Task AddAsync(ProductComment productComment);
        public Task<bool> CommentExists(ProductComment productComment);
        public Task<List<ProductComment>> GetAllChildCommentsAsync(ProductComment productComment);
    }
}
