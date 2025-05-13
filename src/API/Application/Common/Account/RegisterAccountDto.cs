using System.ComponentModel.DataAnnotations;

namespace GameService.Application.Common.Account;

public class RegisterAccountDto
{
    [Required]
    public required string Username { get; set; }
    [Required]
    [EmailAddress]
    public required string Email { get; set; }
    [Required]
    public required string Password { get; set; }
}