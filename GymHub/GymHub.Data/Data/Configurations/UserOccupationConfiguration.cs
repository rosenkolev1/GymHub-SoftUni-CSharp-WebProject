using GymHub.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymHub.Data.Data.Configurations
{
    public class UserOccupationConfiguration : IEntityTypeConfiguration<UserOccupation>
    {
        public void Configure(EntityTypeBuilder<UserOccupation> builder)
        {
            builder
                .HasKey(x => new { x.UserId, x.OccupationId });

            builder
                .Property(x => x.IsDeleted)
                .HasDefaultValue(false);

            builder
                .Property(x => x.DeletedOn)
                .HasDefaultValue(null);
        }
    }
}
