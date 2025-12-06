using Xunit;
using FluentAssertions;
using Moq;
using Microsoft.Extensions.Options;
using BattleshipGame.Application.Services;
using BattleshipGame.Core.Models;
using System;
using System.Collections.Generic;
using BattleshipGame.Core.Configurations;

namespace BattleshipGame.Tests;

public class GameServiceTests
{
    private readonly Mock<IOptions<GameSettings>> _mockOptions;
    private readonly GameSettings _settings;

    public GameServiceTests()
    {
        _mockOptions = new Mock<IOptions<GameSettings>>();
        
        _settings = new GameSettings
        {
            BoardSize = 10,
            Ships =
            [
                new() { Name = "TestShip", Size = 3 } 
            ]
        };

        _mockOptions.Setup(ap => ap.Value).Returns(_settings);
    }

    [Fact]
    public void CreateGame_ShouldReturnValidGuid()
    {
        var service = new GameService(_mockOptions.Object);

        var result = service.CreateGame();

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();
    }

    [Fact]
    public void Fire_ShouldFail_WhenGameIdDoesNotExist()
    {
        var service = new GameService(_mockOptions.Object);
        var randomId = Guid.NewGuid();

        var result = service.Fire(randomId, 0, 0);

        result.IsFailed.Should().BeTrue();
        result.Errors.Should().Contain(e => e.Message.Contains("Game not found"));
    }

    [Fact]
    public void Fire_ShouldFail_WhenCoordinatesAreOutOfBounds()
    {
        var service = new GameService(_mockOptions.Object);
        var gameId = service.CreateGame().Value;

        var resultNegative = service.Fire(gameId, -1, 5);
        var resultTooBig = service.Fire(gameId, 10, 10);

        resultNegative.IsFailed.Should().BeTrue("Negative coordinates should fail");
        resultTooBig.IsFailed.Should().BeTrue("Coordinates >= Size should fail");
        
        resultTooBig.Errors[0].Message.Should().Contain("between 0 and 9");
    }

    [Fact]
    public void Fire_ShouldSucceed_WhenCoordinatesAreValid()
    {
        var service = new GameService(_mockOptions.Object);
        var gameId = service.CreateGame().Value;

        var result = service.Fire(gameId, 5, 5);

        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void Fire_ShouldNotRegisterDoubleHits_OnSameShipSpot()
    {
        var board = new GameBoard(_settings);
        var ship = new Ship("Test", 3);
        ship.Place([new Coordinate(0,0), new Coordinate(0,1), new Coordinate(0,2)]);
        board.Ships.Add(ship);

        board.Fire(new Coordinate(0, 0));
        board.Fire(new Coordinate(0, 0));

        ship.Hits.Should().Be(1);
    }
}