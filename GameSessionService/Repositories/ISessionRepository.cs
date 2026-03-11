namespace GameSessionService.Repositories
{
    using GameSessionService.Models;

    public interface ISessionRepository
    {
        Task SaveAsync(GameSession session);
        Task<GameSession?> GetByIdAsync(string sessionId);
    }
}
