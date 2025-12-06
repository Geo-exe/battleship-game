using BattleshipGame.Core.Configurations;
using BattleshipGame.Core.Models;
using FluentResults;

namespace BattleshipGame.Application.Interfaces;

/// <summary>
/// Defines the core use cases and business logic for the Battleship game.
/// </summary>
public interface IGameService
{
    /// <summary>
    /// Initializes a new game session with a randomized board state.
    /// </summary>
    /// <remarks>
    /// This method creates a new game board and randomly places all configured ships
    /// ensuring no overlaps and staying within grid boundaries.
    /// </remarks>
    /// <returns>A Result containing the unique identifier (GUID) of the newly created game.</returns>
    Result<Guid> CreateGame();

    /// <summary>
    /// Processes a player's shot at the specified coordinates.
    /// </summary>
    /// <param name="gameId">The unique identifier of the active game session.</param>
    /// <param name="x">The horizontal coordinate (0-based index).</param>
    /// <param name="y">The vertical coordinate (0-based index).</param>
    /// <returns>
    /// A Result containing the outcome of the shot (<see cref="FireResult"/>).
    /// Returns a failure if the game is not found or coordinates are out of bounds.
    /// </returns>
    Result<FireResult> Fire(Guid gameId, int x, int y);

    /// <summary>
    /// Retrieves the static configuration settings used for the game.
    /// </summary>
    /// <returns>The <see cref="GameSettings"/> object containing board size and ship definitions.</returns>
    GameSettings GetConfig();
}