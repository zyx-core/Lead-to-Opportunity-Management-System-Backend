using FluentAssertions;
using LeadToOpportunity.BLL.DTOs.Auth;
using LeadToOpportunity.BLL.Interfaces;
using LeadToOpportunity.BLL.interfaces;
using LeadToOpportunity.BLL.Services;
using LeadToOpportunity.DAL.Interfaces;
using LeadToOpportunity.Models.Entities;
using LeadToOpportunity.Models.Enums;
using LeadToOpportunity.Shared.Exception;
using LeadToOpportunity.Shared.Exceptions;
using Moq;
using Xunit;

namespace LeadToOpportunity.Tests.Services;

public class AuthServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IPasswordService> _passwordServiceMock;
    private readonly Mock<IJwtService> _jwtServiceMock;
    private readonly AuthService _sut;

    public AuthServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _passwordServiceMock = new Mock<IPasswordService>();
        _jwtServiceMock = new Mock<IJwtService>();

        _sut = new AuthService(
            _userRepositoryMock.Object,
            _passwordServiceMock.Object,
            _jwtServiceMock.Object
        );
    }

    [Fact]
    public async Task RegisterAsync_WithExistingEmail_ShouldThrowConflictException()
    {
        // Arrange
        var request = new RegisterRequestDto { Email = "test@example.com" };
        var existingUser = new User();

        _userRepositoryMock.Setup(repo => repo.GetByEmailAsync(request.Email))
            .ReturnsAsync(existingUser);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ConflictException>(() => _sut.RegisterAsync(request));
        exception.Message.Should().Be("user already exist");
    }

    [Fact]
    public async Task LoginAsync_WithValidCredentials_ShouldReturnLoginResponse()
    {
        // Arrange
        var request = new LoginRequestDto { Email = "test@example.com", Password = "Password123!" };
        var user = new User { Email = "test@example.com", Role = UserRole.Employee, PasswordHash = "hash" };
        var token = "jwt_token_string";

        _userRepositoryMock.Setup(repo => repo.GetByEmailAsync(request.Email))
            .ReturnsAsync(user);

        _passwordServiceMock.Setup(service => service.VerifyPassword(user, request.Password, user.PasswordHash))
            .Returns(true);

        _jwtServiceMock.Setup(service => service.GenerateToken(user))
            .Returns(token);

        // Act
        var result = await _sut.LoginAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.Token.Should().Be(token);
        result.Email.Should().Be(user.Email);
        result.Role.Should().Be(UserRole.Employee.ToString());
    }

    [Fact]
    public async Task LoginAsync_WithInvalidPassword_ShouldThrowUnauthorizedException()
    {
        // Arrange
        var request = new LoginRequestDto { Email = "test@example.com", Password = "WrongPassword!" };
        var user = new User { Email = "test@example.com", PasswordHash = "hash" };

        _userRepositoryMock.Setup(repo => repo.GetByEmailAsync(request.Email))
            .ReturnsAsync(user);

        _passwordServiceMock.Setup(service => service.VerifyPassword(user, request.Password, user.PasswordHash))
            .Returns(false);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<UnauthorizedException>(() => _sut.LoginAsync(request));
        exception.Message.Should().Be("email or password is worng");
    }
}
