namespace WebApi.Models;

public class SignInResponseModel
{
    public string Token { get; set; } = null!;
    public string Email { get; set; } = null!;
    public DateTime ExpiresAt {  get; set; }
    public string? UserId { get; set; }
}
