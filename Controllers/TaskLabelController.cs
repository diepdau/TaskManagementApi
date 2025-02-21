using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagementApi.Models;
using TaskManagementApi.Repositories;

namespace TaskManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskLabelController : ControllerBase
    {
        private readonly TaskLabelRepository _taskLabelRepository;

        public TaskLabelController(TaskLabelRepository taskLabelRepository)
        {
            _taskLabelRepository = taskLabelRepository;
        }
        [HttpGet]
        //public IActionResult GetAllTasks() => Ok(_taskLabelRepository.GetAll());


        //[HttpPost]
        //public IActionResult AssignLabel([FromBody] TaskLabel taskLabel)
        //{
        //    _taskLabelRepository.Add(taskLabel);
        //    return CreatedAtAction(nameof(AssignLabel), taskLabel);
        //}

        
        [HttpPost]
        public IActionResult AddTaskLabel(int taskId, int labelId)
        {
            TaskLabel newTaskLabel = new TaskLabel
            {
                TaskId = taskId,
                LabelId = labelId

            };
            _taskLabelRepository.Add(newTaskLabel);
            return CreatedAtAction(nameof(AddTaskLabel), newTaskLabel);
        }
        //[HttpDelete("{taskId}/{labelId}")]
        //public IActionResult RemoveLabel(int taskId, int labelId)
        //{
        //    var taskLabels = _taskLabelRepository.GetAll()
        //                        .Where(tl => tl.TaskId == taskId && tl.LabelId == labelId)
        //                        .ToList();

        //    if (!taskLabels.Any())
        //        return NotFound("Label không tồn tại trong Task.");

        //    foreach (var tl in taskLabels)
        //    {
        //        if (tl.LabelId.HasValue)
        //            _taskLabelRepository.Delete(tl.LabelId.Value);
        //    }

        //    return NoContent();
        //}
        //////////////
        //[HttpDelete("{taskId}/{labelId}")]
        //public IActionResult RemoveLabel(int taskId, int labelId)
        //{
        //    var taskLabels = _taskLabelRepository.GetAll()
        //                        .Where(tl => tl.TaskId == taskId && tl.LabelId == labelId)
        //                        .ToList();

        //    if (!taskLabels.Any())
        //        return NotFound("Label không tồn tại trong Task.");

        //    foreach (var tl in taskLabels)
        //    {
        //        if (tl.LabelId.HasValue)
        //            _taskLabelRepository.Delete(tl.LabelId.Value);
        //    }

        //    return NoContent();
        //}

    }
}