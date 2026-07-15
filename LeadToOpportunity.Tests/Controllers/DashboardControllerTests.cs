using System.Security.Claims;
using FluentAssertions;
using LeadToOpportunity.API.Controllers;
using LeadToOpportunity.BLL.DTOs.Dashboard;
using LeadToOpportunity.BLL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace LeadToOpportunity.Tests.Controllers;

public class DashboardControllerTests
{
    private readonly Mock<IDashboardService> _dashboardServiceMock;
    private readonly DashboardController _sut;
    private readonly int _userId = 1;

    public DashboardControllerTests()
    {
        _dashboardServiceMock = new Mock<IDashboardService>();
        _sut = new DashboardController(_dashboardServiceMock.Object);

        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.NameIdentifier, _userId.ToString()),
        }, "mock"));

        _sut.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };
    }

    [Fact]
    public async Task GetEmployeeDashboard_ShouldReturnOkWithData()
    {
        // Arrange
        var dashboardData = new EmployeeDashboardDto { DraftLeads = 5 };
        _dashboardServiceMock.Setup(s => s.GetEmployeeDashboardAsync(_userId)).ReturnsAsync(dashboardData);

        // Act
        var result = await _sut.GetEmployeeDashboard();

        // Assert
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult.StatusCode.Should().Be(200);
        okResult.Value.Should().BeEquivalentTo(dashboardData);
    }

    [Fact]
    public async Task GetManagerDashboard_ShouldReturnOkWithData()
    {
        // Arrange
        var dashboardData = new ManagerDashboardDto { PendingReviews = 2 };
        _dashboardServiceMock.Setup(s => s.GetManagerDashboardAsync(_userId)).ReturnsAsync(dashboardData);

        // Act
        var result = await _sut.GetManagerDashboard();

        // Assert
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult.Value.Should().BeEquivalentTo(dashboardData);
    }
}
