using BusinessObjects.DTOs.User.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.UserServices;

namespace TaskManagement.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(UserRegisterRequest request)
        {
            await _userService.Register(request);
            return Ok();
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(UserLoginRequest request)
        {
            var response = await _userService.Login(request);
            return Ok(response);
        }
    }
}
