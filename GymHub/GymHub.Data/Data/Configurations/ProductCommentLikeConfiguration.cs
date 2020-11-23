using GymHub.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymHub.Data.Data.Configurations
{
    public class ProductCommentLikeConfiguration : IEntityTypeConfiguration<ProductCommentLike>
    {
        public void Configure(EntityTypeBuilder<ProductCommentLike> builder)
        {
            builder
                .HasKey(x => new { x.ProductCommentId, x.UserId });

            builder
                .HasOne(x => x.User)
                .WithMany(u => u.ProductsCommentsLikes)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
