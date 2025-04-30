using GameService.Domain.Entity.Games;
using MediatR;

namespace GameService.Application.Features.Games.Command;

public record CommandUpdateGame 
(
     int Id ,
     string Title ,
     string Description,
     DateTime ReleaseDate,
     decimal Price
) : IRequest<Game?>;