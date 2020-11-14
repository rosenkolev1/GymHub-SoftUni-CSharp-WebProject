using GymHub.Data.Models;
using GymHub.DTOs;
using GymHub.Web.Models.InputModels;
using System.Threading.Tasks;

namespace GymHub.Web.Services
{
    public interface IUserService
    {
        public Task<User> GetUserAsync(LoginUserInputModel inputModel);
        public Task<bool> UserExistsAsync(string username, string password);
        public Task<bool> UserIsTakenAsync(string username, string password, string email, string phoneNumber = null);
        public Task<User> CreateUserAsync(RegisterUserInputModel inputModel, params Role[] roles);
        public Task<User> CreateUserAsync(UserDTO userDTO);
        public Task<string> GetIdByUsernameAndPasswordAsync(LoginUserInputModel inputModel);
        public Task<bool> UsernameExistsAsync(string username);
        public Task<bool> EmailExistsAsync(string email);
        public Task<bool> PhoneNumberExistsAsync(string phoneNumber);
        public string GetUserId(string username);
    }
}
