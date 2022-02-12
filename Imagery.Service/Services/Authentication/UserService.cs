using Imagery.Core.Models;
using Imagery.Repository.Repository;
using Imagery.Service.ViewModels.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Imagery.Service.Services.Authentication
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> UserManager;
        private readonly SignInManager<User> SignInManager;
        private readonly ITokenService TokenService;
        private readonly RoleManager<IdentityRole> RoleManager;

        public UserService(UserManager<User> userManager, SignInManager<User> signInManager, ITokenService tokenService, RoleManager<IdentityRole> roleManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            TokenService = tokenService;
            RoleManager = roleManager;
        }

        public async Task<AuthResponse> SignIn(LoginVM login)
        {
            var user = await UserManager.FindByNameAsync(login.Username);

            var result = await SignInManager.PasswordSignInAsync(login.Username, login.Password, false, false);

            if (!result.Succeeded)
            {
                return null;
            }

            AuthResponse token = await TokenService.BuildToken(user);

            return token;
        }

        public async Task<Response> SignUp(RegisterVM register)
        {
            var userExists = await UserManager.FindByNameAsync(register.Username);

            if (userExists != null)
            {
                //Status = "Error", Message = "User already exists!" }

                return new Response()
                {
                    Status = "Error",
                    Message = "User already exists!",
                    IsSuccess = false
                };
            }

            User user = new User()
            {
                FirstName = register.Firstname,
                LastName = register.Lastname,
                UserName = register.Username,
                Email = register.Email
            };

            var result = await UserManager.CreateAsync(user, register.Password);

            await UserManager.AddClaimAsync(user, new Claim("role", Roles.User));

            if (!result.Succeeded)
            {
               return new Response()
                {
                    Status = "Error",
                    Message = "Invalid credenitals!",
                    IsSuccess = false,
                    Errors = result.Errors.Select(err => err.Description).ToList()
                };
            }

            return new Response()
            {
                Status = "Success",
                Message = "User successfully created!",
                IsSuccess = true
            };
        }
    }
}
