using FluentAssertions;
using LeadToOpportunity.BLL.DTOs.User;
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

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IPasswordService> _passwordServiceMock;
    private readonly UserService _sut;

    public UserServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _passwordServiceMock = new Mock<IPasswordService>();
        _sut = new UserService(_userRepositoryMock.Object, _passwordServiceMock.Object);
    }

    [Fact]
    public async Task GetManagersAsync_ShouldReturnManagerDtos()
    {
        // Arrange
        var managers = new List<User>
        {
            new User { Id = 1, FirstName = "Alice", LastName = "Smith", Email = "alice@test.com", Role = UserRole.Manager },
            new User { Id = 2, FirstName = "Bob", LastName = "Jones", Email = "bob@test.com", Role = UserRole.Manager }
        };

        _userRepositoryMock.Setup(repo => repo.GetManagersAsync()).ReturnsAsync(managers);

        // Act
        var result = await _sut.GetManagersAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.First().FullName.Should().Be("Alice Smith");
    }

    [Fact]
    public async Task GetUserByIdAsync_WithValidId_ShouldReturnUserResponseDto()
    {
        // Arrange
        var user = new User { Id = 1, FirstName = "Alice", LastName = "Smith", Email = "alice@test.com", Role = UserRole.Employee };
        _userRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(user);

        // Act
        var result = await _sut.GetUserByIdAsync(1);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(1);
        result.FirstName.Should().Be("Alice");
    }

    [Fact]
    public async Task GetUserByIdAsync_WithInvalidId_ShouldThrowNotFoundException()
    {
        // Arrange
        _userRepositoryMock.Setup(repo => repo.GetByIdAsync(99)).ReturnsAsync((User)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _sut.GetUserByIdAsync(99));
    }

    [Fact]
    public async Task CreateUserAsync_WithExistingEmail_ShouldThrowConflictException()
    {
        // Arrange
        var request = new CreateUserRequestDto { Email = "test@example.com" };
        _userRepositoryMock.Setup(repo => repo.GetByEmailAsync("test@example.com")).ReturnsAsync(new User());

        // Act & Assert
        await Assert.ThrowsAsync<ConflictException>(() => _sut.CreateUserAsync(request));
    }

    [Fact]
    public async Task CreateUserAsync_WithValidRequest_ShouldReturnUserResponseDto()
    {
        // Arrange
        var request = new CreateUserRequestDto 
        { 
            FirstName = "New", 
            LastName = "User", 
            Email = "new@example.com", 
            Password = "Password123!",
            Role = UserRole.Employee,
            Region = "US"
        };

        _userRepositoryMock.Setup(repo => repo.GetByEmailAsync(request.Email)).ReturnsAsync((User)null);
        _passwordServiceMock.Setup(ps => ps.HashPassword(It.IsAny<User>(), request.Password)).Returns("hashedpassword");

        // Act
        var result = await _sut.CreateUserAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.FirstName.Should().Be("New");
        result.Email.Should().Be("new@example.com");
        _userRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<User>()), Times.Once);
    }
}
