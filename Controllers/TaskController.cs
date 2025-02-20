using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagementApi.Models;
using TaskManagementApi.Services;

namespace TaskManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet]
        public ActionResult<List<TaskItem>> GetTasks() => _taskService.GetAllTasks();

        [HttpGet("{id}")]
        public ActionResult<TaskItem> GetTaskById(int id)
        {
            var task = _taskService.GetTaskById(id);
            return task != null ? Ok(task) : NotFound();
        }

        [HttpPost]
        public IActionResult AddTask(string title, bool completed)
        {
            TaskItem newTask = new TaskItem
            {
                Title = title,
                Completed = completed
            };
            _taskService.AddTask(newTask);
            return CreatedAtAction(nameof(GetTaskById), new { id = newTask.Id }, newTask);
        }


        [HttpPut("{id}")]
        public IActionResult UpdateTask(int id, string title, bool completed)
        {
            var existingTask = _taskService.GetTaskById(id);
            if (existingTask == null) return NotFound();

            existingTask.Title = title;
            existingTask.Completed = completed;
            _taskService.UpdateTask(existingTask);
            return Ok(existingTask);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTask(int id)
        {
            _taskService.DeleteTask(id);
            return NoContent();
        }


    }
}