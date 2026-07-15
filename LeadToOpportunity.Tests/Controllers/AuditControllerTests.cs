using FluentAssertions;
using LeadToOpportunity.API.Controllers;
using LeadToOpportunity.BLL.DTOs.AuditLogs;
using LeadToOpportunity.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace LeadToOpportunity.Tests.Controllers;

public class AuditControllerTests
{
    private readonly Mock<IAuditLogService> _auditLogServiceMock;
    private readonly AuditController _sut;

    public AuditControllerTests()
    {
        _auditLogServiceMock = new Mock<IAuditLogService>();
        _sut = new AuditController(_auditLogServiceMock.Object);
    }

    [Fact]
    public async Task GetHistory_ShouldReturnOk()
    {
        // Arrange
        var history = new List<AuditLogResponseDto> { new AuditLogResponseDto { Action = "Created" } };
        _auditLogServiceMock.Setup(s => s.GetEntityHistoryAsync("Lead", 1)).ReturnsAsync(history);

        // Act
        var result = await _sut.GetHistory("Lead", 1);

        // Assert
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult.Value.Should().BeEquivalentTo(history);
    }
}
