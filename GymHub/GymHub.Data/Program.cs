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
            DeleteDatabase(dbContext, true);
            CreateDatabase(dbContext, true);
            MigrateDatabase(dbContext, true);
        }

        static void DeleteDatabase(ApplicationDbContext dbContext, bool isTrue)
        {
            if (isTrue)
            {
                dbContext.Database.EnsureDeleted();
            }
        }

        static void CreateDatabase(ApplicationDbContext dbContext, bool isTrue)
        {
            if (isTrue)
            {
                dbContext.Database.EnsureDeleted();
                dbContext.Database.EnsureCreated();
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
