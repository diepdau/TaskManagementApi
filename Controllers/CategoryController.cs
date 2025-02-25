using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagementApi.Models;
using TaskManagementApi;
using TaskManagementApi.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace TaskManagementApi.Controllers
{
    [Route("api/categories")]
    [ApiController]
    [Authorize]

    public class CategoryController : ControllerBase
    {
        private readonly CategoryRepository _categoryRepository;

        public CategoryController(CategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        public IActionResult GetAllCategories() => Ok(_categoryRepository.GetAll());

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult AddCategory([FromBody] Category category)
        {
            if (category == null || string.IsNullOrWhiteSpace(category.Name) || string.IsNullOrWhiteSpace(category.Description))
                return BadRequest("Category name and description are required.");

            if (_categoryRepository.GetByName(category.Name) != null)
                return Conflict("Category name must be unique.");

            _categoryRepository.Add(category);
            return CreatedAtAction(nameof(GetAllCategories), new { id = category.Id }, category);
        }




    }
}
