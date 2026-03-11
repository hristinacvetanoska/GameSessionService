namespace GameSessionService.Controllers
{
    using GameSessionService.Services;
    using Microsoft.AspNetCore.Mvc;
    using System.Diagnostics;

    /// <summary>
    /// Provides diagnostic endpoints for performance testing.
    /// </summary>
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

        /// <summary>
        /// Runs a performance test by calling GetSession multiple times.
        /// Measures total and average execution time.
        /// </summary>
        /// <param name="iterations">Number of repeated calls (default 1000).</param>
        /// <returns>Timing summary: total ms, average ms, iterations.</returns>
        [HttpGet("perf-test")]
        public async Task<IActionResult> PerfTest(int iterations = 1000)
        {
            _logger.LogInformation("Perf-test started with {Iterations} iterations", iterations);

            var sw = Stopwatch.StartNew();

            for (int i = 0; i < iterations; i++)
            {
               await _service.GetSessionAsync("test-session");
            }

            sw.Stop();


            _logger.LogInformation("Perf-test finished. TotalMs={TotalMs}, Iterations={Iterations}",
                sw.ElapsedMilliseconds, iterations);

            return Ok(new
            {
                Iterations = iterations,
                TotalMs = sw.ElapsedMilliseconds,
                AvgMs = sw.ElapsedMilliseconds / (double)iterations
            });
        }
    }
}
