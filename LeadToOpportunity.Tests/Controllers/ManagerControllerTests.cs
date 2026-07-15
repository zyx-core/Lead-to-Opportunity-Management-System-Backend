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

public class ManagerControllerTests
{
    private readonly Mock<ILeadService> _leadServiceMock;
    private readonly ManagerController _sut;
    private readonly int _managerId = 2;

    public ManagerControllerTests()
    {
        _leadServiceMock = new Mock<ILeadService>();
        _sut = new ManagerController(_leadServiceMock.Object);

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
    public async Task GetAssignedLeads_ShouldReturnOk()
    {
        // Arrange
        var pagedLeads = new LeadToOpportunity.Shared.Pagination.PagedResult<LeadResponseDto> 
        { 
            Items = new List<LeadResponseDto> { new LeadResponseDto { Id = 1, CompanyName = "Test" } },
            TotalCount = 1,
            PageNumber = 1,
            PageSize = 5
        };
        _leadServiceMock.Setup(s => s.GetManagerLeadsAsync(_managerId, It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(pagedLeads);

        // Act
        var result = await _sut.GetAssignedLeads();

        // Assert
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult.Value.Should().BeEquivalentTo(pagedLeads);
    }

    [Fact]
    public async Task ApproveLead_ShouldReturnNoContent()
    {
        // Arrange
        _leadServiceMock.Setup(s => s.ApproveLeadAsync(1, _managerId)).Returns(Task.CompletedTask);

        // Act
        var result = await _sut.ApproveLead(1);

        // Assert
        var noContentResult = result as NoContentResult;
        noContentResult.Should().NotBeNull();
        noContentResult.StatusCode.Should().Be(204);
    }

    [Fact]
    public async Task RejectLead_ShouldReturnNoContent()
    {
        // Arrange
        var request = new RejectLeadRequestDto { response = "Not a good fit" };
        _leadServiceMock.Setup(s => s.RejectLeadAsync(1, _managerId, request)).Returns(Task.CompletedTask);

        // Act
        var result = await _sut.RejectLead(1, request);

        // Assert
        var noContentResult = result as NoContentResult;
        noContentResult.Should().NotBeNull();
        noContentResult.StatusCode.Should().Be(204);
    }
}
