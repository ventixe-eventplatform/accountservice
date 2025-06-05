using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using WebApi.Factories;
using WebApi.Models;

namespace WebApi.Services;

public class AccountService(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, ITokenService tokenService) : IAccountService
{
    private readonly UserManager<IdentityUser> _userManager = userManager;
    private readonly SignInManager<IdentityUser> _signInManager = signInManager;
    private readonly ITokenService _tokenService = tokenService;

    public async Task<AccountServiceResult<SignInResponseModel>> RegisterAsync(RegisterUserModel model)
    {
        try
        {
            var entity = UserFactory.Create(model);
            var result = await _userManager.CreateAsync(entity, model.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return new AccountServiceResult<SignInResponseModel> { Success = false, Error = errors };
            }

            await _signInManager.SignInAsync(entity, isPersistent: false);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return new AccountServiceResult<SignInResponseModel> { Success = false, Error = "User not found after sign in." };

            var signInResponse = await SignInAsync(new SignInRequestModel
            {
                Email = model.Email,
                Password = model.Password,
            });
            return signInResponse;

        } catch (Exception ex)
        {
            Debug.WriteLine($"Error during registration: {ex.Message}");
            return new AccountServiceResult<SignInResponseModel> { Success = false, Error = "Unexpected error during registration." };
        }
    }

    public async Task<AccountServiceResult<SignInResponseModel>> SignInAsync(SignInRequestModel model)
    {
        if (model == null || string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Password))
            return new AccountServiceResult<SignInResponseModel>
            { Success = false, Error = "Email and password are required." };

        var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: false, lockoutOnFailure: false);
        if (!result.Succeeded)
            return new AccountServiceResult<SignInResponseModel>
            { Success = false, Error = "Failed to sign in." };

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
            return new AccountServiceResult<SignInResponseModel>
            { Success = false, Error = "User not found after sign in." };

        var tokenResult = _tokenService.GenerateToken(user);

        return new AccountServiceResult<SignInResponseModel>
        { Success = true, Data = new SignInResponseModel
            {
                Token = tokenResult.Token,
                Email = model.Email,
                ExpiresAt = tokenResult.ExpiresAt,
                UserId = user.Id
            }
        };
    }

    public async Task SignOutAsync()
    {
        await _signInManager.SignOutAsync();
    }

    public async Task<AccountServiceResult<bool>> UserExistsAsync(EmailRequest request)
    {
        var exists = await _userManager.Users.AnyAsync(x => x.Email == request.Email);
        
        return exists 
            ? new AccountServiceResult<bool>{ Success = true, Error = "User already exists.", Data = exists } 
            : new AccountServiceResult<bool> { Success = true, Data = exists };
    }
}
