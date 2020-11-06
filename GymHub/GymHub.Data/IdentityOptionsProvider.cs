namespace GymHub.Data
{
    using GymHub.Common;
    using Microsoft.AspNetCore.Identity;

    public static class IdentityOptionsProvider
    {
        public static void GetIdentityOptions(IdentityOptions options)
        {
            //Passworr options
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = GlobalConstants.PasswordLengthMin;

            options.SignIn.RequireConfirmedAccount = false;
        }
    }
}