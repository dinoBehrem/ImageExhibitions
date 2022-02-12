using Imagery.Core.Models;
using Imagery.Service.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagery.Service.Services.Authentication
{
    public interface ITokenService
    {
        Task<AuthResponse> BuildToken(User user);
    }
}
