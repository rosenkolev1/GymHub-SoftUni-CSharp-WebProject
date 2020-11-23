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
                .Property(x => x.Rating)
                .HasDefaultValue(0);

            builder
                .HasOne(x => x.ProductComment)
                .WithOne(x => x.ProductRating)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
