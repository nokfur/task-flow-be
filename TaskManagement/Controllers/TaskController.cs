using BusinessObjects.DTOs.WorkTask.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.ColumnServices;
using Services.TaskServices;

namespace TaskManagement.Controllers
{
    [Route("api/tasks")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [Authorize]
        [HttpPut]
        [Route("{taskId}")]
        public async Task<IActionResult> UpdateTask(string taskId, TaskUpdateRequestModel request)
        {
            await _taskService.UpdateTask(taskId, request);
            return Ok();
        }

        [Authorize]
        [HttpDelete]
        [Route("{taskId}")]
        public async Task<IActionResult> DeleteTask(string taskId)
        {
            await _taskService.DeleteTask(taskId);
            return Ok();
        }

        [Authorize]
        [HttpPatch]
        [Route("{taskId}/labels/{labelId}")]
        public async Task<IActionResult> ToggleLabel(string taskId, string labelId)
        {
            await _taskService.ToggleLabel(taskId, labelId);
            return Ok();
        }

        [Authorize]
        [HttpPatch]
        [Route("reorder")]
        public async Task<IActionResult> ReorderTask(TaskReorderRequestModel request)
        {
            await _taskService.Reorder(request);
            return Ok();
        }
    }
}
