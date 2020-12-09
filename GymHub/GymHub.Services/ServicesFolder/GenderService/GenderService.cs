using GymHub.Data.Data;
using GymHub.Data.Models;
using GymHub.Services.Common;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GymHub.Services.ServicesFolder.GenderService
{
    public class GenderService : DeleteableEntityService, IGenderService
    {
        public GenderService(ApplicationDbContext context)
            : base(context)
        {

        }

        public async Task AddAsync(string name)
        {
            await context.AddAsync(new Gender() { Name = name });
            await context.SaveChangesAsync();
        }

        public bool GenderExists(string name, bool hardCheck = false)
        {
            return context.Genders.IgnoreAllQueryFilters(hardCheck).Any(x => x.Name == name);
        }

        public List<Gender> GetAllGenders()
        {
            return context.Genders.ToList();
        }

        public string GetGenderIdByName(string name)
        {
            return context.Genders.FirstOrDefault(x => x.Name == name).Id;
        }

        public string GetGenderNameById(string id)
        {
            return context.Genders.FirstOrDefault(x => x.Id == id).Name;
        }
    }
}
