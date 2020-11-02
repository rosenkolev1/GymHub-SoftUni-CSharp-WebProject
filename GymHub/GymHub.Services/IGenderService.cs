using GymHub.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GymHub.Web.Services
{
    public interface IGenderService
    {
        public Task AddAsync(string name);
        public Task<string> GetGenderIdByNameAsync(string name);
        public Task<List<Gender>> GetAllGendersAsync();
        public Task<bool> GenderExistsAsync(string name);
    }
}
