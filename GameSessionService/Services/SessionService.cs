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

        public StartSessionResponseDto StartSession(StartSessionRequestDto request)
        {
            var correlationId = Guid.NewGuid().ToString();

            //check if there is an active session for the same player and game
            var existing = _repository.GetAllSessions()
                .FirstOrDefault(x => x.PlayerId == request.PlayerId
                                  && x.GameId == request.GameId
                                  && x.Status == "Active");

            if (existing != null)
            {
                _logger.LogInformation(
                    "Duplicate session found. CorrelationId={CorrelationId}, PlayerId={PlayerId}, SessionId={SessionId}",
                    correlationId, existing.PlayerId, existing.SessionId
                );

                //Return existing session details instead of creating a new one
                return new StartSessionResponseDto
                {
                    SessionId = existing.SessionId,
                    StartedAt = existing.StartedAt,
                    Status = existing.Status
                };
            }

            //Create new session
            var newSession = new GameSession
            {
                SessionId = Guid.NewGuid().ToString(),
                PlayerId = request.PlayerId,
                GameId = request.GameId,
                StartedAt = DateTime.UtcNow,
                Status = "Active"
            };

            _repository.TryAddSession(newSession);

            _logger.LogInformation("Session started. CorrelationId={CorrelationId}, PlayerId={PlayerId}, SessionId={SessionId}",
                                   correlationId, newSession.PlayerId, newSession.SessionId);

            return new StartSessionResponseDto
            {
                SessionId = newSession.SessionId,
                StartedAt = newSession.StartedAt,
                Status = newSession.Status
            };
        }

        public (GameSession? gameSession, bool fromCache) GetSession(string sessionId)
        {
            var correlationId = Guid.NewGuid().ToString();

            if (_cache.TryGetValue(sessionId, out GameSession cached))
            {
                _logger.LogInformation(
                    "Session retrieved from cache. CorrelationId={CorrelationId}, SessionId={SessionId}",
                    correlationId, cached.SessionId
                );
                return (cached, true);
            }

            var session = _repository.GetById(sessionId);

            if (session != null)
            {
                _cache.Set(sessionId, session, TimeSpan.FromSeconds(60));
                _logger.LogInformation(
                    "Session retrieved from repository and cached. CorrelationId={CorrelationId}, SessionId={SessionId}",
                    correlationId, session.SessionId
                );
            }

            return (session, false);
        }
    }
}
