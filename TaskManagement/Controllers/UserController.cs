using System.Security.Claims;
using BusinessObjects.DTOs.User.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.UserServices;

namespace TaskManagement.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Route("me")]
        [Authorize]
        public async Task<IActionResult> GetProfile()
        {
            string? currentUserId = HttpContext.User.FindFirstValue("id");
            var user = await _userService.GetUserProfile(currentUserId);
            return Ok(user);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SearchUser(string? search, [FromBody] List<string> exeptIds)
        {
            if (string.IsNullOrEmpty(search)) return Ok(new List<UserProfileResponseModel>());

            var response = await _userService.SearchUser(search, exeptIds);
            return Ok(response);
        }
    }
}
