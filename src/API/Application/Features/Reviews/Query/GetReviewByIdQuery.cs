using GameService.Domain.Entity.Reviews;
using MediatR;

namespace GameService.Application.Features.Reviews.Query;

public record GetReviewByIdQuery(int Id) : IRequest<Review?>;