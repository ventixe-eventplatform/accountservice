namespace WebApi.Models;

public class TokenResult
{
    public string Token { get; set; } = null!;
    public DateTime ExpiresAt { get; set; }
}
