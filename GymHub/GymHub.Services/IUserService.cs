using GymHub.Data.Models;
using GymHub.DTOs;
using GymHub.Web.Models.InputModels;
using System.Threading.Tasks;

namespace GymHub.Services
{
    public interface IUserService
    {
        public Task<User> GetUserAsync(LoginUserInputModel inputModel);
        public Task<bool> UserExistsAsync(string username, string password);
        public bool UserIsTaken(string username, string password, string email, string phoneNumber = null, bool hardCheck = false);
        public Task<User> CreateUserAsync(RegisterUserInputModel inputModel, params Role[] roles);
        public Task<User> CreateUserAsync(UserDTO userDTO);
        public Task<string> GetIdByUsernameAndPasswordAsync(LoginUserInputModel inputModel);
        public bool UsernameExists(string username);
        public bool EmailExists(string email);
        public bool PhoneNumberExists(string phoneNumber);
        public string GetUserId(string username, bool hardCheck = false);
        public string GetEmail(string userId);
        public User GetUser(string userId);
        public User GetUserByUsername(string username);
    }
}
