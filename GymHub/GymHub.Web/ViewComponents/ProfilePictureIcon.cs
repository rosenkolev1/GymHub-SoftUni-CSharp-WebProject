using GymHub.Services;
using GymHub.Services.ServicesFolder.GenderService;
using Microsoft.AspNetCore.Mvc;

namespace GymHub.Web.ViewComponents
{
    public class ProfilePictureIcon : ViewComponent
    {
        private IUserService userService;
        private IGenderService genderService;
        public ProfilePictureIcon(IUserService userService, IGenderService genderService)
        {
            this.userService = userService;
            this.genderService = genderService;
        }

        public IViewComponentResult Invoke()
        {
            var currentUser = this.userService.GetUserByUsername(this.User.Identity.Name);
            var userGender = this.genderService.GetGenderNameById(currentUser.GenderId);
            return this.View<string>(userGender);
        }
    }
}
