using GymHub.Data.Models;
using System.Threading.Tasks;

namespace GymHub.Services.ServicesFolder.RoleService
{
    public interface IRoleService
    {
        public Task AddAsync(string name);
        public Role GetRole(string name);
        public bool RoleExists(string name, bool hardCheck = false);
    }
}
