using WebApi.Models;

namespace WebApi.Services;

public interface IAccountService
{
    Task<AccountServiceResult<SignInResponseModel>> RegisterAsync(RegisterUserModel model);
    Task<AccountServiceResult<SignInResponseModel>> SignInAsync(SignInRequestModel model);
    Task SignOutAsync();
    Task<AccountServiceResult<bool>> UserExistsAsync(EmailRequest request);
}
