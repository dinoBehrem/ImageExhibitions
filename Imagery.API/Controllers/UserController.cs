using Imagery.Service.Services.Authentication;
using Imagery.Service.Services.Image;
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
        private readonly IImageService ImageService;

        public UserController(IUserService userService, IImageService imageService)
        {
            UserService = userService;
            ImageService = imageService;
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

        [HttpGet("{username}")]
        [Authorize]
        public async Task<ActionResult<ProfileVM>> GetUser(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return BadRequest("Invalid username!");
            }

            ProfileVM user = await UserService.GetUserProfile(username);

            return Ok(user);
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<ProfileVM>> GetProfile(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return BadRequest($"Username {username} doesn't exist!");
            }

            var result = await UserService.GetUserProfile(username);

            return Ok(result);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Subscribe([FromBody] SubscribeVM subscription)
        {
            if (string.IsNullOrEmpty(subscription.Creator) || string.IsNullOrEmpty(subscription.Subscriber))
            {
                return BadRequest(new { Message = "Invalid username!", isSuccess = false });
            }

            if (subscription.Creator == subscription.Subscriber)
            {
                return BadRequest(new { Message = "Can't subscribe to yourself!", isSuccess = false });
            }

            var result = UserService.Subscribe(subscription);

            if (!result.Result)
            {
                return BadRequest(new { Message = "Subscription failed, try again!", isSuccess = false });
            }

            return Ok(new { Message = "Subscription successfull!", isSuccess = true });

        }

        [HttpPost]
        [Authorize]
        public ActionResult Unsubscribe([FromBody] SubscribeVM subscription)
        {
            if (string.IsNullOrEmpty(subscription.Creator) || string.IsNullOrEmpty(subscription.Subscriber))
            {
                return BadRequest(new { Message = "Invalid username!", isSuccess = false });
            }

            if (subscription.Creator == subscription.Subscriber)
            {
                return BadRequest(new { Message = "Can't unsubscribe to yourself!", isSuccess = false });
            }

            var result = UserService.Unsubscribe(subscription);

            if (!result.Result)
            {
                return BadRequest(new { Message = "Subscription failed, try again!", isSuccess = false });
            }

            return Ok(new { Message = "Subscription successfull!", isSuccess = true });

        }

        [HttpPut("{username}")]
        [Authorize]
        public async Task<ActionResult<UserEditVM>> EditAccount(string username, [FromForm]UserEditVM userEdit)
        {
            if (string.IsNullOrEmpty(username) || userEdit == null)
            {
                return BadRequest("Invalid username or data!");
            }

            var result = await UserService.EditProfile(username, userEdit);

            if (result == null)
            {
                return BadRequest("Profile not updated!");
            }

            return Ok(result);
        }
    }
}
