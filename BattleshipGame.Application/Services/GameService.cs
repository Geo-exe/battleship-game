using BattleshipGame.Application.Interfaces;
using System.Collections.Concurrent;
using BattleshipGame.Core.Models;
using FluentResults;
using BattleshipGame.Core.Configurations;
using Microsoft.Extensions.Options;

namespace BattleshipGame.Application.Services; 

/// <inheritdoc />
public class GameService(IOptions<GameSettings> gameSettingsOptions) : IGameService
{
    private static readonly ConcurrentDictionary<Guid, GameBoard> _games = [];
    private readonly GameSettings _gameSettings = gameSettingsOptions.Value;

    /// <inheritdoc />
    public Result<Guid> CreateGame()
    {
        try 
        {
            var gameId = Guid.NewGuid();
            
            var board = new GameBoard(_gameSettings);
            
            board.InitializeRandom();
            _games.TryAdd(gameId, board);

            return Result.Ok(gameId);
        }
        catch (Exception ex)
        {
            return Result.Fail(new Error("Failed to initialize game").CausedBy(ex));
        }
    }

    /// <inheritdoc />
    public Result<FireResult> Fire(Guid gameId, int x, int y)
    {
        int maxSize = _gameSettings.BoardSize;
        if (x < 0 || x >= maxSize || y < 0 || y >= maxSize)
        {
            return Result.Fail($"Coordinates must be between 0 and {maxSize - 1}");
        }

        if (!_games.TryGetValue(gameId, out var board))
        {
            return Result.Fail("Game not found. Please init a new game.");
        }

        var result = board.Fire(new Coordinate(x, y));

        return Result.Ok(result);
    }

    /// <inheritdoc />
    public GameSettings GetConfig()
    {
        return _gameSettings;
    }
}