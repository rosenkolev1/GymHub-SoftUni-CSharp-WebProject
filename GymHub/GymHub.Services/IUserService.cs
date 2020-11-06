using GymHub.Data.Models;
using GymHub.Web.Models.InputModels;
using System.Threading.Tasks;

namespace GymHub.Web.Services
{
    public interface IUserService
    {
        public Task<User> GetUserAsync(LoginUserInputModel inputModel);
        public Task<bool> UserExistsAsync(string username, string password);
        public Task<User> CreateUserAsync(RegisterUserInputModel inputModel);
        public Task<User> CreateNormalUserAsync(RegisterUserInputModel inputModel);
        public Task<User> CreateAdminUserAsync(RegisterUserInputModel inputModel);
        public Task<string> GetIdByUsernameAndPasswordAsync(LoginUserInputModel inputModel);
        public Task<bool> UsernameExistsAsync(string username);
        public Task<bool> EmailExistsAsync(string email);
        public Task<bool> PhoneNumberExistsAsync(string phoneNumber);
    }
}
