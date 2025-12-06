namespace BattleshipGame.Core.Enums;

/// <summary>
/// Represents the possible outcomes of a shot fired at the game board.
/// </summary>
public enum FireStatus
{
    /// <summary>
    /// The shot landed in the water and did not hit any ship.
    /// </summary>
    Missed,

    /// <summary>
    /// The shot hit a part of a ship, but the ship is still afloat.
    /// </summary>
    Hit,

    /// <summary>
    /// The shot hit the last remaining part of a ship, causing it to sink.
    /// </summary>
    Sank
}