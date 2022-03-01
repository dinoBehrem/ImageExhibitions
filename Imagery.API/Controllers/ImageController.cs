using Imagery.Service.Services.Image;
using Imagery.Service.ViewModels.Image;
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

        [HttpPost]
        public async Task<ActionResult<string>> ProfilePictureUpload(string username, [FromForm] ProfilePictureVM picture)
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

        [HttpPost("{id}")]
        public ActionResult<string> ItemUpload(int id, [FromForm] ItemUploadVM item)
        {

            var response = ImageService.UploadItem(id, item);

            if (response == null)
            {
                return BadRequest("Image not uploaded, try again!");
            }

            return Ok(response);
        }
    }
}
