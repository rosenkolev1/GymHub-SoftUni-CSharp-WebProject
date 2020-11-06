using GymHub.Data.Models;
using System.Threading.Tasks;

namespace GymHub.Web.Services
{
    public interface IRoleService
    {
        public Task AddAsync(string name);
        public Task<Role> GetRoleAsync(string name);
        public Task<bool> RoleExistsAsync(string name);
    }
}
