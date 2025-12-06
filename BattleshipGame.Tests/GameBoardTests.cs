using Xunit;
using FluentAssertions;
using BattleshipGame.Core.Models;
using BattleshipGame.Core.Enums;
using BattleshipGame.Core.DTOs;
using System.Collections.Generic;
using BattleshipGame.Core.Configurations;

namespace BattleshipGame.Tests;

public class GameBoardTests
{
    private readonly GameSettings _settings = new() 
    { 
        BoardSize = 10, 
        Ships = []
    };

    [Fact]
    public void Fire_ShouldReturnMiss_WhenNoShipIsHit()
    {
        var board = new GameBoard(_settings);

        var result = board.Fire(new Coordinate(5, 5));

        result.Status.Should().Be(FireStatus.Missed);
        board.MissedShots.Should().Contain(new Coordinate(5, 5));
    }

    [Fact]
    public void Fire_ShouldReturnHit_WhenShipIsHit()
    {
        var board = new GameBoard(_settings);
        
        var ship = new Ship("TestDestroyer", 3);
        ship.Place([new Coordinate(0, 0), new Coordinate(0, 1), new Coordinate(0, 2)]);
        board.Ships.Add(ship);

        var result = board.Fire(new Coordinate(0, 0));

        result.Status.Should().Be(FireStatus.Hit);
        ship.Hits.Should().Be(1);
        ship.IsSunk.Should().BeFalse();
    }

    [Fact]
    public void Fire_ShouldReturnSank_WhenLastShipPartIsHit()
    {
        var board = new GameBoard(_settings);
        
        var ship = new Ship("Patrol", 1);
        ship.Place([new Coordinate(9, 9)]);
        board.Ships.Add(ship);

        var result = board.Fire(new Coordinate(9, 9));

        result.Status.Should().Be(FireStatus.Sank);
        result.ShipName.Should().Be("Patrol");
        ship.IsSunk.Should().BeTrue();
    }

    [Fact]
    public void InitializeRandom_ShouldPlaceShipsWithinBounds_AndNoOverlaps()
    {
        var realSettings = new GameSettings
        {
            BoardSize = 10,
            Ships = 
            [
                new() { Name = "S1", Size = 5 },
                new() { Name = "S2", Size = 4 }
            ]
        };
        var board = new GameBoard(realSettings);

        board.InitializeRandom();

        board.Ships.Should().HaveCount(2);
        
        foreach (var ship in board.Ships)
        {
            foreach (var coord in ship.Coordinates)
            {
                coord.X.Should().BeInRange(0, 9);
                coord.Y.Should().BeInRange(0, 9);
            }
        }

        var allCoords = board.Ships.SelectMany(s => s.Coordinates).ToList();
        allCoords.Should().OnlyHaveUniqueItems();
    }
}