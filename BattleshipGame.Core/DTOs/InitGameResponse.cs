namespace BattleshipGame.Core.DTOs;

/// <summary>
/// The response returned when a new game is successfully initialized.
/// </summary>
/// <param name="GameId">The unique identifier of the new game session. This ID must be included in subsequent requests (e.g., firing shots).</param>
/// <param name="Message">A status message confirming the initialization (e.g., "Battle initialized").</param>
public record InitGameResponse(Guid GameId, string Message);