using System.ComponentModel.DataAnnotations;

namespace GameSessionService.Models
{
    /// <summary>
    /// GameSession represents a game session in the system.
    /// It contains information about the session such as the unique session ID,
    /// the player ID, the game ID, the time the session was started,
    /// and the current status of the session.
    /// </summary>
    public class GameSession
    {
        /// <summary>
        /// SessionId is a unique identifier for each game session.
        /// </summary>
        [Key]
        public string SessionId { get; set; }

        /// <summary>
        /// PlayerId is the unique identifier for the player associated with this game session.
        /// </summary>
        public string PlayerId { get; set; }

        /// <summary>
        /// GameId is the unique identifier for the game associated with this session.
        /// </summary>
        public string GameId { get; set; }

        /// <summary>
        /// StartedAt indicates the date and time when the game session was started.
        /// </summary>
        public DateTime StartedAt { get; set; }

        /// <summary>
        /// Status indicates the current status of the game session.
        /// </summary>
        public string Status { get; set; }
    }
}
