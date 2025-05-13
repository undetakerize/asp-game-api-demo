using GameService.Application.Common;
using GameService.Application.Features.Account.DTO;
using MediatR;

namespace GameService.Application.Features.Account.Command;

public record CommandRegisterAccount(
    String Username,
    String Password,
    String Email
) : IRequest<Result<ResponseRegisterAccountDto>>;
