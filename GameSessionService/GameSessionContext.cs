using GameSessionService.Models;
using Microsoft.EntityFrameworkCore;

namespace GameSessionService
{
    /// <summary>
    /// The GameSessionContext class.
    /// </summary>
    public class GameSessionContext : DbContext
    {
        public GameSessionContext(DbContextOptions<GameSessionContext> options) : base(options) { }

        public DbSet<GameSession> GameSessions { get; set; }
    }
}
