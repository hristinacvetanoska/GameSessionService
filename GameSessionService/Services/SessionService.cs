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
        private static readonly SemaphoreSlim _sessionLock = new(1, 1);

        public SessionService(
            ISessionRepository repository,
            IMemoryCache cache,
            ILogger<SessionService> logger)
        {
            _repository = repository;
            _cache = cache;
            _logger = logger;
        }

        /// <summary>
        /// Starts a new game session for a player. 
        /// If a session already exists for the same player and game,
        /// it returns the existing session instead of creating a new one.
        /// This ensures idempotency and prevents duplicate sessions.
        /// Uses SemaphoreSlim to prevent race conditions under concurrent requests.
        /// </summary>
        /// <param name="request">The session request.</param>
        /// <returns>Returns session response details.</returns>
        public async Task<StartSessionResponseDto> StartSessionAsync(StartSessionRequestDto request)
        {
            var correlationId = Guid.NewGuid().ToString();

            await _sessionLock.WaitAsync();
            try
            {
                var existingSessions = await _repository.GetAllSessionsAsync();
                var existing = existingSessions.FirstOrDefault(x => x.PlayerId == request.PlayerId
                                                                  && x.GameId == request.GameId);

                if (existing != null)
                {
                    _logger.LogInformation(
                        "Duplicate session found. CorrelationId={CorrelationId}, PlayerId={PlayerId}, SessionId={SessionId}",
                        correlationId, existing.PlayerId, existing.SessionId
                    );

                    return new StartSessionResponseDto
                    {
                        SessionId = existing.SessionId,
                        StartedAt = existing.StartedAt,
                        Status = existing.Status
                    };
                }

                var newSession = new GameSession
                {
                    SessionId = Guid.NewGuid().ToString(),
                    PlayerId = request.PlayerId,
                    GameId = request.GameId,
                    StartedAt = DateTime.UtcNow,
                    Status = "Active"
                };

                await _repository.AddSessionAsync(newSession);

                _logger.LogInformation("Session started. CorrelationId={CorrelationId}, PlayerId={PlayerId}, SessionId={SessionId}",
                                       correlationId, newSession.PlayerId, newSession.SessionId);

                return new StartSessionResponseDto
                {
                    SessionId = newSession.SessionId,
                    StartedAt = newSession.StartedAt,
                    Status = newSession.Status
                };
            }
            finally
            {
                _sessionLock.Release();
            }
        }

        /// <summary>
        /// Gets a game session by its ID. 
        /// It first checks the in-memory cache for the session,
        /// then calls the repository if not found in cache. 
        /// If the session is retrieved from the repository, it is added to the cache for future requests. 
        /// The method returns a tuple indicating whether the session was retrieved from cache or repository.
        /// </summary>
        /// <param name="sessionId">The Session Id.</param>
        /// <returns>Tuple: gameSession (null if not found), fromCache (true if from cache).</returns>
        public async Task<(GameSession? gameSession, bool fromCache)> GetSessionAsync(string sessionId)
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

            var session = await _repository.GetByIdAsync(sessionId);

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
