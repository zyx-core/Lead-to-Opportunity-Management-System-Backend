using LeadToOpportunity.Models.Enums;

namespace LeadToOpportunity.BLL.DTOs.User;

public class CreateUserRequestDto
{
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public UserRole Role { get; set; }

    public string Region { get; set; } = string.Empty;
}