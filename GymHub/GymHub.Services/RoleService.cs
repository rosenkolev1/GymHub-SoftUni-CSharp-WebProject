using GymHub.Common;
using GymHub.Data;
using GymHub.Data.Data;
using GymHub.Data.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GymHub.Web.Services
{
    public class RoleService : IRoleService
    {
        private readonly ApplicationDbContext context;
        private readonly RoleManager<Role> roleManager;
        public RoleService(ApplicationDbContext context, RoleManager<Role> roleManager)
        {
            this.context = context;
            this.roleManager = roleManager;
        }

        public async Task AddAsync(string name)
        {
            var result = await this.roleManager.CreateAsync(new Role(name));
            if (!result.Succeeded)
            {
                throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
            }
        }

        public async Task<Role> GetAdminUserAsync()
        {
            return this.context.Roles.FirstOrDefault(r => r.Name == GlobalConstants.AdminRoleName);
        }

        public async Task<Role> GetNormalUserRoleAsync()
        {
            return this.context.Roles.FirstOrDefault(x => x.Name == GlobalConstants.NormalUserRoleName);
        }

        public async Task<string> GetNormalUserRoleIdAsync()
        {
            return this.context.Roles.FirstOrDefault(x => x.Name == GlobalConstants.NormalUserRoleName).Id;
        }

        public async Task<Role> GetRoleByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> RoleExistsAsync(string name)
        {
            return this.context.Roles.Any(x => x.Name == name);
        }
    }
}
