using GameService.Domain.Entity.Reviews;
using MediatR;

namespace GameService.Application.Features.Reviews.Query;

public record GetReviewQuery : IRequest<List<Review>>;