using GymHub.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
