namespace GameSessionService.DTOs
{
    /// <summary>
    /// StartSessionResponseDto represents the response data returned after attempting to start a game session.
    /// </summary>
    public class StartSessionResponseDto
    {
        /// <summary>
        /// SessionId is a unique identifier for the game session.
        /// </summary>
        public string SessionId { get; set; }

        /// <summary>
        /// StartedAt indicates when the game session was started.
        /// </summary>
        public DateTime StartedAt { get; set; }

        /// <summary>
        /// Status indicates the current status of the game session.
        /// </summary>
        public string Status { get; set; }
    }
}
