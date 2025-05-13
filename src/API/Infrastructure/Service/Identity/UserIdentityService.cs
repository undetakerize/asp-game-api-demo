using GameService.Application.Interfaces.Account;
using GameService.Domain.Users;
using Microsoft.AspNetCore.Identity;

namespace GameService.Infrastructure.Service.Identity;

public class UserIdentityService(UserManager<AppUser> userManager) : IUserIdentity
{
    public async Task<AppUser?> FindByEmailAsync(string email)
    {
        return await userManager.FindByEmailAsync(email);
    }
    public async Task<IdentityResult> CreateAsync(AppUser user, string password)
    {
        return await userManager.CreateAsync(user, password);
    }
    public async Task<IdentityResult> AddToRoleAsync(AppUser user, string roleName)
    {
        return await userManager.AddToRoleAsync(user, roleName);
    }
}