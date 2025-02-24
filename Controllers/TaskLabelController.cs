using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagementApi.Models;
using TaskManagementApi.Repositories;

namespace TaskManagementApi.Controllers
{
    [Route("api/task-labels")]
    [ApiController]
    public class TaskLabelController : ControllerBase
    {
        private readonly TaskLabelRepository _taskLabelRepository;
        private readonly TaskRepository _taskRepository;
        private readonly LabelRepository _labelRepository; 

        public TaskLabelController(TaskLabelRepository taskLabelRepository, TaskRepository taskRepository, LabelRepository labelRepository)
        {
            _taskLabelRepository = taskLabelRepository;
            _taskRepository = taskRepository;
            _labelRepository = labelRepository;
        }

        [HttpGet]
        public IActionResult GetAllTaskLabels()
        {
            var taskLabels = _taskLabelRepository.GetAll();
            return Ok(taskLabels);
        }


        [HttpPost]
        public IActionResult AddTaskLabel([FromQuery] int taskId, [FromQuery] int labelId)
        {
            if (taskId <= 0 || labelId <= 0)
                return BadRequest("Invalid TaskId or LabelId.");

            var taskExists = _taskRepository.GetById(taskId) != null;
            if (!taskExists)
                return NotFound($"Task with Id {taskId} does not exist.");

            var labelExists = _labelRepository.GetById(labelId) != null;
            if (!labelExists)
                return NotFound($"Label with Id {labelId} does not exist.");

            if (_taskLabelRepository.Exists(taskId, labelId))
                return Conflict("TaskLabel already exists.");

            var newTaskLabel = new TaskLabel
            {
                TaskId = taskId,
                LabelId = labelId
            };

            _taskLabelRepository.Add(newTaskLabel);
            return CreatedAtAction(nameof(AddTaskLabel), new { taskId, labelId }, newTaskLabel);
        }


        [HttpDelete("{taskId}/{labelId}")]
        public IActionResult RemoveLabel(int taskId, int labelId)
        {
            _taskLabelRepository.RemoveLabel(taskId, labelId);
            return NoContent();
        }

    }
}