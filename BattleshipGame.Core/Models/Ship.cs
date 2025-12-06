namespace BattleshipGame.Core.Models;

/// <summary>
/// Represents a ship entity in the game, tracking its position, size, and health status.
/// </summary>
/// <param name="name">The display name of the ship (e.g., "Aircraft Carrier").</param>
/// <param name="size">The length of the ship in grid cells.</param>
public class Ship(string name, int size)
{
    /// <summary>
    /// Gets the display name of the ship.
    /// </summary>
    public string Name { get; } = name;

    /// <summary>
    /// Gets the structural size (length) of the ship.
    /// </summary>
    public int Size { get; } = size;

    /// <summary>
    /// Gets the list of coordinates currently occupied by the ship on the board.
    /// </summary>
    public List<Coordinate> Coordinates { get; private set; } = [];

    /// <summary>
    /// Gets the current number of hits sustained by the ship.
    /// </summary>
    public int Hits { get; private set; }

    /// <summary>
    /// Determines if the ship is completely destroyed.
    /// </summary>
    /// <remarks>
    /// A ship is considered sunk when the number of <see cref="Hits"/> equals or exceeds its <see cref="Size"/>.
    /// </remarks>
    public bool IsSunk => Hits >= Size;

    /// <summary>
    /// Registers damage to the ship by incrementing the hit counter.
    /// </summary>
    public void RegisterHit() => Hits++;

    /// <summary>
    /// Assigns the ship to a specific set of coordinates on the grid.
    /// </summary>
    /// <param name="coordinates">The list of coordinates the ship will occupy.</param>
    public void Place(List<Coordinate> coordinates)
    {
        Coordinates = coordinates;
    }
}