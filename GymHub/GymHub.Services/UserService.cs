using GymHub.Data.Models;
using GymHub.Web.Data;
using GymHub.Web.Models.InputModels;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace GymHub.Web.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext context;
        private readonly IRoleService roleService;
        private readonly IGenderService genderService;
        public UserService(ApplicationDbContext context, IRoleService roleService, IGenderService genderService)
        {
            this.context = context;
            this.roleService = roleService;
            this.genderService = genderService;
        }

        public User RegisterNormalUser(RegisterUserInputModel inputModel)
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
                RoleId = this.roleService.GetNormalUserRoleId(),
                GenderId = inputModel.GenderId,
            };
            this.context.Add(newUser);
            this.context.SaveChanges();
            return newUser;
        }

        public string GetIdByUsernameAndPassword(LoginUserInputModel inputModel)
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

        public bool UsernameExists(string username)
        {
            return this.context.Users.Any(x => x.UserName == username);
        }

        public bool PasswordExists(string password)
        {
            return this.context.Users.Any(x => x.Password == HashPassword(password));
        }

        public bool EmailExists(string email)
        {
            return this.context.Users.Any(x => x.Email == email);
        }

        public bool PhoneNumberExists(string phoneNumber)
        {
            return this.context.Users.Any(x => x.PhoneNumber == phoneNumber);
        }
    }
}
