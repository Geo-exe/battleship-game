using System;
using System.Collections.Generic;
using System.Linq;
using BattleshipGame.Core.Configurations;
using BattleshipGame.Core.Enums;

namespace BattleshipGame.Core.Models;

/// <summary>
/// Represents the game board containing the grid, ships, and shot history.
/// </summary>
/// <remarks>
/// This class encapsulates the core game rules, including ship placement validation,
/// boundary checks, and hit/miss detection logic.
/// </remarks>
/// <param name="settings">The game configuration containing board size and ship definitions.</param>
public class GameBoard(GameSettings settings)
{
    /// <summary>
    /// Gets the dimension of the square grid (e.g., 10).
    /// </summary>
    public int BoardSize => settings.BoardSize;

    /// <summary>
    /// The collection of ships currently placed on the board.
    /// </summary>
    public List<Ship> Ships { get; } = [];

    /// <summary>
    /// A record of coordinates where shots have been fired but hit no ship.
    /// </summary>
    public List<Coordinate> MissedShots { get; } = [];

    /// <summary>
    /// Clears the board and randomly places all ships defined in the settings.
    /// </summary>
    /// <remarks>
    /// The algorithm attempts to place each ship in a random position and orientation.
    /// It retries until a valid position (no overlap, within bounds) is found.
    /// </remarks>
    public void InitializeRandom()
    {
        var random = new Random();
        Ships.Clear();
        MissedShots.Clear(); // Reset anche dello storico colpi

        foreach (var shipDef in settings.Ships)
        {
            bool placed = false;
            // Safety break counter could be added here for robustness
            while (!placed)
            {
                bool isHorizontal = random.Next(2) == 0;
                int startX = random.Next(BoardSize);
                int startY = random.Next(BoardSize);

                var proposedCoordinates = new List<Coordinate>();
                for (int i = 0; i < shipDef.Size; i++)
                {
                    int x = isHorizontal ? startX + i : startX;
                    int y = isHorizontal ? startY : startY + i;
                    proposedCoordinates.Add(new Coordinate(x, y));
                }

                if (IsValidPlacement(proposedCoordinates))
                {
                    var newShip = new Ship(shipDef.Name, shipDef.Size);
                    newShip.Place(proposedCoordinates);
                    Ships.Add(newShip);
                    placed = true;
                }
            }
        }
    }

    /// <summary>
    /// Validates if a set of coordinates fits within the board and does not overlap with existing ships.
    /// </summary>
    /// <param name="proposedCoords">The list of coordinates occupied by the ship.</param>
    /// <returns>True if the placement is valid; otherwise, false.</returns>
    private bool IsValidPlacement(List<Coordinate> proposedCoords)
    {
        // Check 1: Boundaries
        if (proposedCoords.Any(c => c.X >= BoardSize || c.Y >= BoardSize))
            return false;

        // Check 2: Collisions
        foreach (var existingShip in Ships)
        {
            // Intersection check: if any proposed coordinate matches an existing one
            if (existingShip.Coordinates.Any(existing => 
                proposedCoords.Any(proposed => proposed.X == existing.X && proposed.Y == existing.Y)))
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// Processes a shot at the specified coordinate.
    /// </summary>
    /// <param name="coord">The target coordinate.</param>
    /// <returns>
    /// A <see cref="FireResult"/> indicating whether the shot was a hit, a miss, 
    /// or if it caused a ship to sink.
    /// </returns>
    public FireResult Fire(Coordinate coord)
    {
        // 1. Check for Hit
        var ship = Ships.FirstOrDefault(s => s.Coordinates.Contains(coord));

        if (ship != null)
        {
            ship.RegisterHit(coord);

            if (ship.IsSunk)
            {
                return new FireResult(FireStatus.Sank, ship.Name);
            }

            return new FireResult(FireStatus.Hit);
        }

        // 2. Handle Miss
        // Prevent duplicate entries for the same missed coordinate
        if (!MissedShots.Contains(coord))
        {
            MissedShots.Add(coord);
        }

        return new FireResult(FireStatus.Missed);
    }
}