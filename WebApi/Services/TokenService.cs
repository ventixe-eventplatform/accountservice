
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using WebApi.Models;

namespace WebApi.Services;

public class TokenService(IConfiguration config) : ITokenService
{
    private readonly IConfiguration _config = config;

    // Source: https://www.youtube.com/watch?v=w8I32UPEvj8 and
    // https://medium.com/@solomongetachew112/jwt-authentication-in-net-8-a-complete-guide-for-secure-and-scalable-applications-6281e5e8667c

    public TokenResult GenerateToken(IdentityUser user)
    {
        var issuer = _config["JwtConfig:Issuer"];
        var audience = _config["JwtConfig:Audience"];
        var key = _config["JwtConfig:Key"];

        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var tokenValidityMin = _config.GetValue<int>("JwtConfig:TokenValidityMins");
        var tokenExpiryTimeStamp = DateTime.UtcNow.AddMinutes(tokenValidityMin);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
        };

        var creds = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken
            (
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: tokenExpiryTimeStamp,
                signingCredentials: creds
            );

        return new TokenResult { Token = new JwtSecurityTokenHandler().WriteToken(token), ExpiresAt = tokenExpiryTimeStamp };
    }
}
