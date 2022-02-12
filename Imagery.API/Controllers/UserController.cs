using Imagery.Service.Services.Authentication;
using Imagery.Service.ViewModels.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Imagery.API.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService UserService;

        public UserController(IUserService userService)
        {
            UserService = userService;
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterVM register)
        {
            if (register == null)
            {
                return BadRequest("Invalid credentials!");
            }

            Response response = await UserService.SignUp(register);

            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginVM login)
        {
            if (login == null)
            {
                return BadRequest("Invalid credentials!");
            }

            AuthResponse authResponse = await UserService.SignIn(login);

            if (authResponse == null)
            {
                return new BadRequestResult();
            }

            return Ok(authResponse);
        }
    }
}
