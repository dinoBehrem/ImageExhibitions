using Imagery.Core.Models;
using Imagery.Service.ViewModels.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Imagery.Service.Services.Authentication
{
    public class TokenService : ITokenService
    {
        private readonly UserManager<User> UserManager;

        public TokenService(UserManager<User> userManager)
        {
            UserManager = userManager;
        }

        public async Task<AuthResponse> BuildToken(User user)
        {

            var claims = new List<Claim>()
            {
                new Claim("Id", user.Id),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            var userClaims = await UserManager.GetClaimsAsync(user);
            var userRolesAsClaims = await UserRoles(user);

            claims.AddRange(userClaims);
            claims.AddRange(userRolesAsClaims);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@23"));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiriation = DateTime.UtcNow.AddHours(5);

            var token = new JwtSecurityToken(
                issuer: null, audience: null, claims: claims, expires: expiriation, signingCredentials: creds);

            AuthResponse authResponse = new AuthResponse()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiriation = expiriation
            };

            return authResponse;

        }
        private async Task<List<Claim>> UserRoles(User user)
        {
            var userRoles = await UserManager.GetRolesAsync(user);

            List<Claim> rolesToClaims = new List<Claim>();

            foreach (var role in userRoles)
            {
                rolesToClaims.Add(new Claim("role", role));
            }

            return rolesToClaims;

        }
    }

}
