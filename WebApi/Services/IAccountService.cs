using WebApi.Models;

namespace WebApi.Services;

public interface IAccountService
{
    Task<AccountServiceResultT<SignInResponseModel>> RegisterAsync(RegisterUserModel model);
    Task<AccountServiceResultT<SignInResponseModel>> SignInAsync(SignInRequestModel model);
    Task SignOutAsync();
    Task<AccountServiceResult> UserExistsAsync(EmailRequest request);
}
