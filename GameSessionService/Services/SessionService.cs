namespace GameSessionService.Services
{
    using GameSessionService.DTOs;
    using GameSessionService.Models;
    using GameSessionService.Repositories;
    using Microsoft.Extensions.Caching.Memory;

    public class SessionService : ISessionService
    {
        private readonly ISessionRepository _repository;
        private readonly IMemoryCache _cache;
        private readonly ILogger<SessionService> _logger;

        public SessionService(
            ISessionRepository repository,
            IMemoryCache cache,
            ILogger<SessionService> logger)
        {
            _repository = repository;
            _cache = cache;
            _logger = logger;
        }

        public async Task<StartSessionResponse> StartSessionAsync(StartSessionRequestDto request)
        {
            var session = new GameSession
            {
                SessionId = Guid.NewGuid().ToString(),
                PlayerId = request.PlayerId,
                GameId = request.GameId,
                StartedAt = DateTime.UtcNow,
                Status = "Active"
            };

            await _repository.SaveAsync(session);

            _logger.LogInformation("Session started {SessionId} for player {PlayerId}",
                session.SessionId, session.PlayerId);

            return new StartSessionResponse
            {
                SessionId = session.SessionId,
                StartedAt = session.StartedAt,
                Status = session.Status
            };
        }

        public async Task<(GameSession? gameSession, bool fromCache)> GetSessionAsync(string sessionId)
        {
            if (_cache.TryGetValue(sessionId, out GameSession cached))
            {
                return (cached, true);
            }

            var session = await _repository.GetByIdAsync(sessionId);

            if (session != null)
            {
                _cache.Set(sessionId, session, TimeSpan.FromSeconds(60));
            }

            return (session, false);
        }
    }
}
