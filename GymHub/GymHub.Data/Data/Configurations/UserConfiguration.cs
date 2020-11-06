using GymHub.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Security.Cryptography.X509Certificates;

namespace GymHub.Data.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            //Default values
            builder
                .Property(x => x.DeletedOn)
                .HasDefaultValue(null);
            builder
                .Property(x => x.Description)
                .HasDefaultValue(null);
            builder
                .Property(x => x.ProfilePicture)
                .HasDefaultValue(null);
            builder
                .Property(x => x.RegisteredOn)
                .HasDefaultValue(DateTime.UtcNow);

            //Unique column
            builder
                .HasIndex(x => x.UserName)
                .IsUnique();

            builder
                .HasIndex(x => x.PasswordHash)
                .IsUnique();

            builder
                .HasIndex(x => x.Email)
                .IsUnique();

            builder
                .HasIndex(x => x.PhoneNumber)
                .IsUnique();


            //Relationships
            builder
                .HasMany(e => e.Claims)
                .WithOne()
                .HasForeignKey(e => e.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasMany(e => e.Logins)
                .WithOne()
                .HasForeignKey(e => e.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasMany(e => e.Roles)
                .WithOne()
                .HasForeignKey(e => e.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.ToTable("Users");
        }
    }
}
