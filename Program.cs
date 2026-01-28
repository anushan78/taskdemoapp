using System;
using System.Threading;
using System.Threading.Tasks;

Console.WriteLine("Starting the application...");

using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(1));

try
{
  var result = await SimulateJobAsync(jobId: 1, delayMs: 1500, token: cts.Token);
  Console.WriteLine($"Job result: {result}");
}
catch (Exception ex)
{
  Console.WriteLine($"Job cancelled: {ex.Message}");
}

static async Task<int> SimulateJobAsync(int jobId, int delayMs, CancellationToken token)
{
  Console.WriteLine($"Job {jobId} started (delay {delayMs} ms)");
  await Task.Delay(delayMs, token); // Cancellation-aware
  Console.WriteLine($"Job {jobId} completed");
  return jobId * 10;
}