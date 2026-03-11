using GameSessionService.DTOs;
using GameSessionService.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GameSessionService.Controllers
{
    [Route("sessions")]
    [ApiController]
    public class SessionsController : ControllerBase
    {
        private readonly ISessionService _service;

        public SessionsController(ISessionService service)
        {
            _service = service;
        }

        [HttpPost("start")]
        public async Task<IActionResult> StartSession(StartSessionRequestDto request)
        {
            var result = await _service.StartSessionAsync(request);
            return Ok(result);
        }

        [HttpGet("{sessionId}")]
        public async Task<IActionResult> GetSession(string sessionId)
        {
            var result = await _service.GetSessionAsync(sessionId);

            if (result.gameSession == null)
                return NotFound();

            Response.Headers.Add("X-Cache", result.fromCache ? "Hit" : "Miss");

            return Ok(result.gameSession);
        }
    }
}
