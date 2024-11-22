using Microsoft.AspNetCore.Mvc;
using Shopping.Interfaces;
using Shopping.Shared;

namespace Shopping.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : AppControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto  registerDto)
    {
        try
        {
            var result = await _authService.RegisterAsync(registerDto);
            return FromOperationResult(result);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        try
        {
            var result = await _authService.LoginAsync(loginDto);
            return FromOperationResult(result);
            // return Ok(token);           
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}
