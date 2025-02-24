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
        //public IActionResult GetAllCategories()
        //{
        //    var categories = _categoryRepository.GetAllWithTasks();
        //    return Ok(categories);
        //}
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult AddCategory(string name, string description)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest("Category name is required");
            var existingCategory = _categoryRepository.GetByName(name);
            if (existingCategory != null)
            {
                return Conflict("Category name must be unique.");
            }
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
