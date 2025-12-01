using CustomerCrudApi.Dtos;
using CustomerCrudApi.Models;
using CustomerCrudApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Security.Claims;

namespace CustomerCrudApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IJwtService _jwtService;

        public AuthController(IAuthService authService, IJwtService jwtService)
        {
            _authService = authService;
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<CustomerResponseDto>> Register([FromBody] CustomerRequestDto requestDto)
        {
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _authService.RegisterCustomerAsync(requestDto);

            if (response == null)
            {
                return BadRequest();
            }


            return Created(String.Empty, response);
        }

        [HttpPost("login")]
        public async Task<ActionResult<JwtResponseDto>> Login([FromBody] CustomerLoginRequestDto loginRequestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var IsValid = await _authService.ValidateLogin(loginRequestDto);
            if (!IsValid)
            {
                return Unauthorized();
            }

            var jwtToken = _jwtService.GenerateToken(new[]
                {
                    new Claim(ClaimTypes.Name, loginRequestDto.Email!),
                    new Claim(ClaimTypes.Role, "USER")
                });


            return Ok(new JwtResponseDto() { JwtToken = jwtToken});
            
        }
    }
}
