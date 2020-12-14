using GymHub.Data.Data;
using GymHub.Data.Models;
using GymHub.Services.Common;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace GymHub.Services.ServicesFolder.RoleService
{
    public class RoleService : DeleteableEntityService, IRoleService
    {
        private readonly RoleManager<Role> roleManager;

        public RoleService(ApplicationDbContext context, RoleManager<Role> roleManager)
            : base(context)
        {
            this.roleManager = roleManager;
        }

        public async Task AddAsync(string name)
        {
            await roleManager.CreateAsync(new Role(name));
        }

        public Role GetRole(string name)
        {
            return context.Roles.FirstOrDefault(x => x.Name == name);
        }

        public bool RoleExists(string name, bool hardCheck = false)
        {
            return context.Roles.IgnoreAllQueryFilters(hardCheck).Any(x => x.Name == name);
        }
    }
}
