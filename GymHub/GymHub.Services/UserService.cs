using GymHub.Data.Models;
using GymHub.Web.Data;
using GymHub.Web.Models.InputModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GymHub.Web.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext context;
        private readonly IRoleService roleService;
        private readonly IGenderService genderService;
        private readonly UserManager<User> userManager;
        public UserService(ApplicationDbContext context, IRoleService roleService, IGenderService genderService, UserManager<User> userManager)
        {
            this.context = context;
            this.roleService = roleService;
            this.genderService = genderService;
            this.userManager = userManager;
        }

        public async Task<User> RegisterNormalUserAsync(RegisterUserInputModel inputModel)
        {
            var newUser = new User()
            {
                FirstName = inputModel.FirstName,
                MiddleName = inputModel.MiddleName,
                LastName = inputModel.LastName,
                DateOfBirth = inputModel.DateOfBirth,
                UserName = inputModel.Username,
                Password = HashPassword(inputModel.Password),
                Email = inputModel.Email,
                DeletedOn = null,
                IsDeleted = false,
                RegisteredOn = DateTime.UtcNow,
                RoleId = await this.roleService.GetNormalUserRoleIdAsync(),
                GenderId = inputModel.GenderId,
            };
            var resultCreateUser = await this.userManager.CreateAsync(newUser, newUser.Password);
            if (!resultCreateUser.Succeeded)
            {
                throw new Exception(string.Join(Environment.NewLine, resultCreateUser.Errors.Select(e => e.Description)));
            }

            var resultAsingUserHisRole = await this.userManager.AddToRoleAsync(newUser, newUser.Role.Name);
            if (!resultAsingUserHisRole.Succeeded)
            {
                throw new Exception(string.Join(Environment.NewLine, resultAsingUserHisRole.Errors.Select(e => e.Description)));
            }

            return newUser;
        }

        public async Task<string> GetIdByUsernameAndPasswordAsync(LoginUserInputModel inputModel)
        {
            var user = this.context.Users.FirstOrDefault(x => x.UserName == inputModel.Username && x.Password == HashPassword(inputModel.Password));
            return user.Id;
        }

        private string HashPassword(string password)
        {
            // Create a SHA256   
            using (SHA512 sha512Hash = SHA512.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha512Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public async Task<bool> UsernameExistsAsync(string username)
        {
            return this.context.Users.Any(x => x.UserName == username);
        }

        public async Task<bool> PasswordExistsAsync(string password)
        {
            return this.context.Users.Any(x => x.Password == HashPassword(password));
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return this.context.Users.Any(x => x.Email == email);
        }

        public async Task<bool> PhoneNumberExistsAsync(string phoneNumber)
        {
            return this.context.Users.Any(x => x.PhoneNumber == phoneNumber);
        }
    }
}
