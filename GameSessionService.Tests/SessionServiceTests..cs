namespace GameSessionService.Tests
{
    using GameSessionService.DTOs;
    using GameSessionService.Models;
    using GameSessionService.Repositories;
    using GameSessionService.Services;
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;

    /// <summary>
    /// Tests for the SessionService class.
    /// </summary>
    public class SessionServiceTests
    {
        private readonly Mock<ISessionRepository> _repositoryMock;
        private readonly IMemoryCache _cache;
        private readonly Mock<ILogger<SessionService>> _loggerMock;
        private readonly SessionService _service;

        public SessionServiceTests()
        {
            _repositoryMock = new Mock<ISessionRepository>();
            _cache = new MemoryCache(new MemoryCacheOptions());
            _loggerMock = new Mock<ILogger<SessionService>>();

            _service = new SessionService(
                _repositoryMock.Object,
                _cache,
                _loggerMock.Object);
        }

        [Fact]
        public async Task StartSessionAsync_ShouldCreateSession_WhenNoExistingSession()
        {
            var request = new StartSessionRequestDto
            {
                PlayerId = "P123",
                GameId = "G100"
            };

            _repositoryMock
                .Setup(r => r.GetAllSessionsAsync())
                .ReturnsAsync(new List<GameSession>());

            _repositoryMock
                .Setup(r => r.AddSessionAsync(It.IsAny<GameSession>()))
                .ReturnsAsync((GameSession s) => s);

            var result = await _service.StartSessionAsync(request);

            Assert.NotNull(result);
            Assert.Equal("Active", result.Status);

            _repositoryMock.Verify(r => r.AddSessionAsync(It.IsAny<GameSession>()), Times.Once);
        }

        [Fact]
        public async Task StartSessionAsync_ShouldReturnExistingSession_WhenSessionExists()
        {
            var existing = new GameSession
            {
                SessionId = "S1",
                PlayerId = "P123",
                GameId = "G100",
                StartedAt = DateTime.UtcNow,
                Status = "Active"
            };

            _repositoryMock
                .Setup(r => r.GetAllSessionsAsync())
                .ReturnsAsync(new List<GameSession> { existing });

            var request = new StartSessionRequestDto
            {
                PlayerId = "P123",
                GameId = "G100"
            };

            var result = await _service.StartSessionAsync(request);

            Assert.Equal(existing.SessionId, result.SessionId);

            _repositoryMock.Verify(r => r.AddSessionAsync(It.IsAny<GameSession>()), Times.Never);
        }

        [Fact]
        public async Task GetSessionAsync_ShouldReturnFromRepository_WhenCacheMiss()
        {
            var session = new GameSession
            {
                SessionId = "S1",
                PlayerId = "P123",
                GameId = "G100",
                StartedAt = DateTime.UtcNow,
                Status = "Active"
            };

            _repositoryMock
                .Setup(r => r.GetByIdAsync("S1"))
                .ReturnsAsync(session);

            var result = await _service.GetSessionAsync("S1");

            Assert.False(result.fromCache);
            Assert.Equal(session.SessionId, result.gameSession.SessionId);
        }

        [Fact]
        public async Task GetSessionAsync_ShouldReturnFromCache_WhenSessionCached()
        {
            var session = new GameSession
            {
                SessionId = "S1",
                PlayerId = "P123",
                GameId = "G100",
                StartedAt = DateTime.UtcNow,
                Status = "Active"
            };

            _cache.Set("S1", session);

            var result = await _service.GetSessionAsync("S1");

            Assert.True(result.fromCache);
            Assert.Equal("S1", result.gameSession.SessionId);
        }

        [Fact]
        public async Task StartSessionAsync_ShouldCreateOnlyOneSession_WhenCalledConcurrently()
        {
            var request = new StartSessionRequestDto
            {
                PlayerId = "P123",
                GameId = "G100"
            };

            var sessions = new List<GameSession>();

            _repositoryMock
                .Setup(r => r.GetAllSessionsAsync())
                .ReturnsAsync(() => sessions);

            _repositoryMock
                .Setup(r => r.AddSessionAsync(It.IsAny<GameSession>()))
                .Callback<GameSession>(s => sessions.Add(s))
                .ReturnsAsync((GameSession s) => s);

            var tasks = new List<Task<StartSessionResponseDto>>();

            for (int i = 0; i < 10; i++)
            {
                tasks.Add(_service.StartSessionAsync(request));
            }

            await Task.WhenAll(tasks);

            Assert.Single(sessions);
        }

        [Fact]
        public async Task GetSessionAsync_ShouldReturnNull_WhenSessionDoesNotExist()
        {
            _repositoryMock
                .Setup(r => r.GetByIdAsync("S1"))
                .ReturnsAsync((GameSession?)null);

            var result = await _service.GetSessionAsync("S1");

            Assert.Null(result.gameSession);
            Assert.False(result.fromCache);
        }
    }
}