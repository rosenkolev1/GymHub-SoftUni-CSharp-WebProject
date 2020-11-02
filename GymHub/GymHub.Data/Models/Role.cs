using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace GymHub.Data.Models
{
    public class Role : IdentityRole<string>
    {
        public Role(string name)
            : base(name)
        {
            this.Id = Guid.NewGuid().ToString();
        }
        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<IdentityUserRole<string>> UserRoles { get; set; }
        public virtual ICollection<IdentityRoleClaim<string>> RoleClaims { get; set; }
    }
}
