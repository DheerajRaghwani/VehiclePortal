using Microsoft.AspNetCore.Mvc;
using VehiclePortal.Interface;
using VehiclePortal.Models;
using VehiclePortal.Service;

namespace VehiclePortal.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserLoginService _loginService;
        private readonly JwtHelper _jwtHelper;

        public AuthController(IUserLoginService loginService, IConfiguration configuration)
        {
            _loginService = loginService;
            _jwtHelper = new JwtHelper(configuration["Jwt:Key"] ?? "MySuperSecretKeyForJWT123");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var user = await _loginService.ValidateUserAsync(request.UserName, request.Password);
                if (user == null)
                    return Unauthorized("Invalid credentials");

                var token = _jwtHelper.GenerateToken(user);

                return Ok(new
                {
                    token,
                    userId = user.Id,
                    userName = user.UserName,
                    role = user.LoginRole,
                    districtId = user.DistrictId,
                    districtName = user.District?.DistrictName
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message, stack = ex.StackTrace });
            }
        }
    }
}
