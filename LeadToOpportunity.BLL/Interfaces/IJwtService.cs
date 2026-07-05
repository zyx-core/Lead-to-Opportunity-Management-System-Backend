using LeadToOpportunity.Models.Entities;

namespace LeadToOpportunity.BLL.interfaces;

public interface IJwtService
{
    string GenerateToken(User user);
}