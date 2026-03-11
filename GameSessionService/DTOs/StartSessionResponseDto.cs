namespace GameSessionService.DTOs
{
    public class StartSessionResponseDto
    {
        public string SessionId { get; set; }
        public DateTime StartedAt { get; set; }
        public string Status { get; set; }
    }
}
