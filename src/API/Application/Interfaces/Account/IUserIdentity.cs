using GameService.Domain.Users;
using Microsoft.AspNetCore.Identity;

namespace GameService.Application.Interfaces.Account;

public interface IUserIdentity
{
    Task<AppUser?> FindByEmailAsync(string email);
    Task<IdentityResult> CreateAsync(AppUser user, string password);
    Task<IdentityResult> AddToRoleAsync(AppUser user, string roleName);
}