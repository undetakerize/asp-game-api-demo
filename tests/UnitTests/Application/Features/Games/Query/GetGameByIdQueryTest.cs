using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GameService.Application.Features.Games.Query;
using GameService.Domain.Entity.GameReviews;
using GameService.Domain.Entity.Games;
using GameService.Domain.Entity.Reviews;
using GameService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace UnitTestService.Application.Features.Games.Query;

[TestFixture]
public class GetGameByIdQueryTest
{
    private AppDbContext _context = null!;
    private GetGameByIdQueryHandler _handler = null!;

    [SetUp]
    public void SetUp()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;

        _context = new AppDbContext(options);

        // Seed data
        var review = new Review { Id = 1, Name = "User", Content = "Great!" };
        var game = new Game
        {
            Id = 1,
            Title = "Test Game",
            Description = "An adventure game",
            GameReviews = new List<GameReview>
            {
                new GameReview { GameId = 1, ReviewId = 1, Review = review }
            }
        };

        _context.Game.Add(game);
        _context.Review.Add(review);
        _context.SaveChanges();

        _handler = new GetGameByIdQueryHandler(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Test]
    public async Task Handle_GameWithReviews_ReturnsGameWithReviews()
    {
        // act
        var result = await _handler.Handle(new GetGameByIdQuery(1), CancellationToken.None);
        
        // assertion
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Id, Is.EqualTo(1));
        Assert.That(result.GameReviews, Is.Not.Empty);
        Assert.That(result.GameReviews.First().Review.Content, Is.EqualTo("Great!"));
    }
}