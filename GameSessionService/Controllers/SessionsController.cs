using GameSessionService.DTOs;
using GameSessionService.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GameSessionService.Controllers
{
    /// <summary>
    /// Handles game session operations.
    /// </summary>
    [Route("sessions")]
    [ApiController]
    public class SessionsController : ControllerBase
    {
        private readonly ISessionService _service;

        public SessionsController(ISessionService service)
        {
            _service = service;
        }

        /// <summary>
        /// Starts a new game session for a player.
        /// Returns existing session if one already exists to ensure idempotency.
        /// </summary>
        /// <param name="request">PlayerId and GameId for session creation.</param>
        /// <returns>Session details including SessionId, StartedAt, Status.</returns>
        [HttpPost("start")]
        public async Task<IActionResult> StartSession(StartSessionRequestDto request)
        {
            var result =  await _service.StartSessionAsync(request);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves a session by its ID.
        /// Adds X-Cache header indicating whether the result came from cache.
        /// </summary>
        /// <param name="sessionId">The unique session ID.</param>
        /// <returns>Game session details or 404 if session not found.</returns>
        [HttpGet("{sessionId}")]
        public async Task<IActionResult> GetSession(string sessionId)
        {
            var result = await _service.GetSessionAsync(sessionId);

            if (result.gameSession == null)
                return NotFound();

            Response.Headers.Add("X-Cache", result.fromCache ? "Hit" : "Miss");

            return Ok(result.gameSession);
        }
    }
}
