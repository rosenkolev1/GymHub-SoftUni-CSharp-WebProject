using GymHub.Data.Models;
using System.Threading.Tasks;

namespace GymHub.Web.Services
{
    public interface IRoleService
    {
        public Task AddAsync(string name);
        public Task<string> GetNormalUserRoleIdAsync();
        public Task<Role> GetNormalUserRoleAsync();
        public Task<Role> GetAdminUserAsync();
        public Task<Role> GetRoleByIdAsync(string id);
        public Task<bool> RoleExistsAsync(string name);
    }
}
