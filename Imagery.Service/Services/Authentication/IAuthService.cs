using Imagery.Service.ViewModels.User;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagery.Service.Services.Authentication
{
    public interface IAuthService
    {
        IEnumerable<UserVM> GetUsers();
        Task<Response> AsignedToRole(RoleManagerVM roleManager);
        Task<Response> RemoveFromRole(RoleManagerVM roleManager);
        List<string> GetRoles();
        Task CreateRole(string roleName);
    }
}
