using WebApi.Models;

namespace WebApi.Services;

public interface IAccountService
{
    Task<AccountServiceResult> RegisterAsync(RegisterUserModel model);
    Task<AccountServiceResult> SignInAsync(SignInModel model);
    Task SignOutAsync();
    Task<AccountServiceResult> UserExistsAsync(EmailRequest request);
}
