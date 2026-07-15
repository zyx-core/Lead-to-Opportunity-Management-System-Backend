using System.Security.Claims;
using FluentAssertions;
using LeadToOpportunity.API.Controllers;
using LeadToOpportunity.BLL.DTOs.Opportunities;
using LeadToOpportunity.BLL.DTOs;
using LeadToOpportunity.BLL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace LeadToOpportunity.Tests.Controllers;

public class OpportunityControllerTests
{
    private readonly Mock<IOpportunityService> _opportunityServiceMock;
    private readonly OpportunityController _sut;
    private readonly int _managerId = 2;

    public OpportunityControllerTests()
    {
        _opportunityServiceMock = new Mock<IOpportunityService>();
        _sut = new OpportunityController(_opportunityServiceMock.Object);

        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.NameIdentifier, _managerId.ToString()),
        }, "mock"));

        _sut.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };
    }

    [Fact]
    public async Task GetAll_ShouldReturnOk()
    {
        // Arrange
        var opportunities = new List<OpportunityResponseDto> { new OpportunityResponseDto { id = 1 } };
        _opportunityServiceMock.Setup(service => service.GetAllAsync()).ReturnsAsync(opportunities);

        // Act
        var result = await _sut.GetAll();

        // Assert
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult.StatusCode.Should().Be(200);
        okResult.Value.Should().BeEquivalentTo(opportunities);
    }

    [Fact]
    public async Task GetById_WithValidId_ShouldReturnOk()
    {
        // Arrange
        var opportunity = new OpportunityResponseDto { id = 1 };
        _opportunityServiceMock.Setup(service => service.GetByIdAsync(1)).ReturnsAsync(opportunity);

        // Act
        var result = await _sut.GetById(1);

        // Assert
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult.Value.Should().BeEquivalentTo(opportunity);
    }

    [Fact]
    public async Task GetById_WithInvalidId_ShouldReturnNotFound()
    {
        // Arrange
        _opportunityServiceMock.Setup(service => service.GetByIdAsync(99)).ReturnsAsync((OpportunityResponseDto)null);

        // Act
        var result = await _sut.GetById(99);

        // Assert
        var notFoundResult = result as NotFoundResult;
        notFoundResult.Should().NotBeNull();
        notFoundResult.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task MarkWon_ShouldReturnNoContent()
    {
        // Arrange
        _opportunityServiceMock.Setup(service => service.MarkWonAsync(1, _managerId)).Returns(Task.CompletedTask);

        // Act
        var result = await _sut.MarkWon(1);

        // Assert
        var noContentResult = result as NoContentResult;
        noContentResult.Should().NotBeNull();
        noContentResult.StatusCode.Should().Be(204);
    }
}
