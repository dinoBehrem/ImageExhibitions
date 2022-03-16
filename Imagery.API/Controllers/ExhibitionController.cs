using Imagery.Service.Services.Exhbition;
using Imagery.Service.Services.Topics;
using Imagery.Service.ViewModels.Exhbition;
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
    public class ExhibitionController : ControllerBase
    {
        private readonly IExhibitionService ExhibitionService;
        private readonly ITopicService TopicService;

        public ExhibitionController(IExhibitionService exhibitionService, ITopicService topicService)
        {
            ExhibitionService = exhibitionService;
            TopicService = topicService;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Create([FromBody]ExhbitionCreationVM exhbitionCreationVM)
        {
            // check if input is valid
            if (exhbitionCreationVM == null)
            {
                return BadRequest("Invalid input!");
            }

            // add exhbition
            var response = await ExhibitionService.Create(exhbitionCreationVM);

            // check response for success
            if (response == null)
            {
                return BadRequest("Error, try again!");
            }

            return Ok(response.Id);
        }

        [HttpPut("{id}")]
        [Authorize]
        public ActionResult<EditExhibitionVM> Update(int id, [FromBody] EditExhibitionVM exhbitionVM)
        {
            if (exhbitionVM == null)
            {
                return BadRequest("Error, invalid data!");
            }

            var response = ExhibitionService.UpdateExhibition(exhbitionVM);

            if (response == null)
            {
                return BadRequest("Update failed, try again!");
            }

            return Ok(response);
        }

        [HttpGet]
        public ActionResult<List<ExhibitionVM>> GetAll()
        {
            var exhbitions = ExhibitionService.Exhibitions();

            return Ok(exhbitions);
        }

        [HttpGet("{id}")]
        public ActionResult<ExhibitionVM> GetExhbition(int id)
        {
            if (id == -1)
            {
                return BadRequest("Invalid exhbition id!");
            }

            var serviceresponse = ExhibitionService.GetById(id);

            if (serviceresponse == null)
            {
                return BadRequest("Exhbition not found, try again!");
            }

            return Ok(serviceresponse);
        }

        [HttpPut]
        [Authorize]
        public ActionResult<CoverImageVM> UpadteCoverImage([FromBody]CoverImageVM coverImage)
        {
            if (coverImage == null)
            {
                return BadRequest("Invalid data, please try again!");
            }

            var result = ExhibitionService.SetExhibitionCover(coverImage);

            if (result == null)
            {
                return BadRequest("Invalid data, please try again!");
            }

            CoverImageVM cover = new CoverImageVM() { CoverImage = result };

            return Ok(cover);
        }

        [HttpPost]
        [Authorize]
        public ActionResult<TopicVM> AssignTopic([FromBody]AssignTopicVM assignTopic)
        {
            if (assignTopic == null)
            {
                return BadRequest("Error, something went wrong!");
            }

            var result = ExhibitionService.AssignTopic(assignTopic);

            if (result == null)
            {
                return BadRequest("Topic not assigned, try again!");
            }

            return Ok(result);
        }

        [HttpGet]
        public ActionResult<List<TopicVM>> GetTopics()
        {
            var result = TopicService.GetAllTopics();

            if (result == null)
            {
                return BadRequest("Error, topics not loaded!");
            }

            return Ok(result);
        }

        [HttpGet("{username}")]
        //[Authorize]
        public ActionResult<List<ExhibitionVM>> GetUserExhibitions(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return BadRequest("Inavlid username, try again!");
            }

            var result = ExhibitionService.UserExhibitions(username);

            if (result == null)
            {
                return BadRequest("Error, something went wrong!");
            }

            return result;
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteExhbition(int id)
        {
            var response = ExhibitionService.RemoveExhbition(id);

            if (!response)
            {
                return BadRequest(new { Message = "Error while deleting exhbition!", isSuccess = false });
            }

                return Ok(new { Message = "Exhibition successfully deleted!", isSuccess = true });
        }
        
    }
}
