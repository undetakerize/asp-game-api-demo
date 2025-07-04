using GameService.Application.Features.Games.DTO;
using GameService.Domain.Entity.Games;
using MediatR;

namespace GameService.Application.Features.Games.Command;

public record CommandCreateGame
(
    string Title,
    string Description,
    string? ReleaseDate,
    decimal Price
) : IRequest<Game?>;
