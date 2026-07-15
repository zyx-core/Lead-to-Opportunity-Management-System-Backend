using FluentAssertions;
using LeadToOpportunity.BLL.DTOs.Leads;
using LeadToOpportunity.BLL.Interfaces;
using LeadToOpportunity.BLL.Services;
using LeadToOpportunity.DAL.Data;
using LeadToOpportunity.DAL.Interfaces;
using LeadToOpportunity.Models.Entities;
using LeadToOpportunity.Models.Enums;
using LeadToOpportunity.Shared.Exception;
using LeadToOpportunity.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace LeadToOpportunity.Tests.Services;

public class LeadServiceTests
{
    private readonly Mock<ILeadRepository> _leadRepositoryMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IOpportunityRepository> _opportunityRepositoryMock;
    private readonly Mock<ILeadReviewRepository> _leadReviewRepositoryMock;
    private readonly Mock<IAuditLogService> _auditLogServiceMock;
    private readonly AppDbContext _context;
    private readonly LeadService _sut;

    public LeadServiceTests()
    {
        _leadRepositoryMock = new Mock<ILeadRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _opportunityRepositoryMock = new Mock<IOpportunityRepository>();
        _leadReviewRepositoryMock = new Mock<ILeadReviewRepository>();
        _auditLogServiceMock = new Mock<IAuditLogService>();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new AppDbContext(options);

        _sut = new LeadService(
            _leadRepositoryMock.Object,
            _userRepositoryMock.Object,
            _opportunityRepositoryMock.Object,
            _leadReviewRepositoryMock.Object,
            _auditLogServiceMock.Object,
            _context
        );
    }

    [Fact]
    public async Task CreateLeadAsync_ShouldAddLeadAndLogAudit()
    {
        // Arrange
        var request = new CreateLeadRequestDto
        {
            CompanyName = "Test Corp",
            ContactName = "John",
            Email = "john@test.com",
            Phone = "12345",
            Requirement = "Software",
            EstimatedValue = 1000,
            Source = "Web",
            Region = "US"
        };
        var employeeId = 1;

        _leadRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Lead>())).Returns(Task.CompletedTask);

        // Act
        var result = await _sut.CreateLeadAsync(request, employeeId);

        // Assert
        result.Should().NotBeNull();
        result.CompanyName.Should().Be("Test Corp");
        
        _leadRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Lead>()), Times.Once);
        _auditLogServiceMock.Verify(a => a.LogAsync("Lead", It.IsAny<int>(), employeeId, "Created", "", "Draft", ""), Times.Once);
    }

    [Fact]
    public async Task GetLeadByIdAsync_NotFound_ShouldThrowException()
    {
        // Arrange
        _leadRepositoryMock.Setup(r => r.GetEmployeeLeadByIdAsync(1, 1)).ReturnsAsync((Lead)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _sut.GetLeadByIdAsync(1, 1));
    }

    [Fact]
    public async Task SubmitLeadAsync_WithValidLeadAndManager_ShouldUpdateStatusToUnderReview()
    {
        // Arrange
        var lead = new Lead { Id = 1, Status = LeadStatus.Draft };
        var manager = new User { Id = 2, Role = UserRole.Manager };
        var request = new SubmitLeadRequestDto { ManagerId = 2 };

        _leadRepositoryMock.Setup(r => r.GetEmployeeLeadByIdAsync(1, 1)).ReturnsAsync(lead);
        _userRepositoryMock.Setup(r => r.GetByIdAsync(2)).ReturnsAsync(manager);

        // Act
        await _sut.SubmitLeadAsync(1, 1, request);

        // Assert
        lead.Status.Should().Be(LeadStatus.UnderReview);
        lead.AssignedManagerId.Should().Be(2);
        _leadRepositoryMock.Verify(r => r.UpdateAsync(lead), Times.Exactly(2)); // Updates Submitted then UnderReview
    }

    [Fact]
    public async Task DeleteLeadAsync_NotDraft_ShouldThrowBadRequestException()
    {
        // Arrange
        var lead = new Lead { Id = 1, Status = LeadStatus.UnderReview };
        _leadRepositoryMock.Setup(r => r.GetEmployeeLeadByIdAsync(1, 1)).ReturnsAsync(lead);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<BadRequestException>(() => _sut.DeleteLeadAsync(1, 1));
        exception.Message.Should().Contain("only draft lead can be deleted");
    }

    [Fact]
    public async Task AssignManagerAsync_ValidManager_ShouldReassignLead()
    {
        // Arrange
        var lead = new Lead { Id = 1, AssignedManagerId = 2 };
        var newManager = new User { Id = 3, Role = UserRole.Manager, FirstName = "New", LastName = "Manager" };
        var request = new AssignManagerRequestDto { ManagerId = 3 };

        _leadRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(lead);
        _userRepositoryMock.Setup(r => r.GetByIdAsync(3)).ReturnsAsync(newManager);

        // Act
        await _sut.AssignManagerAsync(1, 99, request); // adminId 99

        // Assert
        lead.AssignedManagerId.Should().Be(3);
        _leadRepositoryMock.Verify(r => r.UpdateAsync(lead), Times.Once);
    }
}
