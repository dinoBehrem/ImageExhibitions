﻿using Imagery.Service.Services.Exhbition;
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

        public ExhibitionController(IExhibitionService exhibitionService)
        {
            ExhibitionService = exhibitionService;
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

        [HttpPost]
        [Authorize]
        public ActionResult<ExhibitionVM> Update([FromBody] ExhibitionVM exhbitionVM)
        {
            if (exhbitionVM == null)
            {
                return BadRequest("Error, inavlid data!");
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

        [HttpPost]
        [Authorize]
        public ActionResult<string> UpadteCoverImage([FromBody]CoverImageVM coverImage)
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

            return Ok(result);
        }
    }
}
