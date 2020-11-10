using GymHub.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace GymHub.Data.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, string>
    {
        public ApplicationDbContext()
        {

        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public virtual DbSet<Gender> Genders { get; set; }
        public virtual DbSet<Occupation> Occupations { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductCart> Carts { get; set; }
        public virtual DbSet<ProductComment> ProductsComments { get; set; }
        public virtual DbSet<ProductImage> ProductsImages { get; set; }
        public virtual DbSet<ProductRating> ProductsRatings { get; set; }
        public virtual DbSet<ProductSale> ProductsSales { get; set; }
        public virtual DbSet<Sale> Sales { get; set; }
        public virtual DbSet<UserOccupation> UsersOccupations { get; set; }
        public virtual DbSet<UserImage> UsersImages { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(optionsBuilder.IsConfigured == false)
            {
                optionsBuilder.UseSqlServer("Server=.;Database=GymHub;Integrated Security=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //Default Rating value = 0
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
