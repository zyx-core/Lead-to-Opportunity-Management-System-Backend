using Microsoft.AspNetCore.Identity;
using LeadToOpportunity.BLL.interfaces;
using LeadToOpportunity.Models.Entities;

namespace LeadToOpportunity.BLL.Services;

public class PasswordService : IPasswordService
{
    private readonly PasswordHasher<User> _passwordHasher;

    public PasswordService()
    {
        
        _passwordHasher = new PasswordHasher<User>(); 
    }

    public string HashPassword(User user, string password)
    {
        return _passwordHasher.HashPassword(user, password);
    }

    public bool VerifyPassword(User user, string password, string passwordHash)
    {
        var result = _passwordHasher.VerifyHashedPassword(user, passwordHash, password);
        return result == PasswordVerificationResult.Success;
    }
}