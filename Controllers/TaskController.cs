﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagementApi.Models;
using TaskManagementApi.Repositories;

namespace TaskManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly TaskRepository _taskRepository;

        public TaskController(TaskRepository taskService)
        {
            _taskRepository = taskService;
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
        public IActionResult AddTask(string title, string description, bool isCompleted, int userId, int categoryId, DateTime createAt)
        {
            Models.Task newTask = new Models.Task
            {
                Title = title,
                Description = description,
                IsCompleted = isCompleted,
                UserId = userId,
                CategoryId = categoryId,
                CreatedAt = createAt,
            };
            _taskRepository.Add(newTask);
            return CreatedAtAction(nameof(GetTaskById), new { id = newTask.Id }, newTask);
        }


        [HttpPut("{taskId}")]
        public IActionResult UpdateTask(int taskId, string title, string description, bool isCompleted, int userId, int categoryId, DateTime createAt)
        {
            var existingTask = _taskRepository.GetById(taskId);
            if (existingTask == null)
            {
                return NotFound(new { message = "Task not found" });
            }

            existingTask.Title = title;
            existingTask.Description = description;
            existingTask.IsCompleted = isCompleted;
            existingTask.UserId = userId;
            existingTask.CategoryId = categoryId;
            existingTask.CreatedAt = createAt;

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