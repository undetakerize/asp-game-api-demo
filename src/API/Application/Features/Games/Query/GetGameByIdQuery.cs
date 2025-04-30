using GameService.Domain.Entity.Games;
using MediatR;

namespace GameService.Application.Features.Games.Query;

public class GetGameByIdQuery : IRequest<Game?>
{
    public int? Id { get; set; }
}