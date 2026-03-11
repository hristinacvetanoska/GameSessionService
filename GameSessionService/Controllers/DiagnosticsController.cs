namespace GameSessionService.Controllers
{
    using GameSessionService.Services;
    using Microsoft.AspNetCore.Mvc;
    using System.Diagnostics;

    [ApiController]
    [Route("diagnostics")]
    public class DiagnosticsController : ControllerBase
    {
        private readonly ISessionService _service;
        private readonly ILogger<DiagnosticsController> _logger;

        public DiagnosticsController(ISessionService service, ILogger<DiagnosticsController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet("perf-test")]
        public async Task<IActionResult> PerfTest(int iterations = 1000)
        {
            _logger.LogInformation("Perf-test started with {Iterations} iterations", iterations);

            var sw = Stopwatch.StartNew();

            for (int i = 0; i < iterations; i++)
            {
                _service.GetSession("test-session");
            }

            sw.Stop();


            _logger.LogInformation("Perf-test finished. TotalMs={TotalMs}, Iterations={Iterations}",
                sw.ElapsedMilliseconds, iterations);

            return Ok(new
            {
                Iterations = iterations,
                TotalMs = sw.ElapsedMilliseconds, // total time in ms for all iterations
                AvgMs = sw.ElapsedMilliseconds / (double)iterations // average time per call
            });
        }
    }
}
