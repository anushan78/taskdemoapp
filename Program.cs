using System;
using System.Threading;
using System.Threading.Tasks;

Console.WriteLine("Starting the application...");

// Create a CancellationTokenSource with a 10-second timeout
// This will automatically cancel the token after 10 seconds
// in this case both jobs beklow complete before the timeout (1.5s plus 2s less than 10s)
using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));

try
{
  var result = await SimulateJobAsync(jobId: 1, delayMs: 1500, token: cts.Token);
  Console.WriteLine($"Job result: {result}");

  // Simulate user cancellation
  // cts.Cancel();

  var userName = await GetUserNameAsync(userId: 42, token: cts.Token);
  Console.WriteLine($"Fetched user name: {userName}");
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

static async Task<string> GetUserNameAsync(int userId, CancellationToken token)
{
  // Simulate fetching user name with cancellation support
  token.ThrowIfCancellationRequested();

  await Task.Delay(2000, token);
  return $"User name for ID {userId}";
}