namespace GameSessionService.Services
{
    using GameSessionService.DTOs;
    using GameSessionService.Models;

    public interface ISessionService
    {
        Task<StartSessionResponse> StartSessionAsync(StartSessionRequestDto request);
        Task<(GameSession? gameSession, bool fromCache)> GetSessionAsync(string sessionId);
    }
}
