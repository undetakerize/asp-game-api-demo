using GameService.Application.Common;
using MediatR;

namespace GameService.Application.Features.Account.Command;

public record CommandLoginAccount(
    String Email, 
    String Password
) : IRequest <Result<Object>>;