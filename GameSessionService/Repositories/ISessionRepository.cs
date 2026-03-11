namespace GameSessionService.Repositories
{
    using GameSessionService.Models;

    public interface ISessionRepository
    {
        /// <summary>
        /// Adds a new GameSession to the repository.
        /// </summary>
        /// <param name="session">The game session.</param>
        /// <returns> Returns true if the session was added successfully, or false if a session with the same SessionId already exists.</returns>
        bool TryAddSession(GameSession session);
        //Task SaveAsync(GameSession session);

        /// <summary>
        /// Gets a GameSession by its sessionId.
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns>Returns the GameSession with the specified sessionId, or null if not found.</returns>
        GameSession? GetById(string sessionId);

        /// <summary>
        /// Gets all active GameSessions. Active sessions are those that have a status of "Active".
        /// </summary>
        /// <returns> Returns a list of all active GameSessions.</returns>
        IEnumerable<GameSession> GetAllSessions();
    }
}
