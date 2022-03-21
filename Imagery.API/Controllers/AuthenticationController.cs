using Imagery.Service.Services.Authentication;
using Imagery.Service.ViewModels.User;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Imagery.API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthService AuthService;

        public AuthenticationController(IAuthService userService)
        {
            AuthService = userService;
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public ActionResult<List<UserVM>> GetUsers()
        {
            return AuthService.GetUsers().ToList();
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<ActionResult<Response>> PromoteTo([FromBody]RoleManagerVM roleManager)
        {
            var result = await AuthService.AsignedToRole(roleManager);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<ActionResult<Response>> DemoteTo([FromBody] RoleManagerVM roleManager)
        {
            var result = await AuthService.RemoveFromRole(roleManager);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public ActionResult<string> GetRoles()
        {
            var roles = AuthService.GetRoles();

            return Ok(roles);
        }
    }
}
