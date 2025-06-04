using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Services;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountsController(IAccountService accountService) : ControllerBase
{
    private readonly IAccountService _accountService = accountService;

    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterUserModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(new { Error = "Email or password not valid." });

        var result = await _accountService.RegisterAsync(model);
        return result.Success ? Ok(result) : StatusCode(500, result.Error);
    }

    [HttpPost("signin")]
    public async Task<IActionResult> SignInAsync([FromBody] SignInModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(new { Error = "Email and password do not match." });

        var result = await _accountService.SignInAsync(model);
        return result.Success ? Ok(result) : StatusCode(500, result?.Error);
    }

    [HttpPost("signout")]
    public async Task<IActionResult> SignOutAsync()
    {
        await _accountService.SignOutAsync();
        return Ok(new { messsage = "Signed out successfully." });
    }

    [HttpPost("exists")]
    public async Task<IActionResult> UserExistsAsync([FromBody] EmailRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var result = await _accountService.UserExistsAsync(request);
        return Ok(result);
    }
}
