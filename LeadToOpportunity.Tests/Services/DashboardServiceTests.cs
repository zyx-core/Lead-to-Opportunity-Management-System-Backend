using FluentAssertions;
using LeadToOpportunity.BLL.Services;
using LeadToOpportunity.DAL.Interfaces;
using LeadToOpportunity.Models.Entities;
using LeadToOpportunity.Models.Enums;
using Moq;
using Xunit;

namespace LeadToOpportunity.Tests.Services;

public class DashboardServiceTests
{
    private readonly Mock<ILeadRepository> _leadRepositoryMock;
    private readonly Mock<IOpportunityRepository> _opportunityRepositoryMock;
    private readonly DashboardService _sut;

    public DashboardServiceTests()
    {
        _leadRepositoryMock = new Mock<ILeadRepository>();
        _opportunityRepositoryMock = new Mock<IOpportunityRepository>();
        _sut = new DashboardService(_leadRepositoryMock.Object, _opportunityRepositoryMock.Object);
    }

    [Fact]
    public async Task GetEmployeeDashboardAsync_ShouldReturnAggregatedMetrics()
    {
        // Arrange
        var employeeId = 1;
        _leadRepositoryMock.Setup(r => r.CountEmployeeLeadsByStatusAsync(employeeId, LeadStatus.Draft)).ReturnsAsync(5);
        _leadRepositoryMock.Setup(r => r.CountEmployeeLeadsByStatusAsync(employeeId, LeadStatus.UnderReview)).ReturnsAsync(3);
        _leadRepositoryMock.Setup(r => r.CountEmployeeLeadsByStatusAsync(employeeId, LeadStatus.Approved)).ReturnsAsync(2);
        _leadRepositoryMock.Setup(r => r.CountEmployeeLeadsByStatusAsync(employeeId, LeadStatus.Rejected)).ReturnsAsync(1);
        _leadRepositoryMock.Setup(r => r.CountEmployeeLeadsByStatusAsync(employeeId, LeadStatus.ModificationRequested)).ReturnsAsync(4);

        // Act
        var result = await _sut.GetEmployeeDashboardAsync(employeeId);

        // Assert
        result.Should().NotBeNull();
        result.DraftLeads.Should().Be(5);
        result.UnderReviewLeads.Should().Be(3);
        result.ApprovedLeads.Should().Be(2);
        result.RejectedLeads.Should().Be(1);
        result.ModificationRequestedLeads.Should().Be(4);
        result.TotalLeads.Should().Be(15);
    }

    [Fact]
    public async Task GetManagerWorkloadAsync_ShouldReturnGroupedLeads()
    {
        // Arrange
        var leads = new List<Lead>
        {
            new Lead { Id = 1, Status = LeadStatus.UnderReview, AssignedManagerId = 10, AssignedManager = new User { FirstName = "John", LastName = "Doe" } },
            new Lead { Id = 2, Status = LeadStatus.UnderReview, AssignedManagerId = 10, AssignedManager = new User { FirstName = "John", LastName = "Doe" } },
            new Lead { Id = 3, Status = LeadStatus.UnderReview, AssignedManagerId = 11, AssignedManager = new User { FirstName = "Jane", LastName = "Smith" } },
            new Lead { Id = 4, Status = LeadStatus.Approved, AssignedManagerId = 10, AssignedManager = new User { FirstName = "John", LastName = "Doe" } } // Should be ignored
        };

        _leadRepositoryMock.Setup(r => r.GetAllWithManagerAsync()).ReturnsAsync(leads);

        // Act
        var result = await _sut.GetManagerWorkloadAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        
        var first = result.First(); // Should be ordered by PendingReviews desc
        first.ManagerId.Should().Be(10);
        first.ManagerName.Should().Be("John Doe");
        first.PendingReviews.Should().Be(2);

        var second = result.Last();
        second.ManagerId.Should().Be(11);
        second.PendingReviews.Should().Be(1);
    }
}
