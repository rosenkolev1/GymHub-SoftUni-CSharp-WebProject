using GymHub.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymHub.Data.Data.Configurations
{
    public class ProductCommentConfiguration : IEntityTypeConfiguration<ProductComment>
    {
        public void Configure(EntityTypeBuilder<ProductComment> builder)
        {
            builder
                .Property("IsDeleted")
                .HasDefaultValue(false);
            builder
                .Property("DeletedOn")
                .HasDefaultValue(null);
        }
    }
}
