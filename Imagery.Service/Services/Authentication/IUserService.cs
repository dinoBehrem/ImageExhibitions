using Imagery.Core.Models;
using Imagery.Service.ViewModels.User;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagery.Service.Services.Authentication
{
    public interface IUserService
    {
        Task<Response> SignUp(RegisterVM register);
        Task<AuthResponse> SignIn(LoginVM login);
        Task<UserVM> GetUser(string username);
        Task<ProfileVM> GetUserProfile(string username);

        Task<bool> Subscribe(SubscribeVM subscription);
        Task<bool> Unsubscribe(SubscribeVM subscription);
    }
}
