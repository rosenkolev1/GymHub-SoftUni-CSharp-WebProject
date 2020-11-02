using GymHub.Web.Data;
using System.Linq;

namespace GymHub.Data
{
    class Program
    {
        static void Main(string[] args)
        {
            var dbContext = new ApplicationDbContext();
            CreateDatabase(dbContext, true);
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
    }
}
