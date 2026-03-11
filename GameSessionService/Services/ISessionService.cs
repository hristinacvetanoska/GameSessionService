namespace GameSessionService.Services
{
    using GameSessionService.DTOs;
    using GameSessionService.Models;

    /// <summary>
    /// Interface for session management service. 
    /// Defines methods for starting a session and retrieving session details.
    /// </summary>
    public interface ISessionService
    {

        /// <summary>
        /// Starts a new game session for a player. 
        /// If a session already exists for the same player and game,
        /// it returns the existing session instead of creating a new one.
        /// This ensures idempotency and prevents duplicate sessions.
        /// Uses SemaphoreSlim to prevent race conditions under concurrent requests.
        /// </summary>
        /// <param name="request">The session request.</param>
        /// <returns>Returns session response details.</returns>
        Task<StartSessionResponseDto> StartSessionAsync(StartSessionRequestDto request);

        /// <summary>
        /// Gets a game session by its ID. 
        /// It first checks the in-memory cache for the session,
        /// then calls the repository if not found in cache. 
        /// If the session is retrieved from the repository, it is added to the cache for future requests. 
        /// The method returns a tuple indicating whether the session was retrieved from cache or repository.
        /// </summary>
        /// <param name="sessionId">The Session Id.</param>
        /// <returns>Tuple: gameSession (null if not found), fromCache (true if from cache).</returns>
        Task<(GameSession? gameSession, bool fromCache)> GetSessionAsync(string sessionId);
    }
}
