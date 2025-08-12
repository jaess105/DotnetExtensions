namespace Extensions.Threading.Tasks;

public static class TaskUtil
{
    /// <summary>
    /// Processes a sequence of inputs by creating and executing asynchronous tasks in parallel,
    /// with a fixed maximum number of concurrent tasks. Tasks are executed in batches, and the
    /// next task is started as soon as one finishes, until all inputs have been processed.
    /// </summary>
    /// <typeparam name="TIn">The type of the input values used to create tasks.</typeparam>
    /// <typeparam name="TOut">The type of the results returned by each task.</typeparam>
    /// <param name="taskFactory">
    /// A function that, given an input value of type <typeparamref name="TIn"/>, returns a
    /// <see cref="Func{Task}"/> producing a task that yields a result of type <typeparamref name="TOut"/>.
    /// This allows deferring task creation until execution time.
    /// </param>
    /// <param name="input">The sequence of input values to be processed.</param>
    /// <param name="batchSize">
    /// The maximum number of tasks to run concurrently. Defaults to 3.
    /// </param>
    /// <param name="cancellationToken">
    /// A token that can be used to cancel the batch processing. If cancellation is requested,
    /// an <see cref="OperationCanceledException"/> is thrown.
    /// </param>
    /// <returns>
    /// A list containing the results of all tasks, in the order they complete.
    /// </returns>
    /// <remarks>
    /// <para>
    /// This method implements a "task pool" pattern:
    /// - It fills the pool with up to <paramref name="batchSize"/> tasks.
    /// - When a task completes, its result is added to the output list and a new task is started,
    ///   until all inputs are exhausted.
    /// - Remaining tasks are awaited at the end.
    /// </para>
    /// <para>
    /// The results are collected in order of completion, not in the original input order.
    /// </para>
    /// <para>
    /// Any exceptions thrown by individual tasks will be propagated immediately when that task completes.
    /// </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="taskFactory"/> or <paramref name="input"/> is null.</exception>
    /// <exception cref="OperationCanceledException">Thrown if cancellation is requested via <paramref name="cancellationToken"/>.</exception>
    public static async Task<List<TOut>> ProcessInBatch<TIn, TOut>(this Func<TIn, Func<Task<TOut>>> taskFactory,
        IEnumerable<TIn> input, int batchSize = 3, CancellationToken cancellationToken = default)
    {
        List<Func<Task<TOut>>> queries = [.. input.Select(taskFactory)];
        List<Task<TOut>> jobs = new(batchSize);
        List<TOut> fetchResults = new(queries.Count);
        Task<TOut> finishedTask;

        while (queries.Count is not 0)
        {
            while (jobs.Count < batchSize)
            {
                // job is started
                jobs.Add(queries[^1]());
                queries.RemoveAt(queries.Count - 1);
            }

            finishedTask = await Task.WhenAny(jobs).WaitAsync(cancellationToken);
            // We can access the result directly, as we know the task is finished
            fetchResults.Add(finishedTask.Result);
            jobs.Remove(finishedTask);
        }

        fetchResults.AddRange(await Task.WhenAll(jobs).WaitAsync(cancellationToken));
        return fetchResults;
    }

}