using GameService.Domain.Entity.Games;
using MediatR;

namespace GameService.Application.Features.Games.Command;

public record CommandDeleteGame
(
    int Id
) : IRequest<Game?>;