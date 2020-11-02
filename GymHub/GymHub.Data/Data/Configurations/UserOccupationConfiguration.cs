using GymHub.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymHub.Data.Data.Configurations
{
    public class UserOccupationConfiguration : IEntityTypeConfiguration<UserOccupation>
    {
        public void Configure(EntityTypeBuilder<UserOccupation> builder)
        {
            builder
                .HasKey(x => new { x.UserId, x.OccupationId });
            builder
                .Property("IsDeleted")
                .HasDefaultValue(false);
            builder
                .Property("DeletedOn")
                .HasDefaultValue(null);
        }
    }
}
