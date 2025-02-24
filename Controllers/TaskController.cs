using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagementApi.Models;
using TaskManagementApi.Repositories;

namespace TaskManagementApi.Controllers
{
    [Route("api/tasks")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly TaskRepository _taskRepository;
        private readonly UserRepository _userRepository;
        private readonly CategoryRepository _categoryRepository;
        public TaskController(TaskRepository taskService, UserRepository userRepository, CategoryRepository categoryRepository)
        {
            _taskRepository = taskService;
            _userRepository = userRepository;
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        public IActionResult GetAllTasks() => Ok(_taskRepository.GetAll());

        [HttpGet("{id}")]
        public ActionResult GetTaskById(int id)
        {
            var task = _taskRepository.GetById(id);
            return task != null ? Ok(task) : NotFound();
        }

        [HttpPost]
        public IActionResult AddTask(string title, string description, bool isCompleted, int userId, int categoryId, DateTime? createAt = null)
        {
            var userExist = _userRepository.GetById(userId) != null;
            if (!userExist) return NotFound($"User with Id {userId} does not exist.");

            var categoryExist = _categoryRepository.GetById(categoryId) != null;
            if (!categoryExist) return NotFound($"Category with Id {categoryId} does not exist.");
            Models.Task newTask = new Models.Task
            {
                Title = title,
                Description = description,
                IsCompleted = isCompleted,
                UserId = userId,
                CategoryId = categoryId,
                CreatedAt = createAt ?? DateTime.UtcNow,
            };
            _taskRepository.Add(newTask);
            return CreatedAtAction(nameof(GetTaskById), new { id = newTask.Id }, newTask);
        }


        [HttpPut("{id}")]
        public IActionResult UpdateTask(int id, string title, string description, bool isCompleted, int userId, int categoryId, DateTime? createAt = null)
        {
            

            var existingTask = _taskRepository.GetById(id);
            if (existingTask == null)
            {
                return NotFound(new { message = "Task not found" });
            }
            var userExist = _userRepository.GetById(userId) != null;
            if (!userExist) return NotFound($"User with Id {userId} does not exist");

            var categoryExist = _categoryRepository.GetById(categoryId) != null;
            if (!categoryExist) return NotFound($"Category with Id {categoryId} does not exist.");
            existingTask.Title = title;
            existingTask.Description = description;
            existingTask.IsCompleted = isCompleted;
            existingTask.UserId = userId;
            existingTask.CategoryId = categoryId;
            existingTask.CreatedAt = createAt ?? DateTime.UtcNow;

            _taskRepository.Update(existingTask);

            return Ok(existingTask);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTask(int id)
        {
            _taskRepository.Delete(id);
            return NoContent();
        }

    }
}