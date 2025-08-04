using Sentry;

namespace MVP.Common.Interceptors;

/// <summary>
/// Provides a centralised way to invoke actions and functions within presenters,
/// allowing errors to be intercepted, logged, and optionally passed back to the caller for handling.
/// This acts as a middleman between UI events and presenter logic,
/// ensuring that unhandled exceptions are captured (e.g., by Sentry) with optional context for easier debugging.
///
/// Usage:
/// - Wrap presenter method calls inside Invoke() or InvokeAsync() to intercept errors.
/// - The caller receives an InvocationResult object indicating success or failure.
/// - InvocationResult contains the exception (if any) and, for functions, the return value.
/// - Allows the View to react to presenter errors without crashing the app.
/// 
/// Example (Sync):
/// var result = _invoker.Invoke(() => _presenter.DoSomething(), "Presenter Action");
/// if (!result.IsSuccess)
/// {
///     ShowError(result.Exception?.Message);
/// }
///
/// Example (Async):
/// var result = await _invoker.InvokeAsync(() => _presenter.DoSomethingAsync(), "Async Action");
/// if (!result.IsSuccess)
/// {
///     ShowError(result.Exception?.Message);
/// }
/// </summary>
public class PresenterInvoker
{
    public InvocationResult Invoke(Action action, string context = null!)
    {
        try
        {
            action();
            return new InvocationResult { IsSuccess = true };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Intercepted Error] {context ?? "Action"} failed: {ex.Message}");
            SentrySdk.CaptureException(ex);
            return new InvocationResult { IsSuccess = false, Exception = ex };
        }
    }

    public InvocationResult<T> Invoke<T>(Func<T> func, string context = null!)
    {
        try
        {
            return new InvocationResult<T> { IsSuccess = true, Value = func() };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Intercepted Error] {context ?? "Function"} failed: {ex.Message}");
            SentrySdk.CaptureException(ex);
            return new InvocationResult<T> { IsSuccess = false, Exception = ex };
        }
    }

    public async Task<InvocationResult> InvokeAsync(Func<Task> func, string context = null!)
    {
        try
        {
            await func();
            return new InvocationResult { IsSuccess = true };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Intercepted Error] {context ?? "Async Action"} failed: {ex.Message}");
            SentrySdk.CaptureException(ex);
            return new InvocationResult { IsSuccess = false, Exception = ex };
        }
    }

    public async Task<InvocationResult<T>> InvokeAsync<T>(Func<Task<T>> func, string context = null!)
    {
        try
        {
            var result = await func();
            return new InvocationResult<T> { IsSuccess = true, Value = result };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Intercepted Error] {context ?? "Async Function"} failed: {ex.Message}");
            SentrySdk.CaptureException(ex);
            return new InvocationResult<T> { IsSuccess = false, Exception = ex };
        }
    }
}

/// <summary>
/// Represents the result of an invoked action, indicating whether it succeeded,
/// and providing exception details if it failed.
/// </summary>
public class InvocationResult
{
    public bool IsSuccess { get; set; }
    public Exception? Exception { get; set; }
}

/// <summary>
/// Represents the result of an invoked function that returns a value,
/// including success status, the return value (if successful), and exception details (if failed).
/// </summary>
public class InvocationResult<T> : InvocationResult
{
    public T? Value { get; set; }
}