using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechnicalTest.Application.DTOs;
using TechnicalTest.Application.Interfaces.Repositories;
namespace TechnicalTest.Api.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IIdentityService _identityService;

        public UserController(IIdentityService identityService)
        {
            this._identityService = identityService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] TokenRequest tokenRequest)
        {
            var ipAddress = GenerateIPAddress();
            var token = await _identityService.GetTokenAsync(tokenRequest, ipAddress);
            return Ok(token);
        }

        [AllowAnonymous]
        [HttpGet("login")]
        public IActionResult Login()
        {
            var ipAddress = GenerateIPAddress();
            return Ok(ipAddress);
        }

        private string GenerateIPAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
            {
                return Request.Headers["X-Forwarded-For"];
            }
            else
            {
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            }
        }
    }
}
