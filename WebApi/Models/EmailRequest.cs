using System.ComponentModel.DataAnnotations;

namespace WebApi.Models;

public class EmailRequest
{
    [Required]
    public string Email { get; set; } = null!;
}
