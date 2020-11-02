using GymHub.Common;
using GymHub.Data.Models;
using GymHub.Web.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GymHub.Web.Services
{
    public class RoleService : IRoleService
    {
        private readonly ApplicationDbContext context;
        public RoleService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task AddAsync(RoleManager<Role> roleManager, string name)
        {
            var result = await roleManager.CreateAsync(new Role(name));
            if (!result.Succeeded)
            {
                throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
            }
        }

        public Role GetAdminUser()
        {
            return this.context.Roles.FirstOrDefault(r => r.Name == GlobalConstants.AdminRoleName);
        }

        public string GetNormalUserRoleId()
        {
            return this.context.Roles.FirstOrDefault(x => x.Name == GlobalConstants.NormalUserRoleName).Id;
        }

        public Role GetRoleById(string id)
        {
            throw new NotImplementedException();
        }

        public bool RoleExists(string name)
        {
            return this.context.Roles.Any(x => x.Name == name);
        }
    }
}
