using FluentAssertions;
using LeadToOpportunity.API.Controllers;
using LeadToOpportunity.BLL.DTOs.User;
using LeadToOpportunity.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace LeadToOpportunity.Tests.Controllers;

public class UserControllerTests
{
    private readonly Mock<IUserService> _userServiceMock;
    private readonly UserController _sut;

    public UserControllerTests()
    {
        _userServiceMock = new Mock<IUserService>();
        _sut = new UserController(_userServiceMock.Object);
    }

    [Fact]
    public async Task GetManagers_ShouldReturnOk()
    {
        // Arrange
        var managers = new List<ManagerDto> { new ManagerDto { Id = 1 } };
        _userServiceMock.Setup(s => s.GetManagersAsync()).ReturnsAsync(managers);

        // Act
        var result = await _sut.GetManagers();

        // Assert
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult.Value.Should().BeEquivalentTo(managers);
    }
}
