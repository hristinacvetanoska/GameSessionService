using System.ComponentModel.DataAnnotations;

namespace GameSessionService.DTOs
{
    /// <summary>
    /// StartSessionRequestDto represents the data required to start a new game session.
    /// </summary>
    public class StartSessionRequestDto
    {
        /// <summary>
        /// PlayerId is the unique identifier for the player starting the session.
        /// </summary>
        [Required(ErrorMessage = "PlayerId is required")]
        public string PlayerId { get; set; }

        /// <summary>
        /// GameId is the unique identifier for the game for which the session is being started.
        /// </summary>
        [Required(ErrorMessage = "GameId is required")]
        public string GameId { get; set; }
    }
}
