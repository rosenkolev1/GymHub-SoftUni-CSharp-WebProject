using GymHub.Data.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace GymHub.Data
{
    class Program
    {
        static void Main(string[] args)
        {
            var dbContext = new ApplicationDbContext();
            CreateDatabase(dbContext, true);
            MigrateDatabase(dbContext, false);
        }

        static void CreateDatabase(ApplicationDbContext dbContext, bool isTrue)
        {
            if (isTrue)
            {
                dbContext.Database.EnsureDeleted();
                dbContext.Database.EnsureCreated();
                dbContext.Users.FirstOrDefault();
            }
        }

        static void MigrateDatabase(ApplicationDbContext dbContext, bool isTrue)
        {
            if (isTrue)
            {
                dbContext.Database.Migrate();
            }
        }
    }
}
