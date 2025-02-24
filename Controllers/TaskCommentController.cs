using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagementApi.Models;
using TaskManagementApi.Repositories;

namespace TaskManagementApi.Controllers
{
    [Route("api/task-comments")]
    [ApiController]
    public class TaskCommentController : ControllerBase
    {
        private readonly TaskCommentRepository _taskCommentRepository;
        private readonly TaskRepository _taskRepository;
        private readonly UserRepository _userRepository;
        public TaskCommentController(TaskCommentRepository taskCommentRepository , UserRepository userRepository, TaskRepository taskRepository)
        {
            _taskCommentRepository = taskCommentRepository;
            _taskRepository = taskRepository;
            _userRepository = userRepository;
        }

        [HttpPost]
        public IActionResult AddComment(int taskId,int userId, string content, DateTime? createAt = null)
        {
            var taskExists = _taskRepository.GetById(taskId) != null;
            if (!taskExists)
                return NotFound($"Task with Id {taskId} does not exist.");

            var userExists = _userRepository.GetById(userId) != null;
            if (!userExists)
                return NotFound($"User with Id {userId} does not exist.");
            TaskComment newTaskComment = new TaskComment
            {
                TaskId = taskId,
                UserId = userId,
                Content = content,
                CreatedAt = createAt ?? DateTime.UtcNow,
            };
            _taskCommentRepository.Add(newTaskComment);
            return CreatedAtAction(nameof(AddComment), newTaskComment);
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteComment(int id)
        {
            _taskCommentRepository.Delete(id);
            return NoContent();
        }
    }
}

