using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SmartStartWebAPI.Business.Helpers
{
    public static class ConcurrencyLimiter
    {
        // Custom delegate type for asynchronous actions
        public delegate Task AsyncAction();

        /* Usage example:
            //Create a ConcurrentBag to store the results
            ConcurrentBag<Vehicle> YearMakeModel = new ConcurrentBag<Vehicle>();

            // Create a list to store tasks for each API call
            List<AsyncAction> apiCallFunctions = new List<AsyncAction>();
                
            foreach (var make in MakeNames)
            {
                apiCallFunctions.Add(async () =>
                {
                    Do something....
                    YearMakeModel.Add(Some Vehicle object);
                    return;
                });
            }
            await ConcurrencyLimiter.LimitExecution(5, apiCallFunctions);
         */

        /// <summary>
        /// This will restrict how my of the passed in async tasks can execute at a time. Once all tasks have been executed then LimitExecution will resolve.
        /// </summary>
        /// <param name="maxConcurrency">The maximum number of async tasks can execute at one time</param>
        /// <param name="actions">A list of async tasks that need to be executed</param>
        /// <returns>Task</returns>
        public static async Task LimitExecution(int maxConcurrency, List<AsyncAction> actions)
        {
            // Use SemaphoreSlim to control concurrency
            var semaphore = new SemaphoreSlim(maxConcurrency);

            // Create tasks for each action
            var tasks = new List<Task>();

            foreach (var action in actions)
            {
                // Wait until a semaphore slot is available
                await semaphore.WaitAsync();

                // Execute the action asynchronously
                tasks.Add(Task.Run(async () =>
                {
                    try
                    {
                        await action(); // Execute the action
                    }
                    finally
                    {
                        // Release the semaphore slot
                        semaphore.Release();
                    }
                }));
            }

            // Wait for all tasks to complete
            await Task.WhenAll(tasks);
        }
    }
}
