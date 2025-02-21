using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagementApi.Models;
using TaskManagementApi;
using TaskManagementApi.Repositories;

namespace TaskManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryRepository _categoryRepository;

        public CategoryController(CategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        public IActionResult GetAllCategories() => Ok(_categoryRepository.GetAll());
        
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var task = _categoryRepository.GetById(id);
            if (task == null)
            {
                return NotFound(new { message = "Task not found" });
            }
            return Ok(task);
        }

        [HttpPost]
        public IActionResult AddCategory(string name, string description)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest("Category name is required");

            var category = new Category
            {
                Name = name,
                Description = description
            };

            _categoryRepository.Add(category);
            return CreatedAtAction(nameof(GetAllCategories), category);
        }


    }
}
