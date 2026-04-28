using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.Auth;
using TaskManager.Application.Auth.Requests;

namespace TaskManager.Controllers;

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
    public IActionResult Login([FromBody] LoginRequest request)
    {
        var response = _service.Login(request);
        return Ok(response);
    }
    
    [HttpPost("refresh")]
    public IActionResult Refresh([FromBody] RefreshTokensRequest request)
    {
        var response = _service.RefreshToken(request);
        
        return Ok(response);
    }}