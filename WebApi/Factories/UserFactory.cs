using Microsoft.AspNetCore.Identity;
using WebApi.Models;

namespace WebApi.Factories;

public static class UserFactory
{
    public static IdentityUser Create(RegisterUserModel model)
    {
        return new IdentityUser
        {
            UserName = model.Email,
            Email = model.Email
        };
    }
}
