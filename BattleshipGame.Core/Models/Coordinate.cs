namespace BattleshipGame.Core.Models;

/// <summary>
/// Represents a specific point on the game board grid defined by X and Y coordinates.
/// </summary>
/// <param name="X">The horizontal coordinate (0-based index).</param>
/// <param name="Y">The vertical coordinate (0-based index).</param>
public record Coordinate(int X, int Y)
{
    /// <summary>
    /// Returns the string representation of the coordinate in "(X,Y)" format.
    /// </summary>
    /// <returns>A formatted string (e.g., "(0,5)").</returns>
    public override string ToString() => $"({X},{Y})";
}