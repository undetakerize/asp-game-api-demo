using GameService.Domain.Users;

namespace GameService.Application.Interfaces;

public interface ITokenService
{
    string GenerateToken(AppUser user);
}