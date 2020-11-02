using GymHub.Data.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace GymHub.Web.Services
{
    public interface IRoleService
    {
        public Task AddAsync(RoleManager<Role> roleManager, string name);
        public string GetNormalUserRoleId();
        public Role GetAdminUser();
        public Role GetRoleById(string id);
        public bool RoleExists(string name);
    }
}
