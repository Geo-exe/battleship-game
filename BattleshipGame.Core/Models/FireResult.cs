using BattleshipGame.Core.Enums;

namespace BattleshipGame.Core.Models;

/// <summary>
/// Represents the internal result of a fire action on the game board.
/// </summary>
/// <param name="Status">The status of the shot (Missed, Hit, or Sank).</param>
/// <param name="ShipName">The name of the ship if it was sunk; otherwise null.</param>
public record FireResult(FireStatus Status, string? ShipName = null)
{
    /// <summary>
    /// Formats the result into a human-readable message conforming to the game rules.
    /// </summary>
    /// <returns>A string such as "Missed!", "Hit!", or "Sank [ShipName]!".</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the status is not a valid enum value.</exception>
    public string ToMessage() => Status switch
    {
        FireStatus.Missed => "Missed!",
        FireStatus.Hit => "Hit!",
        FireStatus.Sank => $"Sank {ShipName}!",
        _ => throw new ArgumentOutOfRangeException()
    };
}