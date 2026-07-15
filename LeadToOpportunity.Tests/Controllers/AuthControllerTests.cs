using FluentAssertions;
using LeadToOpportunity.API.Controllers;
using LeadToOpportunity.BLL.DTOs.Auth;
using LeadToOpportunity.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace LeadToOpportunity.Tests.Controllers;

public class AuthControllerTests
{
    private readonly Mock<IAuthService> _authServiceMock;
    private readonly AuthController _sut;

    public AuthControllerTests()
    {
        _authServiceMock = new Mock<IAuthService>();
        _sut = new AuthController(_authServiceMock.Object);
    }

    [Fact]
    public async Task Register_WithValidRequest_ShouldReturnOk()
    {
        // Arrange
        var request = new RegisterRequestDto();
        _authServiceMock.Setup(service => service.RegisterAsync(request)).Returns(Task.CompletedTask);

        // Act
        var result = await _sut.Register(request);

        // Assert
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult.StatusCode.Should().Be(200);
    }

    [Fact]
    public async Task Login_WithValidCredentials_ShouldReturnOkWithResponse()
    {
        // Arrange
        var request = new LoginRequestDto();
        var response = new LoginResponseDto { Token = "test_token" };
        _authServiceMock.Setup(service => service.LoginAsync(request)).ReturnsAsync(response);

        // Act
        var result = await _sut.Login(request);

        // Assert
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult.StatusCode.Should().Be(200);
        okResult.Value.Should().BeEquivalentTo(response);
    }
}
