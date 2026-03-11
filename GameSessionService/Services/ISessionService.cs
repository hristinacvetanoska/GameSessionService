namespace GameSessionService.Services
{
    using GameSessionService.DTOs;
    using GameSessionService.Models;

    public interface ISessionService
    {
        StartSessionResponseDto StartSession(StartSessionRequestDto request);
        (GameSession? gameSession, bool fromCache) GetSession(string sessionId);
    }
}
