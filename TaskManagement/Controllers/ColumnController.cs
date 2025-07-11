using BusinessObjects.DTOs.Column.Request;
using BusinessObjects.DTOs.WorkTask.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.ColumnServices;
using Services.TaskServices;

namespace TaskManagement.Controllers
{
    [Route("api/columns")]
    [ApiController]
    public class ColumnController : ControllerBase
    {
        private readonly IColumnService _columnService;
        private readonly ITaskService _taskService;

        public ColumnController(IColumnService columnService, ITaskService taskService)
        {
            _columnService = columnService;
            _taskService = taskService;
        }

        [HttpPut]
        [Authorize]
        [Route("{columnId}")]
        public async Task<IActionResult> UpdateColumn(string columnId, ColumnUpdateRequestModel request)
        {
            await _columnService.UpdateColumn(columnId, request);
            return Ok();
        }

        [HttpPatch]
        [Authorize]
        [Route("positions")]
        public async Task<IActionResult> UpdateColumnPositions(List<ColumnPositionUpdateRequestModel> request)
        {
            await _columnService.UpdateColumnPositions(request);
            return Ok();
        }

        [HttpDelete]
        [Authorize]
        [Route("{columnId}")]
        public async Task<IActionResult> DeleteColumn(string columnId)
        {
            await _columnService.DeleteColumn(columnId);
            return Ok();
        }

        [HttpPost]
        [Authorize]
        [Route("{columnId}/tasks")]
        public async Task<IActionResult> AddTask(string columnId, TaskAddRequestModel request)
        {
            var task = await _taskService.AddTask(columnId, request);
            return Ok(task);
        }

        [HttpDelete]
        [Authorize]
        [Route("{columnId}/tasks")]
        public async Task<IActionResult> DeleteAllTasks(string columnId)
        {
            await _taskService.DeleteAllTasks(columnId);
            return Ok();
        }
    }
}
