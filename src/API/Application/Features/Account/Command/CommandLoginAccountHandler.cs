using GameService.Application.Common;
using GameService.Application.Interfaces;
using GameService.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace GameService.Application.Features.Account.Command;

public class CommandLoginAccountHandler(UserManager<AppUser> userManager, 
    SignInManager<AppUser> signInManager , ITokenService tokenService) : IRequestHandler<CommandLoginAccount, Result<Object>>
{
    public async Task<Result<Object>> Handle(CommandLoginAccount request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user == null) return Result<Object>.Failure("Invalid username!", 404);
        var result = await signInManager.CheckPasswordSignInAsync(user, request.Password, false);
        if(!result.Succeeded) return Result<Object>.Failure("Invalid username or password!", 401);
        return Result<Object>.Success(new
        {
            Token = tokenService.GenerateToken(user)
        });
    }
}