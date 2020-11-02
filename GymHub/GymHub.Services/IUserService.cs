using GymHub.Data.Models;
using GymHub.Web.Models.InputModels;
using System.Threading.Tasks;

namespace GymHub.Web.Services
{
    public interface IUserService
    {
        public Task<User> RegisterNormalUserAsync(RegisterUserInputModel inputModel);
        public Task<string> GetIdByUsernameAndPasswordAsync(LoginUserInputModel inputModel);
        public Task<bool> UsernameExistsAsync(string username);
        public Task<bool> PasswordExistsAsync(string password);
        public Task<bool> EmailExistsAsync(string email);
        public Task<bool> PhoneNumberExistsAsync(string phoneNumber);
    }
}
