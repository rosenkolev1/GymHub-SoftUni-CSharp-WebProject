using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GymHub.Web.AuthorizationPolicies
{
    public class AuthorizeAsAdminRequirement : IAuthorizationRequirement
    {
        public AuthorizeAsAdminRequirement()
        {
        }
    }
}
