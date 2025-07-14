using Azure.Core;
using BusinessObjects.Constants;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.UserServices;
using BusinessObjects.DTOs.Board.Request;
using Services.BoardServices;

namespace TaskManagement.Controllers
{
    [Route("api/admin")]
    [ApiController]
    [Authorize(Roles = $"{UserRoles.Admin}")]
    public class AdminController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IBoardService _boardService;

        public AdminController(IUserService userService, IBoardService boardService)
        {
            _userService = userService;
            _boardService = boardService;
        }

        [HttpPost]
        [Route("templates")]
        public async Task<IActionResult> AddBoardTemplate(BoardTemplateAddRequestModel request)
        {
            var userId = HttpContext.User.FindFirstValue("id");
            await _boardService.AddBoardTemplate(request, userId);
            return Ok();
        }

        [HttpGet]
        [Route("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsers();

            return Ok(users);
        }
    }
}
