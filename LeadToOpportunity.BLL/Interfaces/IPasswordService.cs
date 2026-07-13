using LeadToOpportunity.Models.Entities;

namespace LeadToOpportunity.BLL.interfaces;

public interface IPasswordService
{
    string HashPassword(User user, string password);
    bool VerifyPassword(User user, string password, string passwordHash);
}