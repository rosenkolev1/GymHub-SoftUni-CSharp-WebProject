using Microsoft.AspNetCore.Authorization;

namespace GymHub.Web.AuthorizationPolicies
{
    public class AuthorizeAsAdminRequirement : IAuthorizationRequirement
    {
        public AuthorizeAsAdminRequirement()
        {
        }
    }
}
