using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagementApi.Models;
using TaskManagementApi.Repositories;

namespace TaskManagementApi.Controllers
{
    [Route("api/labels")]
    [ApiController]
    [Authorize]
    public class LabelController : ControllerBase
    {
        private readonly LabelRepository _labelRepository;

        public LabelController(LabelRepository labelRepository)
        {
            _labelRepository = labelRepository;
        }
        [HttpGet]
        public IActionResult GetAllLabel() => Ok(_labelRepository.GetAll());

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult AddLabel(string name)
        {
            Label newLabel = new Label
            {
                Name = name,
            };
            _labelRepository.Add(newLabel);
            return CreatedAtAction(nameof(AddLabel), newLabel);
        }

        
    }
}
