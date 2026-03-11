namespace GameSessionService.Repositories
{
    using GameSessionService.Models;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Concurrent;

    /// <summary>
    /// The session repository is responsible for managing game sessions. 
    /// It provides methods to add new game sessions, retrieve a session by its unique ID, 
    /// and get all active sessions. 
    /// </summary>
    public class SessionRepository : ISessionRepository
    {
        private readonly GameSessionContext _context;

        public SessionRepository(GameSessionContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets a game session by its unique session ID.
        /// </summary>
        /// <param name="sessionId">The game session Id.</param>
        /// <returns>Returns a game session.</returns>
        public async Task<GameSession?> GetByIdAsync(string sessionId)
        {
            return await _context.GameSessions.FirstOrDefaultAsync(x => x.SessionId == sessionId);
        }

        /// <summary>
        /// Adds a new GameSession to the database.
        /// </summary>
        /// <param name="session">The game session.</param>
        /// <returns>Returns the newly created game session.</returns>
        public async Task<GameSession> AddSessionAsync(GameSession session)
        {
            _context.GameSessions.Add(session);
            await _context.SaveChangesAsync();
            return session;
        }

        /// <summary>
        /// Gets a game session by player ID and game ID. 
        /// This method is useful for retrieving an active session for a specific player and game combination.
        /// </summary>
        /// <param name="playerId">The player id.</param>
        /// <param name="gameId">The game id.</param>
        /// <returns>Returns game session.</returns>
        public async Task<GameSession?> GetByPlayerAndGameAsync(string playerId, string gameId)
        {
            return await _context.GameSessions
                .FirstOrDefaultAsync(x => x.PlayerId == playerId && x.GameId == gameId && x.Status == "Active");
        }
    }
}
