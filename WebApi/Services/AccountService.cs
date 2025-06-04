using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApi.Factories;
using WebApi.Models;

namespace WebApi.Services;

public class AccountService(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager) : IAccountService
{
    private readonly UserManager<IdentityUser> _userManager = userManager;
    private readonly SignInManager<IdentityUser> _signInManager = signInManager;

    public async Task<AccountServiceResult> RegisterAsync(RegisterUserModel model)
    {
        try
        {
            var entity = UserFactory.Create(model);
            var result = await _userManager.CreateAsync(entity, model.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return new AccountServiceResult { Success = false, Error = errors };
            }

            await _signInManager.SignInAsync(entity, isPersistent: false);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return new AccountServiceResult { Success = false, Error = "User not found after sign in." };

            return new AccountServiceResult { Success = true, Message = "Registration succeeded.", UserId = user.Id.ToString() };

        } catch (Exception ex)
        {
            Debug.WriteLine($"Error during registration: {ex.Message}");
            return new AccountServiceResult { Success = false, Error = "Unexpected error during registration." };
        }
    }

    public async Task<AccountServiceResult> SignInAsync(SignInModel model)
    {
        if (model == null || string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Password))
            return new AccountServiceResult { Success = false, Error = "Email and password are required." };

        var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: false, lockoutOnFailure: false);
        if (!result.Succeeded)
            return new AccountServiceResult { Success = false, Message = "Failed to sign in." };

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
            return new AccountServiceResult { Success = false, Error = "User not found after sign in." };

        return new AccountServiceResult { Success = true, UserId = user.Id.ToString() };
    }

    public async Task SignOutAsync()
    {
        await _signInManager.SignOutAsync();
    }

    public async Task<AccountServiceResult> UserExistsAsync(EmailRequest request)
    {
        var exists = await _userManager.Users.AnyAsync(x => x.Email == request.Email);
        
        return exists 
            ? new AccountServiceResult{ Success = true, Error = "User already exists.", Data = exists } 
            : new AccountServiceResult { Success = true, Data = exists };
    }
}
