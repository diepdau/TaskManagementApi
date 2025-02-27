﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagementApi.Models;
using TaskManagementApi.Repositories;

namespace TaskManagementApi.Controllers
{
    [Route("api/labels")]
    [ApiController]
    public class LabelController : ControllerBase
    {
        private readonly LabelRepository _labelRepository;

        public LabelController(LabelRepository labelRepository)
        {
            _labelRepository = labelRepository;
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetAllLabel() => Ok(_labelRepository.GetAll());

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult AddLabel([FromBody] Label label)
        {
            if (label == null || string.IsNullOrWhiteSpace(label.Name))
                return BadRequest("Label name is required.");
            if (_labelRepository.GetByName(label.Name) != null)
                return Conflict("label name must be unique.");
            _labelRepository.Add(label);
            return CreatedAtAction(nameof(AddLabel), new { id = label.Id }, label);
        }


    }
}
