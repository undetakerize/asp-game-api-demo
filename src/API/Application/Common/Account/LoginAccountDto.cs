using System.ComponentModel.DataAnnotations;

namespace GameService.Application.Common.Account;

public partial class LoginAccountDto
{
    [Required (AllowEmptyStrings = false, ErrorMessage = "Email is required")]
    [EmailAddress]
    public string Email { get; set; }
    [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required")]
    public string Password { get; set; }
}