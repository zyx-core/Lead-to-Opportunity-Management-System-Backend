using FluentAssertions;
using LeadToOpportunity.BLL.DTOs.Opportunities;
using LeadToOpportunity.BLL.Interfaces;
using LeadToOpportunity.BLL.Services;
using LeadToOpportunity.DAL.Interfaces;
using LeadToOpportunity.Models.Entities;
using LeadToOpportunity.Models.Enums;
using Moq;
using Xunit;

namespace LeadToOpportunity.Tests.Services;

public class OpportunityServiceTests
{
    private readonly Mock<IOpportunityRepository> _opportunityRepositoryMock;
    private readonly Mock<IAuditLogService> _auditLogServiceMock;
    private readonly OpportunityService _sut;

    public OpportunityServiceTests()
    {
        _opportunityRepositoryMock = new Mock<IOpportunityRepository>();
        _auditLogServiceMock = new Mock<IAuditLogService>();
        _sut = new OpportunityService(_opportunityRepositoryMock.Object, _auditLogServiceMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnOpportunityResponseDtos()
    {
        // Arrange
        var opportunities = new List<Opportunity>
        {
            new Opportunity { Id = 1, LeadId = 1, Lead = new Lead { CompanyName = "Acme Corp" }, Stage = OpportunityStage.Qualification, EstimatedValue = 1000 },
            new Opportunity { Id = 2, LeadId = 2, Lead = new Lead { CompanyName = "Global Tech" }, Stage = OpportunityStage.Proposal, EstimatedValue = 5000 }
        };

        _opportunityRepositoryMock.Setup(repo => repo.GetAllWithLeadAsync()).ReturnsAsync(opportunities);

        // Act
        var result = await _sut.GetAllAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.First().CompanyName.Should().Be("Acme Corp");
    }

    [Fact]
    public async Task UpdateStageAsync_WithValidOpportunity_ShouldUpdateStageAndLogAudit()
    {
        // Arrange
        var opportunity = new Opportunity { Id = 1, Stage = OpportunityStage.Qualification };
        var request = new UpdateOpportunityStageDto { Stage = OpportunityStage.Proposal };
        var managerId = 10;

        _opportunityRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(opportunity);

        // Act
        await _sut.UpdateStageAsync(1, managerId, request);

        // Assert
        opportunity.Stage.Should().Be(OpportunityStage.Proposal);
        _opportunityRepositoryMock.Verify(repo => repo.UpdateAsync(opportunity), Times.Once);
        _auditLogServiceMock.Verify(audit => audit.LogAsync(
            "Opportunity", 1, managerId, "Stage Updated", "Qualification", "Proposal", ""), Times.Once);
    }

    [Fact]
    public async Task MarkWonAsync_AlreadyClosed_ShouldThrowException()
    {
        // Arrange
        var opportunity = new Opportunity { Id = 1, Stage = OpportunityStage.Won };
        _opportunityRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(opportunity);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _sut.MarkWonAsync(1, 10));
        exception.Message.Should().Contain("already Won");
    }

    [Fact]
    public async Task MarkLostAsync_ValidOpportunity_ShouldSetStageToLost()
    {
        // Arrange
        var opportunity = new Opportunity { Id = 1, Stage = OpportunityStage.Negotiation };
        var managerId = 10;

        _opportunityRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(opportunity);

        // Act
        await _sut.MarkLostAsync(1, managerId);

        // Assert
        opportunity.Stage.Should().Be(OpportunityStage.Lost);
        _opportunityRepositoryMock.Verify(repo => repo.UpdateAsync(opportunity), Times.Once);
        _auditLogServiceMock.Verify(audit => audit.LogAsync(
            "Opportunity", 1, managerId, "Stage Updated", "Negotiation", "Lost", ""), Times.Once);
    }
}
