using System.ComponentModel.DataAnnotations;

namespace GameService.Application.Common.Account;

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