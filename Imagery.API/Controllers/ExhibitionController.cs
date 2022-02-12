using Imagery.Service.Services.Exhbition;
using Imagery.Service.ViewModels.Exhbition;
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

        public ExhibitionController(IExhibitionService exhibitionService)
        {
            ExhibitionService = exhibitionService;
        }

        [HttpPost]
        public async Task<ActionResult<string>> Create([FromBody]ExhbitionCreationVM exhbitionCreationVM)
        {
            // check if input is valid
            if (exhbitionCreationVM == null)
            {
                return BadRequest("Invalid input!");
            }

            // add exhbition
            var response = await ExhibitionService.Create(exhbitionCreationVM);

            // check response for success
            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpGet]
        public ActionResult<List<ExhibitionVM>> GetAll()
        {
            var exhbitions = ExhibitionService.Exhibitions();

            return Ok(exhbitions);
        }
    }
}
