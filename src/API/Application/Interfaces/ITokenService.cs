namespace GameService.Domain.Users;

public interface ITokenService
{
    string GenerateToken(AppUser user);
}