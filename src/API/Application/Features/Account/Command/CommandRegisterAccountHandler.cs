using GameService.Application.Common;
using GameService.Application.Features.Account.DTO;
using GameService.Application.Interfaces;
using GameService.Application.Interfaces.Account;
using GameService.Domain.Users;
using MediatR;

namespace GameService.Application.Features.Account.Command;

public class CommandRegisterAccountHandler(
    IUserIdentity userIdentity, 
    ITokenService tokenService)
    : IRequestHandler<CommandRegisterAccount, Result<ResponseRegisterAccountDto>>
{
    public async Task<Result<ResponseRegisterAccountDto>> Handle(CommandRegisterAccount request, CancellationToken cancellationToken)
    {
        var appUser = new AppUser
        {
            Email = request.Email,
            UserName = request.Username,
        };

        var userExist = await userIdentity.FindByEmailAsync(request.Email);
        if (userExist != null)
            return Result<ResponseRegisterAccountDto>.Failure("User Already Exist");
        var createUserResult = await userIdentity.CreateAsync(appUser, request.Password);
        if (!createUserResult.Succeeded)
            return Result<ResponseRegisterAccountDto>.Failure("Failed to create user");
        var roleResult = await userIdentity.AddToRoleAsync(appUser, "User");
        if (!roleResult.Succeeded)
            return Result<ResponseRegisterAccountDto>.Failure("Failed to assign role");
        return Result<ResponseRegisterAccountDto>.Success(new ResponseRegisterAccountDto
        {
            Username = appUser.UserName,
            Email = appUser.Email,
            Token = tokenService.GenerateToken(appUser)
        });
    }
}