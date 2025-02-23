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

        public TaskCommentController(TaskCommentRepository taskCommentRepository)
        {
            _taskCommentRepository = taskCommentRepository;
        }

        [HttpPost]
        public IActionResult AddComment(int taskId,int userId, string content, DateTime? createAt = null)
        {
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

