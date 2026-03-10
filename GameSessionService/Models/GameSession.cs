namespace GameSessionService.Models
{
    public class GameSession
    {
        public string SessionId { get; set; }
        public string PlayerId { get; set; }
        public string GameId { get; set; }
        public DateTime StartedAt { get; set; }
        public string Status { get; set; }
    }
}
