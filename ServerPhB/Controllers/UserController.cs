using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ServerPhB.Services;
using ServerPhB.Models;

namespace ServerPhB.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly AuthenticationService _authenticationService;

        public UserController(AuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var (token, role) = await _authenticationService.Register(request.Username, request.Password, request.Name, request.Email, request.Phone, request.Role);
            if (token == null)
            {
                return BadRequest(new { message = "Username already exists" });
            }
            return Ok(new { token, role });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var (token, role) = await _authenticationService.Authenticate(request.Username, request.Password);
            if (token == null)
            {
                return Unauthorized(new { message = "Invalid username or password" });
            }
            return Ok(new { token, role });
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            var (token, refreshToken) = await _authenticationService.RefreshToken(request.Token);
            if (token == null)
            {
                return Unauthorized(new { message = "Invalid refresh token" });
            }
            return Ok(new { token, refreshToken });
        }
    }
}
