using Microsoft.AspNetCore.Mvc;
using RachaStats.Application.Auth;
using RachaStats.Application.Auth.Requests;

namespace RachaStats.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _service;

    public AuthController(IAuthService service)
    {
        _service = service;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var response = await _service.LoginAsync(request);
        return Ok(response);
    }
    
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokensRequest request)
    {
        var response = await _service.RefreshTokenAsync(request);
        
        return Ok(response);
    }
}
