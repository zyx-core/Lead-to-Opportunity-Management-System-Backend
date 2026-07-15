using System.IdentityModel.Tokens.Jwt;
using FluentAssertions;
using LeadToOpportunity.BLL.Services;
using LeadToOpportunity.BLL.Settings;
using LeadToOpportunity.Models.Entities;
using LeadToOpportunity.Models.Enums;
using Microsoft.Extensions.Options;
using Xunit;

namespace LeadToOpportunity.Tests.Services;

public class JwtServiceTests
{
    private readonly JwtService _sut;

    public JwtServiceTests()
    {
        var jwtSettings = new JwtSetting
        {
            Key = "SuperSecretKeyThatIsLongEnoughToGenerateAValidTokenForTestingPurpose!!",
            Issuer = "TestIssuer",
            Audience = "TestAudience",
            DurationInMinutes = 60
        };

        var options = Options.Create(jwtSettings);
        _sut = new JwtService(options);
    }

    [Fact]
    public void GenerateToken_ShouldReturnValidJwtString()
    {
        // Arrange
        var user = new User
        {
            Id = 1,
            Email = "test@example.com",
            Role = UserRole.Employee
        };

        // Act
        var token = _sut.GenerateToken(user);

        // Assert
        token.Should().NotBeNullOrWhiteSpace();

        // Optional: verify it is a valid JWT by parsing it
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);
        
        jwtToken.Issuer.Should().Be("TestIssuer");
        jwtToken.Audiences.Should().Contain("TestAudience");
    }
}
