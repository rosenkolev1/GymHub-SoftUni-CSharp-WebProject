using GymHub.Common;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace GymHub.Web.AuthorizationPolicies
{
    public class AuthorizeAsAdminHandler : AuthorizationHandler<AuthorizeAsAdminRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AuthorizeAsAdminRequirement requirement)
        {
            var currentUser = context.User;

            if (currentUser.IsInRole(GlobalConstants.AdminRoleName))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
