namespace GameSessionService.Repositories
{
    using GameSessionService.Models;
    using System.Collections.Concurrent;

    public class SessionRepository : ISessionRepository
    {
        private static readonly ConcurrentDictionary<string, GameSession> _sessions = new();

        public GameSession? GetById(string sessionId)
        {
            _sessions.TryGetValue(sessionId, out var session);
            return session;
        }

        public bool TryAddSession(GameSession session)
        {
            return _sessions.TryAdd(session.SessionId, session);
        }

        public IEnumerable<GameSession> GetAllSessions() => _sessions.Values;
    }
}
