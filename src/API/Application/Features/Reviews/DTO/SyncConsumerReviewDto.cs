using System.ComponentModel.DataAnnotations;
using GameService.Domain.Entity.Reviews;

namespace GameService.Application.Features.Reviews.DTO;

public class SyncConsumerReviewDto
{
    public required string Action { get; set; }
    public Review? Review { get; set; } // Review data related to the event
}