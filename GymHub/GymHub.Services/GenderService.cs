using GymHub.Data.Data;
using GymHub.Data.Models;
using GymHub.Services.Common;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GymHub.Services
{
    public class GenderService : DeleteableEntityService, IGenderService
    {
        public GenderService(ApplicationDbContext context)
            :base(context)
        {

        }

        public async Task AddAsync(string name)
        {
            await this.context.AddAsync(new Gender() { Name = name });
            await this.context.SaveChangesAsync();
        }

        public bool GenderExists(string name, bool hardCheck = false)
        {
            return this.context.Genders.IgnoreAllQueryFilter(hardCheck).Any(x => x.Name == name);
        }

        public List<Gender> GetAllGenders()
        {
            return this.context.Genders.ToList();
        }

        public string GetGenderIdByName(string name)
        {
            return this.context.Genders.FirstOrDefault(x => x.Name == name).Id;
        }
    }
}
