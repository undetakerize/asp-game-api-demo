namespace GameService.Application.Features.Account.DTO;

public class ResponseRegisterAccountDto()
{
    public required string Username {get; set; }
    public required string Email {get; set; }
    public required string Token {get; set; }
}