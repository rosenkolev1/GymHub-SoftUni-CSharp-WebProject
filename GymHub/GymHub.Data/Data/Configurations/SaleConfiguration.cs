using GymHub.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Security.Cryptography.X509Certificates;

namespace GymHub.Data.Data.Configurations
{
    public class SaleConfiguration : IEntityTypeConfiguration<Sale>
    {
        public void Configure(EntityTypeBuilder<Sale> builder)
        {           
            builder
                .Property(x => x.PurchasedOn)
                .HasDefaultValue(DateTime.UtcNow);

        }
    }
}
