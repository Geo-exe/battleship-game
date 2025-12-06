namespace BattleshipGame.Core.DTOs;

/// <summary>
/// The outcome of a shot fired during the game.
/// </summary>
/// <param name="Result">The result message of the shot (e.g., "Hit!", "Missed!", "Sank Submarine!").</param>
/// <param name="ShipSunk">The name of the ship sunk by this specific shot, or null if no ship was sunk.</param>
public record FireResponse(string Result, string? ShipSunk);