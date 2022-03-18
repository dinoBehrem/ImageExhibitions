﻿using Imagery.Core.Models;
using Imagery.Repository.Repository;
using Imagery.Service.Helpers;
using Imagery.Service.Services.Exhbition;
using Imagery.Service.ViewModels.Exhbition;
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
        private readonly IExhibitionService ExhibitionService;
        private readonly IRepository<UserSubscription> SubscriptionRepository;

        public UserService(UserManager<User> userManager, SignInManager<User> signInManager, ITokenService tokenService, IExhibitionService exhibitionService, IRepository<UserSubscription> subscriptionRepository)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            TokenService = tokenService;
            ExhibitionService = exhibitionService;
            SubscriptionRepository = subscriptionRepository;
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

        public async Task<UserVM> GetUser(string username)
        {
            var userExists = await UserManager.FindByNameAsync(username);

            if (userExists == null)
            {
                return null;
            }

            var roles = await UserManager.GetRolesAsync(userExists);

            UserVM user = new UserVM()
            {
                Username = userExists.UserName,
                Firstname = userExists.FirstName,
                Lastname = userExists.LastName,
                Email = userExists.Email,
                Picture = userExists.ProfilePicture,
                Roles = roles.ToList()
            };

            return user;
        }

        public async Task<ProfileVM> GetUserProfile(string username)
        {
            var userExist = await UserManager.FindByNameAsync(username);

            if (userExist == null)
            {
                return null;
            }

            ProfileVM profile = new ProfileVM()
            {
                FirstName = userExist.FirstName,
                LastName = userExist.LastName,
                UserName = userExist.UserName,
                ProfilePicture = userExist.ProfilePicture,
                Email = userExist.Email,
                Phone = userExist.PhoneNumber,
                Biography = userExist.Biography,
                Exhibitions = ExhibitionService.UserExhibitions(username).Select(exhibition => new ExhibitionProfileVM() { Id = exhibition.Id, Title = exhibition.Title, Date = exhibition.Date, Description = exhibition.Description, Expired = exhibition.Expired, Started = DateTime.Now > exhibition.Date }).ToList(),
                Followers = GetSubs(SubscriptionRepository.GetAll().Where(sub => sub.CreatorId == userExist.Id).ToList(), "followers"),
                Following = GetSubs(SubscriptionRepository.GetAll().Where(sub => sub.SubscriberId == userExist.Id).ToList(), "following")
            };

            return profile;
        }

        private List<UserVM> GetSubs(List<UserSubscription> userSubscriptions, string subsType)
        {
            List<Task<User>> users = new List<Task<User>>();
            if(subsType == "followers")
            {
                users = userSubscriptions.Select(async sub => await UserManager.FindByIdAsync(sub.SubscriberId)).ToList();
            }

            if(subsType == "following")
            {
                users = userSubscriptions.Select(async sub => await UserManager.FindByIdAsync(sub.CreatorId)).ToList();
            }

            var subs = users.Select(user => Mapper.MapUserVM(user.Result)).ToList();

            return subs;
        }

        public async Task<bool> Subscribe(SubscribeVM subscription)
        {
            var userExist = await UserManager.FindByNameAsync(subscription.Creator);
            var subscriber = await UserManager.FindByNameAsync(subscription.Subscriber);

            if (userExist == null)
            {
                return false;
            }

            var response = SubscriptionRepository.Add(new UserSubscription() { CreatorId = userExist.Id, SubscriberId = subscriber.Id});

            if (!response.IsSuccess)
            {
                return false;
            }

            return true;
        }
        
        public async Task<bool> Unsubscribe(SubscribeVM subscription)
        {
            var userExist = await UserManager.FindByNameAsync(subscription.Creator);
            var subscriber = await UserManager.FindByNameAsync(subscription.Subscriber);

            if (userExist == null)
            {
                return false;
            }

            var response = SubscriptionRepository.Remove(new UserSubscription() { CreatorId = userExist.Id, SubscriberId = subscriber.Id});

            if (!response.IsSuccess)
            {
                return false;
            }

            return true;
        }
    }
}