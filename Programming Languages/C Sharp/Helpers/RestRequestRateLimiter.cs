using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace SmartStartWebAPI.Business.Helpers
{
    /// <summary>
    /// 1. Multi-Threaded Support: The semaphore enforces a global rate limit across all threads/tasks, ensuring compliance even in multi-threaded environments.
    /// 2. Burst Handling Within Limits: A queue lets you process multiple requests quickly within a smaller time frame as long as the total requests stay within the limit for the defined interval.
    /// 3. Flexibility for Dynamic Limits A queue can adapt to changes in the rate limit more naturally without recalculating intervals for each request.
    /// 
    /// // Example usage
    /// var rateLimiter = new RateLimiter(1, TimeSpan.FromMilliseconds(250)); // 1 request every 250ms
    /// 
    /// var client = new RestClient("https://api.example.com");
    /// var request = new RestRequest("endpoint");
    /// 
    /// for (int i = 0; i< 10; i++)
    /// {
    /// await rateLimiter.WaitAsync(); // Ensures rate limiting
    /// var response = await client.ExecuteAsync(request);
    /// Console.WriteLine(response.Content);
    /// }
    /// </summary>
    public class RestRequestRateLimiter
    {
        private readonly int _requestsPerInterval;
        private readonly TimeSpan _interval;
        private readonly ConcurrentQueue<DateTime> _requestTimestamps;
        private readonly SemaphoreSlim _semaphore;

        public RestRequestRateLimiter(int requestsPerInterval, TimeSpan interval)
        {
            _requestsPerInterval = requestsPerInterval;
            _interval = interval;
            _requestTimestamps = new ConcurrentQueue<DateTime>();
            _semaphore = new SemaphoreSlim(requestsPerInterval, requestsPerInterval);
        }

        public async Task WaitAsync()
        {
            while (true)
            {
                lock (_requestTimestamps)
                {
                    var now = DateTime.UtcNow;

                    // Remove timestamps older than the interval
                    while (_requestTimestamps.TryPeek(out var timestamp) && (now - timestamp) > _interval)
                    {
                        _requestTimestamps.TryDequeue(out _);
                        _semaphore.Release();
                    }

                    // Check if we can proceed
                    if (_semaphore.CurrentCount > 0)
                    {
                        _semaphore.Wait();
                        _requestTimestamps.Enqueue(now);
                        return;
                    }
                }

                // Wait a bit before checking again
                await Task.Delay(50); // 50ms is small enough to avoid delays but not overload the CPU
            }
        }
    }
}