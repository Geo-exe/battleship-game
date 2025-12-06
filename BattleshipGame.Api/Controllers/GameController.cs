using BattleshipGame.Core.DTOs;
using BattleshipGame.Application.Interfaces;
using BattleshipGame.Core.Enums;
using Microsoft.AspNetCore.Mvc;
using BattleshipGame.Core.Configurations;

namespace BattleshipGame.Api.Controllers;

/// <summary>
/// API Controller responsible for managing Battleship game sessions.
/// </summary>
/// <remarks>
/// Exposes endpoints to initialize a game, execute moves (fire shots), and retrieve game settings.
/// </remarks>
/// <param name="gameService">The injected application service that handles the core game logic.</param>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class GameController(IGameService gameService) : ControllerBase
{

    /// <summary>
    /// Initializes a new Battleship game session.
    /// </summary>
    /// <remarks>
    /// Randomly places ships on the grid according to the configuration.
    /// Returns a GameId to be used for firing shots.
    /// </remarks>
    /// <returns>The ID of the new game and a confirmation message.</returns>
    [HttpPost("init")]
    [ProducesResponseType(typeof(InitGameResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult InitGame()
    {
        var result = gameService.CreateGame();
        
        if (result.IsSuccess)
        {
            var response = new InitGameResponse(result.Value, "Battle initialized");
            return Ok(response);
        }
        
        return BadRequest(result.Errors);
    }

    /// <summary>
    /// Fires a shot at specific coordinates.
    /// </summary>
    /// <remarks>
    /// Submit X and Y coordinates to attack.
    /// The response indicates if the shot was a "Hit", "Miss", or if a ship "Sank".
    /// </remarks>
    /// <param name="request">The target coordinates and game ID.</param>
    /// <returns>The result of the shot (Hit/Miss/Sank).</returns>
    [HttpPost("fire")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(FireResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Fire([FromBody] FireRequest request)
    {
        if (request.GameId == Guid.Empty)
            return BadRequest("Invalid Game ID");

        var result = gameService.Fire(request.GameId, request.X, request.Y);

        if (result.IsSuccess)
        {
            var domainResult = result.Value;

            var response = new FireResponse(
                Result: domainResult.ToMessage(),
                ShipSunk: domainResult.Status == FireStatus.Sank ? domainResult.ShipName : null
            );

            return Ok(response);
        }

        return BadRequest(result.Errors);
    }

    /// <summary>
    /// Retrieves the current game configuration.
    /// </summary>
    /// <remarks>
    /// Useful for the frontend to determine grid size and available ships.
    /// </remarks>
    /// <returns>Configuration object containing BoardSize and Ships list.</returns>
    [HttpGet("config")]
    [ProducesResponseType(typeof(GameSettings), StatusCodes.Status200OK)]
    public IActionResult GetConfig()
    {
        return Ok(gameService.GetConfig()); 
    }
}