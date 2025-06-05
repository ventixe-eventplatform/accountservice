using System.ComponentModel.DataAnnotations;

namespace WebApi.Models;

public class SignInRequestModel
{
    [Required]
    public string Email { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;
}
