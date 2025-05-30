namespace WebApi.Models;

public class AccountServiceResult
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public string? Error { get; set; }
    public bool? Data { get; set; }
    public string? UserId { get; set; }
}
