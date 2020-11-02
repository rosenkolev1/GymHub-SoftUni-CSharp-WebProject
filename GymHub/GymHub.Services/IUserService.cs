using GymHub.Data.Models;
using GymHub.Web.Models.InputModels;

namespace GymHub.Web.Services
{
    public interface IUserService
    {
        public User RegisterNormalUser(RegisterUserInputModel inputModel);
        public string GetIdByUsernameAndPassword(LoginUserInputModel inputModel);
        public bool UsernameExists(string username);
        public bool PasswordExists(string password);
        public bool EmailExists(string email);
        public bool PhoneNumberExists(string phoneNumber);
    }
}
