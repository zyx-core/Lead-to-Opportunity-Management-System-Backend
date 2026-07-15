using System.Security.Claims;
using FluentAssertions;
using LeadToOpportunity.API.Controllers;
using LeadToOpportunity.BLL.DTOs.Leads;
using LeadToOpportunity.BLL.Leads;
using LeadToOpportunity.BLL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace LeadToOpportunity.Tests.Controllers;

public class LeadControllerTests
{
    private readonly Mock<ILeadService> _leadServiceMock;
    private readonly LeadController _sut;
    private readonly int _employeeId = 1;

    public LeadControllerTests()
    {
        _leadServiceMock = new Mock<ILeadService>();
        _sut = new LeadController(_leadServiceMock.Object);

        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.NameIdentifier, _employeeId.ToString()),
        }, "mock"));

        _sut.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };
    }

    [Fact]
    public async Task Create_ShouldReturnOk()
    {
        // Arrange
        var request = new CreateLeadRequestDto();
        var response = new LeadResponseDto { Id = 10 };
        _leadServiceMock.Setup(service => service.CreateLeadAsync(request, _employeeId)).ReturnsAsync(response);

        // Act
        var result = await _sut.Create(request);

        // Assert
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult.StatusCode.Should().Be(200);
        okResult.Value.Should().BeEquivalentTo(response);
    }

    [Fact]
    public async Task GetMyLeads_ShouldReturnOk()
    {
        // Arrange
        var pagedResult = new LeadToOpportunity.Shared.Pagination.PagedResult<LeadResponseDto> 
        { 
            Items = new List<LeadResponseDto> { new LeadResponseDto { Id = 1 } },
            TotalCount = 1,
            PageNumber = 1,
            PageSize = 5
        };
        _leadServiceMock.Setup(service => service.GetMyLeadAsync(_employeeId, 1, 5)).ReturnsAsync(pagedResult);

        // Act
        var result = await _sut.GetMyLeads(1, 5);

        // Assert
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult.Value.Should().BeEquivalentTo(pagedResult);
    }

    [Fact]
    public async Task Delete_ShouldReturnNoContent()
    {
        // Arrange
        var leadId = 5;
        _leadServiceMock.Setup(service => service.DeleteLeadAsync(leadId, _employeeId)).Returns(Task.CompletedTask);

        // Act
        var result = await _sut.Delete(leadId);

        // Assert
        var noContentResult = result as NoContentResult;
        noContentResult.Should().NotBeNull();
        noContentResult.StatusCode.Should().Be(204);
    }
}
