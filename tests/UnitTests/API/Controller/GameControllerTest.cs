using System;
using System.Threading;
using System.Threading.Tasks;
using GameService.API.Controller;
using GameService.Application.Features.Games.DTO;
using GameService.Application.Features.Games.Query;
using GameService.Application.Interfaces.Games;
using GameService.Domain.Entity.Games;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace UnitTestService.API.Controller;

[TestFixture]
[TestOf(typeof(GameController))]
public class GameControllerTest
{

    private Mock<IMediator> _mockMediator = null!;
    private Mock<IGameEventProducer> _mockGameEventProducer = null!;
    private Mock<ILogger<GameController>> _mockLogger = null!;
    private GameController _controller = null!;

    [SetUp]
    public void SetUp()
    {
        _mockMediator = new Mock<IMediator>();
        _mockGameEventProducer = new Mock<IGameEventProducer>();
        _mockLogger = new Mock<ILogger<GameController>>();
        _controller = new GameController(_mockMediator.Object, _mockGameEventProducer.Object, _mockLogger.Object);
    }
    
    [Test]
    public async Task GetById_GameExists_ReturnsOk()
    {
        // set
        int gameId = 1;
        var fakeGame = new Game
        {
            Id = gameId,
            Title = "Cyberpunk 2077",
            Description = "Waking up in 2077 , Chip is always a weapon",
            Price = 490000.000m,
            ReleaseDate = new DateTime()
        };

        var game = _mockMediator
            .Setup(m => m.Send(It.Is<GetGameByIdQuery>(q => q.Id == gameId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeGame);

        // act get
        var result = await _controller.GetById(gameId);

        // assertion
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
    }
    
    [Test]
    public async Task GetById_ReturnsNotFound_WhenGameDoesNotExist()
    {
        // arrange
        var gameId = 2;
        _mockMediator.Setup(m => m.Send(It.IsAny<GetGameByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Game)null!);

        // act get
        var result = await _controller.GetById(gameId);

        // assertion
        Assert.That(result, Is.InstanceOf<NotFoundResult>());
    }
}