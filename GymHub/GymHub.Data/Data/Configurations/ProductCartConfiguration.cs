using GymHub.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymHub.Data.Data.Configurations
{
    public class ProductCartConfiguration : IEntityTypeConfiguration<ProductCart>
    {
        public void Configure(EntityTypeBuilder<ProductCart> builder)
        {
            builder
                .HasKey(x => new { x.ProductId, x.UserId });
        }
    }
}
