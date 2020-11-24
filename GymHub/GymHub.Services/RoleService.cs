﻿using GymHub.Data.Data;
using GymHub.Data.Models;
using GymHub.Services.Common;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GymHub.Services
{
    public class RoleService : DeleteableEntityService, IRoleService
    {
        private readonly RoleManager<Role> roleManager;
        public RoleService(ApplicationDbContext context, RoleManager<Role> roleManager)
            :base(context)
        {
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

        public Role GetRole(string name)
        {
            return this.context.Roles.FirstOrDefault(x => x.Name == name);
        }

        public bool RoleExists(string name, bool hardCheck = false)
        {
            return this.context.Roles.IgnoreAllQueryFilter(hardCheck).Any(x => x.Name == name);
        }
    }
}
