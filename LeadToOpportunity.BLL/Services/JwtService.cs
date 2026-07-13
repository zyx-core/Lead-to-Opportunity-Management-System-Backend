using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LeadToOpportunity.BLL.interfaces;
using LeadToOpportunity.BLL.Settings;
using LeadToOpportunity.Models.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace LeadToOpportunity.BLL.Services;

public class JwtService : IJwtService
{
    private readonly JwtSetting _jwtSetting;

    public JwtService(IOptions<JwtSetting>options){
        _jwtSetting = options.Value;
        
    }

    public string GenerateToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email.ToString()),
            new Claim(ClaimTypes.Role, user.Role.ToString())

            
        }  ;
        var Key  = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_jwtSetting.Key)
        );

        var credentials = new SigningCredentials(
            Key,
            SecurityAlgorithms.HmacSha256
        );

        var token = new JwtSecurityToken(
            issuer:_jwtSetting.Issuer,
            audience: _jwtSetting.Audience,
            claims:claims,
            expires:DateTime.UtcNow.AddMinutes(_jwtSetting.DurationInMinutes),
            signingCredentials:credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

}