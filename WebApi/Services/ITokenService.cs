using Microsoft.AspNetCore.Identity;
using WebApi.Models;

namespace WebApi.Services;

public interface ITokenService
{
    public TokenResult GenerateToken(IdentityUser user);
}