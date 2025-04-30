using System.ComponentModel.DataAnnotations;
using Lombok.NET;

namespace GameService.Application.Common.Account;

[With]
public partial class RegisterAccountDto
{
    [Required]
    public string? Username { get; set; }
    [Required]
    [EmailAddress]
    public string? Email { get; set; }
    [Required]
    public string? Password { get; set; }
}