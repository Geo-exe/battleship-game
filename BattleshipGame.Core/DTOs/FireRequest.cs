namespace BattleshipGame.Core.DTOs;

/// <summary>
/// Represents a request to fire a shot at a specific target on the grid.
/// </summary>
/// <param name="GameId">The unique identifier of the active game session.</param>
/// <param name="X">The horizontal coordinate (0-based index).</param>
/// <param name="Y">The vertical coordinate (0-based index).</param>
public record FireRequest(Guid GameId, int X, int Y);