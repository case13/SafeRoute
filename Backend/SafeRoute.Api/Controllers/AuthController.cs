using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SafeRoute.Application.Services.Implementations;
using SafeRoute.Application.Services.Interfaces;
using SafeRoute.Shared.Dtos.Auth;
using System.Security.Claims;

namespace SafeRoute.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;
        public AuthController(IAuthService service)
        {
            _service = service;
        }

        [Authorize]
        [HttpPut("change-password")]
        public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordRequestDto dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdClaim, out var userId) || userId <= 0)
                return Unauthorized("Usuário inválido.");

            try
            {
                await _service.ChangePasswordAsync(userId, dto);
                return NoContent();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("set-password")]
        public async Task<IActionResult> SetPassword([FromBody] SetPasswordDto dto)
        {
            try
            {
                await _service.SetPasswordAsync(dto);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<LoginResultDto>> Login(
            [FromBody] LoginRequestDto dto)
        {
            try
            {
                var result = await _service.LoginAsync(dto);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("refresh")]
        public async Task<ActionResult<LoginResultDto>> Refresh(
            [FromBody] RefreshTokenDto dto)
        {
            try
            {
                var result = await _service.RefreshAsync(dto.RefreshToken);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }


    }
}
