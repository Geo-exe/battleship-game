namespace BattleshipGame.Core.Configurations;

/// <summary>
/// Configuration settings for the Battleship game rules.
/// </summary>
public class GameSettings
{
    /// <summary>
    /// The size of the square grid side (e.g., 10 for a 10x10 board).
    /// </summary>
    public int BoardSize { get; set; }

    /// <summary>
    /// The list of ships available in the game to be placed on the board.
    /// </summary>
    public List<ShipDefinition> Ships { get; set; } = [];
}

/// <summary>
/// Defines the properties of a specific type of ship.
/// </summary>
public class ShipDefinition
{
    /// <summary>
    /// The display name of the ship (e.g., "Aircraft Carrier").
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The length of the ship expressed in number of grid cells.
    /// </summary>
    public int Size { get; set; }
}