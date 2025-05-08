using System.Threading;
using System.Threading.Tasks;
using GameService.Application.Features.Reviews.Query;
using GameService.Domain.Entity.Reviews;
using GameService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace UnitTestService.Application.Features.Reviews.Query;

[TestFixture]
public class GetReviewByIdQueryTest
{
    private AppDbContext _context;
    private GetReviewByIdQueryHandler _handler;

    [SetUp]
    public void SetUp()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName:"TestDb")
            .Options;
        
        _context = new AppDbContext(options);
        
        // seed data
        var review = new Review
        {
            Id = 1,
            Name = "Test",
            Content = "Test",
        };
        
        _context.Add(review);
        _context.SaveChanges();
        
        _handler = new GetReviewByIdQueryHandler(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Test]
    public async Task Handle_Reviews_ReturnsReview()
    {
        //act
        var result = await _handler.Handle(new GetReviewByIdQuery(1), CancellationToken.None);
        
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Id, Is.EqualTo(1));
        Assert.That(result!.Name, Is.EqualTo("Test"));
    }
}