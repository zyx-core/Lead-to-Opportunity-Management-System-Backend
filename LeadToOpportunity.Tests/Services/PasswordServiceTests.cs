using FluentAssertions;
using LeadToOpportunity.BLL.Services;
using LeadToOpportunity.Models.Entities;
using Xunit;

namespace LeadToOpportunity.Tests.Services;

public class PasswordServiceTests
{
    private readonly PasswordService _sut; // System Under Test

    public PasswordServiceTests()
    {
        _sut = new PasswordService();
    }

    [Fact]
    public void HashPassword_ShouldReturnNonEmptyHash()
    {
        // Arrange
        var user = new User();
        var password = "SecurePassword123!";

        // Act
        var hash = _sut.HashPassword(user, password);

        // Assert
        hash.Should().NotBeNullOrWhiteSpace();
        hash.Should().NotBe(password);
    }

    [Fact]
    public void VerifyPassword_WithValidPassword_ShouldReturnTrue()
    {
        // Arrange
        var user = new User();
        var password = "SecurePassword123!";
        var hash = _sut.HashPassword(user, password);

        // Act
        var result = _sut.VerifyPassword(user, password, hash);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void VerifyPassword_WithInvalidPassword_ShouldReturnFalse()
    {
        // Arrange
        var user = new User();
        var password = "SecurePassword123!";
        var invalidPassword = "WrongPassword!";
        var hash = _sut.HashPassword(user, password);

        // Act
        var result = _sut.VerifyPassword(user, invalidPassword, hash);

        // Assert
        result.Should().BeFalse();
    }
}
