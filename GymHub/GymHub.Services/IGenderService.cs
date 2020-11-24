using GymHub.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GymHub.Services
{
    public interface IGenderService
    {
        public Task AddAsync(string name);
        public string GetGenderIdByName(string name);
        public List<Gender> GetAllGenders();
        public bool GenderExists(string name, bool hardCheck = false);
    }
}
