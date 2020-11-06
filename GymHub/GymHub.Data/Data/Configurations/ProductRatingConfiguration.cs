using GymHub.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymHub.Data.Data.Configurations
{
    public class ProductRatingConfiguration : IEntityTypeConfiguration<ProductRating>
    {
        public void Configure(EntityTypeBuilder<ProductRating> builder)
        {
            builder
                .HasKey(x => new { x.ProductId, x.UserId });

            builder
                .Property(x => x.Rating)
                .HasDefaultValue(0);
        }
    }
}
