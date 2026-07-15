using System.Security.Claims;
using LeadToOpportunity.BLL.DTOs.Leads;
using LeadToOpportunity.BLL.DTOs.User;
using LeadToOpportunity.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LeadToOpportunity.API.Controllers;

[ApiController]
[Route("api/admin")]
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILeadService _leadService;

    public AdminController(IUserService userService,ILeadService leadService)
    {
        _userService = userService;
        _leadService = leadService;
    }

    private int GetAdminId()
{
    return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
}
[HttpGet("users")]
public async Task<IActionResult> GetAllUsers([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 5)
{
    var users = await _userService.GetAllUsersAsync(pageNumber, pageSize);

    return Ok(users);
}

[HttpGet("users/{id}")]
public async Task<IActionResult> GetUserById(int id)
{
    var user = await _userService.GetUserByIdAsync(id);

    return Ok(user);
}

[HttpPost("users")]
public async Task<IActionResult> CreateUser([FromBody]
    CreateUserRequestDto request)
{
    var user = await _userService.CreateUserAsync(request);

    return CreatedAtAction(
        nameof(GetUserById),
        new { id = user.Id },
        user);
}
[HttpPut("users/{id}")]
public async Task<IActionResult> UpdateUser(
    int id,[FromBody]
    UpdateUserRequestDto request)
{
    await _userService.UpdateUserAsync(id, request);

    return NoContent();
}

[HttpPut("dashboard/assign/manager")]
public async Task<IActionResult> AssignManager(
    [FromBody] AssignManagerRequestDto request)
{
    await _leadService.AssignManagerAsync(GetAdminId(), request);

    return NoContent();
}

[HttpGet("pipeline")]
public async Task<IActionResult> GetPipeline([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 5)
{
    var pipeline = await _leadService.GetPipelineAsync(pageNumber, pageSize);

    return Ok(pipeline);
}
}