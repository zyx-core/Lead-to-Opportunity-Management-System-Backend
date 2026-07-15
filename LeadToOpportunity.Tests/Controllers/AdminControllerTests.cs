using System.Security.Claims;
using FluentAssertions;
using LeadToOpportunity.API.Controllers;
using LeadToOpportunity.BLL.DTOs.Leads;
using LeadToOpportunity.BLL.DTOs.User;
using LeadToOpportunity.BLL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace LeadToOpportunity.Tests.Controllers;

public class AdminControllerTests
{
    private readonly Mock<IUserService> _userServiceMock;
    private readonly Mock<ILeadService> _leadServiceMock;
    private readonly AdminController _sut;
    private readonly int _adminId = 1;

    public AdminControllerTests()
    {
        _userServiceMock = new Mock<IUserService>();
        _leadServiceMock = new Mock<ILeadService>();
        _sut = new AdminController(_userServiceMock.Object, _leadServiceMock.Object);

        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.NameIdentifier, _adminId.ToString()),
        }, "mock"));

        _sut.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };
    }

    [Fact]
    public async Task GetAllUsers_ShouldReturnOk()
    {
        // Arrange
        var pagedUsers = new LeadToOpportunity.Shared.Pagination.PagedResult<UserResponseDto> 
        { 
            Items = new List<UserResponseDto> { new UserResponseDto { Id = 1 } },
            TotalCount = 1,
            PageNumber = 1,
            PageSize = 5
        };
        _userServiceMock.Setup(s => s.GetAllUsersAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(pagedUsers);

        // Act
        var result = await _sut.GetAllUsers();

        // Assert
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult.Value.Should().BeEquivalentTo(pagedUsers);
    }

    [Fact]
    public async Task CreateUser_ShouldReturnCreatedAtAction()
    {
        // Arrange
        var request = new CreateUserRequestDto();
        var response = new UserResponseDto { Id = 10 };
        _userServiceMock.Setup(s => s.CreateUserAsync(request)).ReturnsAsync(response);

        // Act
        var result = await _sut.CreateUser(request);

        // Assert
        var createdResult = result as CreatedAtActionResult;
        createdResult.Should().NotBeNull();
        createdResult.StatusCode.Should().Be(201);
        createdResult.RouteValues["id"].Should().Be(10);
        createdResult.Value.Should().BeEquivalentTo(response);
    }

    [Fact]
    public async Task AssignManager_ShouldReturnNoContent()
    {
        // Arrange
        var request = new AssignManagerRequestDto { ManagerId = 2 };
        _leadServiceMock.Setup(s => s.AssignManagerAsync(5, _adminId, request)).Returns(Task.CompletedTask);

        // Act
        var result = await _sut.AssignManager(5, request);

        // Assert
        var noContentResult = result as NoContentResult;
        noContentResult.Should().NotBeNull();
        noContentResult.StatusCode.Should().Be(204);
    }
}
