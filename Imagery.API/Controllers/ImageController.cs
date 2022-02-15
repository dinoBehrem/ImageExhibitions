using Imagery.Service.Services.Image;
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
    public class ImageController : ControllerBase
    {
        private readonly IImageService ImageService;

        public ImageController(IImageService imageService)
        {

            ImageService = imageService;
        }
        public class Picture
        {
            public IFormFile Image { get; set; }
        }

        [HttpPost]
        public async Task<ActionResult<string>> UploadImage(string username, [FromForm] Picture picture)
        {

            if (string.IsNullOrEmpty(username))
            {
                return BadRequest("Invalid username!");
            }
            else if (!(picture.Image.Length > 0))
            {
                return BadRequest("Invalid file, please select another image!");
            }

            string response = await ImageService.UploadProfilePicture(username, picture.Image);

            if (string.IsNullOrEmpty(response))
            {
                return BadRequest("Error while saving picture, try again!");
            }

            return Ok(response);
        }
    }
}
