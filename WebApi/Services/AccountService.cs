using Microsoft.AspNetCore.Identity;

namespace WebApi.Services;

public class AccountService(UserManager<IdentityUser> userManager) : IAccountService
{
    private readonly UserManager<IdentityUser> _userManager = userManager;
}
