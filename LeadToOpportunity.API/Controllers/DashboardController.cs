using System.Security.Claims;
using LeadToOpportunity.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LeadToOpportunity.API.Controllers;
[ApiController]
[Route("api/dashboard")]
[Authorize]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _dashboardService;

    public DashboardController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    private int GetUserId()
    {
        return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
    }

    [HttpGet("employee")]
    [Authorize(Roles =  "Employee")]
    public async Task<IActionResult> GetEmployeeDashboard()
    {
        var dashboard = await  _dashboardService.GetEmployeeDashboardAsync(GetUserId());

        return Ok(dashboard);
    }

    [HttpGet("manager")]
[Authorize(Roles = "Manager")]
public async Task<IActionResult> GetManagerDashboard()
{
    var dashboard = await _dashboardService.GetManagerDashboardAsync(GetUserId());
    return Ok(dashboard);
}
}