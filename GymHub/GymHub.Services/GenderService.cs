using GymHub.Data;
using GymHub.Data.Data;
using GymHub.Data.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GymHub.Web.Services
{
    public class GenderService : IGenderService
    {
        private readonly ApplicationDbContext context;
        public GenderService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task AddAsync(string name)
        {
            await this.context.AddAsync(new Gender() { Name = name });
            await this.context.SaveChangesAsync();
        }

        public async Task<bool> GenderExistsAsync(string name)
        {
            return this.context.Genders.Any(x => x.Name == name);
        }

        public async Task<List<Gender>> GetAllGendersAsync()
        {
            return this.context.Genders.ToList();
        }

        public async Task<string> GetGenderIdByNameAsync(string name)
        {
            return this.context.Genders.FirstOrDefault(x => x.Name == name).Id;
        }
    }
}
