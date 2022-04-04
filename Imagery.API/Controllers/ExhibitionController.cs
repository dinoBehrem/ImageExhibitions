using Imagery.Service.Helpers;
using Imagery.Service.Services.Exhbition;
using Imagery.Service.Services.Topics;
using Imagery.Service.ViewModels;
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
        public async Task<ActionResult> Create([FromBody] ExhbitionCreationVM exhbitionCreationVM)
        {
            // check if input is valid
            if (exhbitionCreationVM == null)
            {
                return BadRequest(new { Message = "Invalid input!" });
            }

            // add exhbition
            try
            {
                var response = await ExhibitionService.Create(exhbitionCreationVM);

                return Ok(response.Id);

            }
            catch (Exception exc)
            {
                return BadRequest(new { Message = exc.Message });
            }

        }

        [HttpPut("{id}")]
        [Authorize]
        public ActionResult<EditExhibitionVM> Update(int id, [FromBody] EditExhibitionVM exhbitionVM)
        {
            if (exhbitionVM == null)
            {
                return BadRequest(new { Message = "Error, invalid data!" });
            }

            try
            {
                var response = ExhibitionService.UpdateExhibition(id, exhbitionVM);

                return Ok(response);
            }
            catch (Exception exc)
            {
                return BadRequest(new { Message = exc.Message });
            }
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
                return BadRequest(new { Message = "Invalid exhibition id!" });
            }

            try
            {
                var serviceresponse = ExhibitionService.GetById(id);
                return Ok(serviceresponse);
            }
            catch (Exception exc)
            {
                return BadRequest(new { Message = exc.Message });
            }
        }

        [HttpPut]
        [Authorize]
        public ActionResult<CoverImageVM> UpadteCoverImage([FromBody] CoverImageVM coverImage)
        {
            if (coverImage == null)
            {
                return BadRequest(new { Message = "Invalid data, please try again!" });
            }
            try
            {
                var result = ExhibitionService.SetExhibitionCover(coverImage);

                CoverImageVM cover = new CoverImageVM() { CoverImage = result };

                return Ok(cover);

            }
            catch (Exception exc)
            {
                return BadRequest(new { Message = exc.Message });
            }

        }

        [HttpPost]
        [Authorize]
        public ActionResult<TopicVM> AssignTopic([FromBody] AssignTopicVM assignTopic)
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
        public ActionResult<List<MyExhibitionVM>> GetUserExhibitions(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return BadRequest(new { Message = "Inavlid username, try again!" });
            }

            try
            {
                var result = ExhibitionService.UserExhibitions(username);
                var count = ExhibitionService.GetTotalCount();

                return Ok(new { Exhibitions = result, Count = count });

            }
            catch (Exception exc)
            {
                return BadRequest(new { Message = exc.Message});
            }
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<List<MyExhibitionVM>>> MyExhibitions(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return BadRequest(new { Message = "Inavlid username, try again!" });
            }

            try
            {
                var result = await ExhibitionService.MyExhibitions(username);
                var count = ExhibitionService.GetTotalCount();

                return Ok(new { Exhibitions = result, Count = count });

            }
            catch (Exception exc)
            {
                return BadRequest(new { Message = exc.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public ActionResult DeleteExhbition(int id)
        {
            try
            {
                var response = ExhibitionService.RemoveExhbition(id);
                return Ok(new { Message = "Exhibition successfully deleted!"});
            }
            catch (Exception exc)
            {
                return BadRequest(new { Message = exc.Message });
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Subscribe([FromBody] ExhibitionSubscriptionVM subscription)
        {
            if (subscription == null || string.IsNullOrEmpty(subscription.Username))
            {
                return BadRequest(new { Message = "Invalid data, try again!" });
            }

            try
            {
                var result = await ExhibitionService.Subscribe(subscription);

                return Ok(new { Message = "Subscription successfull!" });
            }
            catch (Exception exc)
            {

                return BadRequest(new { Message = exc.Message });
            }

        }
    
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Unsubscribe([FromBody] ExhibitionSubscriptionVM subscription)
        {
            if (subscription == null || string.IsNullOrEmpty(subscription.Username))
            {
                return BadRequest("Invalid data, try again!");
            }

            try
            {
                var result = await ExhibitionService.Unsubscribe(subscription);

                return Ok(new { Message = "You've unsubscribed successfully!" });
            }
            catch (Exception exc)
            {

                return BadRequest(new { Message = exc.Message });
            }
        }

        [HttpGet]
        public ActionResult<List<ExhibitionVM>> FilterByTitle([FromQuery]string title)
        {
            if (string.IsNullOrEmpty(title))
            {
                return BadRequest(new { Message = "Please provide a title" });
            }

            var result = ExhibitionService.FilterByName(title);
            int count = ExhibitionService.GetTotalCount();

            return Ok(new { Exhibitions = result, Count = count });
        }

        [HttpGet]
        public ActionResult<List<ExhibitionVM>> GetByFilters([FromQuery]FilterVM filters, [FromQuery] PageParameters pageParameters)
        {
            var response = ExhibitionService.GetFilteredExhbition(filters, pageParameters);
            int count = ExhibitionService.GetTotalCount();

            return Ok(new { Exhibitions = response, Count = count });
        }

        // Pagination test

        [HttpGet]
        public ActionResult<List<ExhibitionVM>> GetPaged([FromQuery]PageParameters pageParameters)
        {
            var exhibitions = ExhibitionService.GetPagedExhbition(pageParameters);
            int count = ExhibitionService.GetTotalCount();

            return Ok(new { Exhibitions = exhibitions, Count = count });
        }
    }
}
