using System.Security.Claims;
using BusinessObjects.DTOs.Board.Request;
using BusinessObjects.DTOs.Board.Response;
using BusinessObjects.DTOs.Column.Request;
using BusinessObjects.DTOs.Label.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.BoardServices;
using Services.ColumnServices;
using Services.LabelServices;
using Services.UserServices;

namespace TaskManagement.Controllers
{
    [Route("api/boards")]
    [ApiController]
    public class BoardController : ControllerBase
    {
        private readonly IBoardService _boardService;
        private readonly IColumnService _columnService;
        private readonly ILabelService _labelService;
        private readonly IUserService _userService;

        public BoardController(IBoardService boardService, IColumnService columnService, ILabelService labelService, IUserService userService)
        {
            _boardService = boardService;
            _columnService = columnService;
            _labelService = labelService;
            _userService = userService;
        }

        [HttpGet]
        [Route("{boardId}")]
        [Authorize]
        public async Task<IActionResult> GetBoardDetailById(string boardId)
        {
            var userId = HttpContext.User.FindFirstValue("id");
            var response = await _boardService.GetBoardDetailById(boardId, userId);
            return Ok(response);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetBoardsByUserId()
        {
            var userId = HttpContext.User.FindFirstValue("id");
            var response = await _boardService.GetBoardsByUserId(userId);
            return Ok(response);
        }

        [HttpGet]
        [Authorize]
        [Route("templates")]
        public async Task<IActionResult> GetBoardTemplates()
        {
            var response = await _boardService.GetBoardTemplatesForSetup();
            return Ok(response);
        }        

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddBoard(BoardAddRequest request)
        {
            var userId = HttpContext.User.FindFirstValue("id");
            await _boardService.AddBoard(request, userId);
            return Ok();
        }

        [HttpPut]
        [Authorize]
        [Route("{boardId}")]
        public async Task<IActionResult> UpdateBoard(string boardId, BoardUpdateRequest request)
        {
            var userId = HttpContext.User.FindFirstValue("id");
            await _boardService.UpdateBoard(boardId, request, userId);
            return Ok();
        }

        [HttpDelete]
        [Authorize]
        [Route("{boardId}")]
        public async Task<IActionResult> DeleteBoard(string boardId)
        {
            var userId = HttpContext.User.FindFirstValue("id");
            await _boardService.DeleteBoard(boardId, userId);
            return Ok();
        }

        [HttpPost]
        [Authorize]
        [Route("{boardId}/columns")]
        public async Task<IActionResult> AddColumn(string boardId, ColumnAddRequest request)
        {
            var column = await _columnService.AddColumn(request, boardId);
            return Ok(column);
        }

        [HttpPost]
        [Authorize]
        [Route("{boardId}/labels")]
        public async Task<IActionResult> AddLabel(string boardId, LabelAddRequest request)
        {
            var label = await _labelService.AddLabel(boardId, request);
            return Ok(label);
        }

        [HttpPatch]
        [Authorize]
        [Route("{boardId}/members")]
        public async Task<IActionResult> AddMemberToBoard(string boardId, [FromBody] ICollection<string> emails)
        {
            await _userService.AddMemberToBoard(boardId, emails);
            return Ok();
        }

        [HttpDelete]
        [Authorize]
        [Route("{boardId}/members")]
        public async Task<IActionResult> RemoveMemberFromBoard(string boardId, [FromBody] ICollection<string> emails)
        {
            await _userService.RemoveMemberFromBoard(boardId, emails);
            return Ok();
        }
    }
}
