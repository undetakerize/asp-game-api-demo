using GameService.Application.Interfaces.Reviews;
using GameService.Domain.Entity.GameReviews;
using GameService.Domain.Entity.Reviews;
using GameService.DTO.Review;
using GameService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GameService.Infrastructure;

public class ReviewRepository: IReviewRepository
{
    private readonly AppDbContext _context;

    public ReviewRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<Review>> GetAllAsync()
    {
        return await _context.Review.ToListAsync();
    }

    public async Task<Review?> GetByIdAsync(int id)
    {
        return await _context.Review.FindAsync(id);
    }

    public async Task<GameReview> CreateReviewAsync(GameReview review)
    {
        await _context.Review.AddAsync(review.Review);
        await _context.GameReview.AddAsync(review);
        await _context.SaveChangesAsync();
        return review;
    }

    public async Task<Review?> UpdateReviewAsync(int id, UpdateReviewDto dto)
    {
        var review = await _context.Review.FirstOrDefaultAsync(r => r.Id == id);
        if (review == null) return review;
        review.Title = dto.Title;
        review.Content = dto.Content;
        await _context.SaveChangesAsync();
        return review;
    }

    public async Task<Review?> DeleteReviewAsync(int id)
    {
        var review = await _context.Review.FirstOrDefaultAsync(r => r.Id == id);
        if (review == null) return review;
        _context.Review.Remove(review);
        await _context.SaveChangesAsync();
        return review;
    }

    public Task<bool> ReviewExistsAsync(int id)
    {
        return _context.Review.AnyAsync(r => r.Id == id);
    }
}