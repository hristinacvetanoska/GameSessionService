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
        /// Gets all active GameSessions. Active sessions are those that have a status of "Active".
        /// </summary>
        /// <returns>Returns all active game sessions.</returns>
        public async Task<IEnumerable<GameSession>> GetAllSessionsAsync()
        {
            return await _context.GameSessions
                .Where(x => x.Status == "Active")
                .ToListAsync();
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
    }
}
