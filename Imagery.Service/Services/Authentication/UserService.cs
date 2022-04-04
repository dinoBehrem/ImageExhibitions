using Imagery.Core.Models;
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
                throw new Exception("Incorrect username or password");
            }

            AuthResponse token = await TokenService.BuildToken(user);

            return token;
        }

        public async Task<Response> SignUp(RegisterVM register)
        {
            var userExists = await UserManager.FindByNameAsync(register.Username);

            if (userExists != null)
            {
                return new Response()
                {
                    Status = "Error",
                    Message = $"User with username \"{register.Username}\" already exists!",
                    IsSuccess = false
                };
            }

            User user = new User()
            {
                FirstName = register.Firstname,
                LastName = register.Lastname,
                UserName = register.Username,
                Email = register.Email,
                ProfilePicture = register.Image
            };

            var result = await UserManager.CreateAsync(user, register.Password);


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
            await UserManager.AddToRoleAsync(user, Roles.User);

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
            var roles = await UserManager.GetRolesAsync(userExist);

            ProfileVM profile = new ProfileVM()
            {
                FirstName = userExist.FirstName,
                LastName = userExist.LastName,
                UserName = userExist.UserName,
                ProfilePicture = userExist.ProfilePicture,
                Email = userExist.Email,
                Phone = userExist.PhoneNumber,
                Biography = userExist.Biography,
                //Exhibitions = ExhibitionService.UserExhibitions(username).Select(exhibition => new ExhibitionProfileVM() { Id = exhibition.Id, Title = exhibition.Title, Date = exhibition.Date, Description = exhibition.Description, Expired = exhibition.Expired, Started = DateTime.Now > exhibition.Date, Subscribers = exhibition.Subscribers }).ToList(),
                Exhibitions = ExhibitionService.UserExhibitions(username),
                Followers = GetSubs(SubscriptionRepository.Find(sub => sub.CreatorId == userExist.Id).ToList(), "followers"),
                Following = GetSubs(SubscriptionRepository.Find(sub => sub.SubscriberId == userExist.Id).ToList(), "following"),
                Roles = roles.ToList()
            };

            return profile;
        }
        
        public async Task<bool> Subscribe(SubscribeVM subscription)
        {
            var userExist = await UserManager.FindByNameAsync(subscription.Creator);

            if (userExist == null)
            {
                return false;
            }

            var subscriber = await UserManager.FindByNameAsync(subscription.Subscriber);

            if (subscriber == null)
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

        public async Task<UserEditVM> EditProfile(string username, UserEditVM user)
        {
            var userExist = await UserManager.FindByNameAsync(username);

            if (userExist == null)
            {
                throw new Exception("User doesn't exist!");
            }

            userExist.FirstName = user.Firstname;
            userExist.LastName = user.Lastname;
            userExist.Email = user.Email;
            userExist.PhoneNumber = user.Phone;
            userExist.Biography = user.Biography;

            var response = await UserManager.UpdateAsync(userExist);

            if (!response.Succeeded)
            {
                throw new Exception("Error, edit failed!");
            }

            return user;
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


        // Methods for generating test data
        public async Task AddTestUsers(List<RegisterVM> users, List<string> pictures)
        {
            for (int i = 0; i < users.Count; i++)
            {
                users[i].Image = pictures[i];
               await SignUp(users[i]);
            }
        }
        
        public async Task EditUserTest(RegisterVM user, string bio, string phone)
        {
            await EditProfile(user.Username, new UserEditVM()
            {
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                Email = user.Email,
                Phone = phone,
                Biography = bio
            });
        }

        public async Task TestSubscriptions(List<RegisterVM> users)
        {
            Random random = new Random();
            List<SubscribeVM> subscribes = new List<SubscribeVM>();

            SubscribeVM subscribeVM = new SubscribeVM();

            foreach (var user in users)
            {
                int subCount = random.Next(2, 5);


                for (int i = 0; i < subCount; i++)
                {
                    int index = random.Next(0, users.Count);

                    if (user.Username != users[index].Username)
                    {
                        subscribeVM.Creator = user.Username;
                        subscribeVM.Subscriber = users[index].Username;

                        if (!subscribes.Contains(subscribeVM))
                        {
                            subscribes.Add(subscribeVM);
                        }
                    }
                }

                foreach (var sub in subscribes)
                {
                    var success = await Subscribe(sub);
                }

                subscribes.Clear();
            }
        }

        public async Task SuperAdminMethod(string username)
        {
            var userExists = await UserManager.FindByNameAsync(username);

            var userRoles = await UserManager.GetRolesAsync(userExists);

            if (!userRoles.Contains(Roles.SuperAdmin))
            {
                var result = await UserManager.AddToRoleAsync(userExists, Roles.SuperAdmin);
            }


        }
    }
}