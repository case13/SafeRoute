using Microsoft.AspNetCore.Mvc;
using SafeRoute.Application.Services.Interfaces;
using SafeRoute.Domain.Entities;
using SafeRoute.Shared.Enums.Tipos;

[ApiController]
[Route("api/jwt-test")]
public class JwtTestController : ControllerBase
{
    private readonly ITokenService _tokenService;

    public JwtTestController(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    [HttpGet("generate")]
    public IActionResult Generate()
    {
        var fakeUser = new User
        {
            Name = "Teste JWT",
            Email = "teste@saferoute.dev",
            CompanyId = 1,
            UserType = UserTypeEnum.Administrador
        };

        var token = _tokenService.GenerateAccessToken(fakeUser);

        return Ok(token);
    }
}
