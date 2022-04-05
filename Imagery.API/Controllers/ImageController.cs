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
        [Authorize]
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
        [Authorize]
        public ActionResult<string> ItemUpload(int id, [FromForm] ItemUploadVM item)
        {

            var response = ImageService.UploadItem(id, item);

            if (response == null)
            {
                return BadRequest("Image not uploaded, try again!");
            }

            return Ok(response);
        }

        [HttpPost("{id}")]
        [Authorize]
        public ActionResult<DimensionsVM> AddDimension(int id, [FromBody]DimensionsVM dimension)
        {
            if (dimension == null)
            {
                return BadRequest("Error, try again!");
            }

            var result = ImageService.AddDimensions(id, dimension);

            if (result == null)
            {
                return BadRequest("Error, dimensions have not been added!");
            }

            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public ActionResult<DimensionsVM> DeleteDimension(int id)
        {
            var result = ImageService.RemoveDimensions(id);

            if (!result)
            {
                return BadRequest(new { Message = "Deletion failed, try again!", isSuccess = false });
            }

            return Ok(new { Message = "Deletion successfull!", isSuccess = true });
        }

        [HttpPut("{id}")]
        [Authorize]
        public ActionResult<EditItemVM> ItemUpdate(int id, [FromForm]EditItemVM editItem)
        {
            if (editItem == null)
            {
                return BadRequest("Invalid data!");
            }

            var result = ImageService.UpdateExponentItem(id, editItem);

            if (result == null)
            {
                return BadRequest("Error, something went wrong!");
            }

            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public ActionResult DeleteItem(int id)
        {
            var result = ImageService.RemoveItem(id);

            if (!result)
            {
                return BadRequest(new { Message = "Deletion failed, try again!", isSuccess = false });
            }

            return Ok(new { Message = "Deletion successfull!", isSuccess = true });
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> BuyItem([FromBody] CollectionVM collectionItems)
        {
            if (collectionItems == null)
            {
                return BadRequest(new { Message = "Error, item doesn't exist!", isSuccess = false });
            }

            var response = await ImageService.AddColection(collectionItems);

            if (!response)
            {
                return BadRequest(new { Message = "Error, while buying item!", isSuccess = false });
            }

            return Ok(new { Message = "Purchase sucessfull!", isSuccess = true });
        }

        [HttpGet("{username}")]
        [Authorize]
        public async Task<ActionResult<List<CollectionItemVM>>> GetCollection(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return null;
            }

            try
            {
                var result = await ImageService.GetCollection(username);
                return result;
            }
            catch (Exception exc)
            {
                return BadRequest(new { Message = exc.InnerException?.Message });
            }

        }
    }
}
