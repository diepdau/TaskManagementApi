using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using TaskManagementApi.Models;
using TaskManagementApi.Repositories;

namespace TaskManagementApi.Controllers
{
    [Route("api/tasks")]
    [ApiController]
    [Authorize]

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
        public IActionResult AddTask([FromBody] TaskRequest task)
        {
            if (task == null || string.IsNullOrWhiteSpace(task.Title) || string.IsNullOrWhiteSpace(task.Description))
                return BadRequest("Task title and description are required.");

            if (_userRepository.GetById(task.UserId) == null)
                return NotFound($"User with Id {task.UserId} does not exist.");

            if (_categoryRepository.GetById(task.CategoryId) == null)
                return NotFound($"Category with Id {task.CategoryId} does not exist.");

            var newTask = new Models.Task
            {
                Title = task.Title,
                Description = task.Description,
                IsCompleted = task.IsCompleted ?? false,
                UserId = task.UserId,
                CategoryId = task.CategoryId,
                CreatedAt = task.CreatedAt ?? DateTime.UtcNow
            };

            _taskRepository.Add(newTask);
            return CreatedAtAction(nameof(GetTaskById), new { id = newTask.Id }, newTask);
        }
            [HttpPut("{id}")]
            public IActionResult UpdateTask(int id, [FromBody] TaskRequest task)
            {
                var existingTask = _taskRepository.GetById(id);
                if (existingTask == null)
                    return NotFound(new { message = "Task not found." });

                if (_userRepository.GetById(task.UserId) == null)
                    return NotFound($"User with Id {task.UserId} does not exist.");

                if (_categoryRepository.GetById(task.CategoryId) == null)
                    return NotFound($"Category with Id {task.CategoryId} does not exist.");

                existingTask.Title = task.Title;
                existingTask.Description = task.Description;
                existingTask.IsCompleted = task.IsCompleted;
                existingTask.UserId = task.UserId;
                existingTask.CategoryId = task.CategoryId;
                existingTask.CreatedAt = task.CreatedAt ?? DateTime.UtcNow;

                _taskRepository.Update(existingTask);
                return Ok(existingTask);
            }


        [HttpDelete("{id}")]
        public IActionResult DeleteTask(int id)
        {
            var taskDel = _taskRepository.GetById(id);
            if (taskDel == null)
            {
                return NotFound(new { message = $"task with Id {id} does not exist." });
            }

            _taskRepository.Delete(id);
            return NoContent();
        }
        public class TaskRequest
        {
            [Required(ErrorMessage = "Title is required.")]
            public string Title { get; set; } = null!;

            public string? Description { get; set; }

            public bool? IsCompleted { get; set; } = false;

            [Required(ErrorMessage = "UserId is required.")]
            public int UserId { get; set; }

            [Required(ErrorMessage = "CategoryId is required.")]
            public int CategoryId { get; set; }
            public DateTime? CreatedAt { get; set; }
        }


    }
}