using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagementApi.Models;
using TaskManagementApi;
using TaskManagementApi.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace TaskManagementApi.Controllers
{
    [Authorize]
    [Route("api/categories")]
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
        [HttpGet("search")]
        public IActionResult GetTasks(string? keyword, int page = 1, int pageSize = 3)
        {
            int totalItems;

            var tasks = _categoryRepository.GetPaged(
                filter: t => string.IsNullOrEmpty(keyword) || t.Name.Contains(keyword) || t.Description.Contains(keyword),
                page: page,
                pageSize: pageSize,
                totalItems: out totalItems
            );

            return Ok(new
            {
                TotalItems = totalItems,
                Page = page,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling((double)totalItems / pageSize),
                Data = tasks
            });
        }




    }
}
