using GameService.Domain.Entity.Games;
using MediatR;

namespace GameService.Application.Features.Games.Query;

public record GetGameByIdQuery(int Id) : IRequest<Game?>;
