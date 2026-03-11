namespace GameSessionService.Repositories
{
    using GameSessionService.Models;

    public class SessionRepository : ISessionRepository
    {
        private static readonly Dictionary<string, GameSession> _sessions = new();

        public Task SaveAsync(GameSession session)
        {
            _sessions[session.SessionId] = session;
            return Task.CompletedTask;
        }

        public Task<GameSession?> GetByIdAsync(string sessionId)
        {
            _sessions.TryGetValue(sessionId, out var session);
            return Task.FromResult(session);
        }
    }
}
