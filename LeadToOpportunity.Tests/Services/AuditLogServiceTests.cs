using FluentAssertions;
using LeadToOpportunity.BLL.DTOs.AuditLogs;
using LeadToOpportunity.BLL.Services;
using LeadToOpportunity.DAL.Interfaces;
using LeadToOpportunity.Models.Entities;
using Moq;
using Xunit;

namespace LeadToOpportunity.Tests.Services;

public class AuditLogServiceTests
{
    private readonly Mock<IAuditLogRepository> _auditRepositoryMock;
    private readonly AuditLogService _sut;

    public AuditLogServiceTests()
    {
        _auditRepositoryMock = new Mock<IAuditLogRepository>();
        _sut = new AuditLogService(_auditRepositoryMock.Object);
    }

    [Fact]
    public async Task LogAsync_ShouldAddAuditLogToRepository()
    {
        // Arrange
        var entityType = "Lead";
        var entityId = 1;
        var actorId = 2;
        var action = "Created";
        var fromStatus = "Draft";
        var toStatus = "UnderReview";
        var comment = "Submitted for review";

        _auditRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<AuditLog>())).Returns(Task.CompletedTask);

        // Act
        await _sut.LogAsync(entityType, entityId, actorId, action, fromStatus, toStatus, comment);

        // Assert
        _auditRepositoryMock.Verify(repo => repo.AddAsync(It.Is<AuditLog>(log => 
            log.EntityType == entityType &&
            log.EntityId == entityId &&
            log.ActorId == actorId &&
            log.Action == action &&
            log.FromStatus == fromStatus &&
            log.ToStatus == toStatus &&
            log.Comment == comment
        )), Times.Once);
    }

    [Fact]
    public async Task GetEntityHistoryAsync_ShouldReturnAuditLogResponseDtos()
    {
        // Arrange
        var logs = new List<AuditLog>
        {
            new AuditLog { Id = 1, EntityType = "Lead", EntityId = 1, ActorId = 2, Actor = new User { FirstName = "John", LastName = "Doe" }, Action = "Created", CreatedAt = DateTime.UtcNow },
            new AuditLog { Id = 2, EntityType = "Lead", EntityId = 1, ActorId = 3, Actor = new User { FirstName = "Jane", LastName = "Smith" }, Action = "Approved", CreatedAt = DateTime.UtcNow }
        };

        _auditRepositoryMock.Setup(repo => repo.GetEntityHistoryAsync("Lead", 1)).ReturnsAsync(logs);

        // Act
        var result = await _sut.GetEntityHistoryAsync("Lead", 1);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.First().Action.Should().Be("Created");
        result.Last().Action.Should().Be("Approved");
    }
}
