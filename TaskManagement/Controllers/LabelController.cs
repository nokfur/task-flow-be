using BusinessObjects.DTOs.Label.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.LabelServices;

namespace TaskManagement.Controllers
{
    [Route("api/labels")]
    [ApiController]
    public class LabelController : ControllerBase
    {
        private readonly ILabelService _labelService;

        public LabelController(ILabelService labelService)
        {
            _labelService = labelService;
        }

        [Authorize]
        [HttpPut]
        [Route("{labelId}")]
        public async Task<IActionResult> UpdateLabel(string labelId, LabelUpdateRequest request)
        {
            await _labelService.UpdateLabel(labelId, request);
            return Ok();
        }

        [Authorize]
        [HttpDelete]
        [Route("{labelId}")]
        public async Task<IActionResult> DeleteLabel(string labelId)
        {
            await _labelService.DeleteLabel(labelId);
            return Ok();
        }
    }
}
