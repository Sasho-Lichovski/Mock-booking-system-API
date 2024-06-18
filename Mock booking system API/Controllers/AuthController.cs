using Microsoft.AspNetCore.Mvc;
using Services.Models.Auth;
using Services.Services;

namespace Mock_booking_system_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly TokenService tokenService;

        public AuthController(TokenService tokenService)
        {
            this.tokenService = tokenService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel model)
        {

            if (model.UserName == "some-user" && model.Password == "some-password")
            {
                var token = tokenService.GenerateToken(model.UserName);
                return Ok(new { Token = token });
            }

            return Unauthorized();
        }
    }
}
