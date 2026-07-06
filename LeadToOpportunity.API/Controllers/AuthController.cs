using System.Runtime.CompilerServices;
using LeadToOpportunity.BLL.DTOs.Auth;
using LeadToOpportunity.BLL.Interfaces;
using LeadToOpportunity.BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace LeadToOpportunity.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]

    public async Task<IActionResult> Register(RegisterRequestDto request)
    {
        await _authService.RegisterAsync(request);

        return Ok(new
        {
            message= "User registered successfully."
        }
        );
    }

     [HttpPost("login")]
     public async Task<IActionResult> Login (LoginRequestDto request)
    {
        var response = await _authService.LoginAsync(request);

        return Ok(response);
    }
}