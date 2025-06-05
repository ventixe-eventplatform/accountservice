namespace WebApi.Models;

public class AccountServiceResult<T>
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public string? Error { get; set; }
    public T? Data { get; set; }
    public string? UserId { get; set; }
}
