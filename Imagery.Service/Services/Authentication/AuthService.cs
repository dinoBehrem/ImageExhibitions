using Imagery.Core.Models;
using Imagery.Repository.Repository;
using Imagery.Service.ViewModels.User;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Imagery.Service.Services.Authentication
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> UserManager;
        private readonly RoleManager<IdentityRole> RoleManager;
        private readonly IRepository<User> UserRepository;

        public AuthService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IRepository<User> userRepository)
        {
            UserManager = userManager;
            RoleManager = roleManager;
            UserRepository = userRepository;
        }

        public async Task<Response> AsignedToRole(RoleManagerVM roleManager)
        {
            var userExists = await UserManager.FindByNameAsync(roleManager.Username);

            if (userExists == null)
            {
                return new Response()
                {
                    Status = "Error",
                    Message = "Invalid username!",
                    IsSuccess = false
                };
            }

            var userRoles = await UserManager.GetRolesAsync(userExists);

            if (userRoles.Contains(roleManager.Role))
            {
                return new Response()
                {
                    Status = "Error",
                    Message = $"User \"{roleManager.Username}\" is already \"{roleManager.Role}\"!",
                    IsSuccess = false
                };
            }

            var result = await UserManager.AddToRoleAsync(userExists, roleManager.Role);

            if (!result.Succeeded)
            {
                return new Response()
                {
                    Status = "Error",
                    Message = $"Role \"{roleManager.Role}\" has not been asigned to user \"{roleManager.Username}\", try again!",
                    IsSuccess = false
                };
            }

            return new Response()
            {
                Status = "Success",
                Message = $"Role \"{roleManager.Role}\" has been successfully asigned to user {roleManager.Username}!",
                IsSuccess = true
            };
        }

        public async Task CreateRole(string roleName)
        {
            await RoleManager.CreateAsync(new IdentityRole(roleName));
        }

        public List<string> GetRoles()
        {
            return RoleManager.Roles.Select(role => role.Name).ToList();
        }

        public IEnumerable<UserVM> GetUsers()
        {
            return UserRepository.GetAll().Select(user => 
                new UserVM()
                {
                    Firstname = user.FirstName,
                    Lastname = user.LastName,
                    Username = user.UserName,
                    Email = user.Email,
                    Picture = user.ProfilePicture,
                    Roles = UserRoles(user).Result
                });
        }

        public async Task<Response> RemoveFromRole(RoleManagerVM roleManager)
        {
            var userExists = await UserManager.FindByNameAsync(roleManager.Username);

            if (userExists == null)
            {
                return new Response()
                {
                    Status = "Error",
                    Message = "Invalid username!",
                    IsSuccess = false
                };
            }

            var userRoles = await UserManager.GetRolesAsync(userExists);

            if (!userRoles.Contains(roleManager.Role))
            {
                return new Response()
                {
                    Status = "Error",
                    Message = $"User desn't hvae {roleManager.Role} role!",
                    IsSuccess = false
                };
            }

            var result = await UserManager.RemoveFromRoleAsync(userExists, roleManager.Role);

            if (!result.Succeeded)
            {
                return new Response()
                {
                    Status = "Error",
                    Message = "Role has not been removed user, try again!",
                    IsSuccess = false
                };
            }

            return new Response()
            {
                Status = "Success",
                Message = "Role has been successfully removed from user!",
                IsSuccess = true
            };

        }
        private async Task<List<string>> UserRoles(User user)
        {
            IList<string> roles = await UserManager.GetRolesAsync(user);

            return roles.ToList();
        }
    }
}
