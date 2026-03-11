namespace GameSessionService.Repositories
{
    using GameSessionService.Models;

    /// <summary>
    /// Interface for managing game sessions. 
    /// This repository provides methods to add new game sessions, 
    /// retrieve a session by its unique ID, and get all active sessions.
    /// </summary>
    public interface ISessionRepository
    {

        /// <summary>
        /// Adds a new GameSession to the database.
        /// </summary>
        /// <param name="session">The game session.</param>
        /// <returns>Returns the newly created game session.</returns>
        Task<GameSession> AddSessionAsync(GameSession session);


        /// <summary>
        /// Gets a game session by its unique session ID.
        /// </summary>
        /// <param name="sessionId">The game session Id.</param>
        /// <returns>Returns a game session.</returns>
        Task<GameSession?> GetByIdAsync(string sessionId);

        /// <summary>
        /// Gets all active GameSessions. Active sessions are those that have a status of "Active".
        /// </summary>
        /// <returns>Returns all active game sessions.</returns>
        Task<IEnumerable<GameSession>> GetAllSessionsAsync();
    }
}
