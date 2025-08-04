using System;
using MVP.Common.Interceptors;

namespace MVP.Android.Activities;

/// <summary>
/// A base Activity that provides a unified way to invoke presenter actions
/// with error interception and contextual logging.
/// It uses <see cref="PresenterInvoker"/> to ensure exceptions are properly captured
/// and can be extended by other activities to simplify error handling.
///
/// Usage:
/// - Inherit from ActivityBase in your Activities.
/// - Set the Invoker property (inject via DI or set manually).
/// - Use Invoke(action, context) or InvokeAsync(func, context) methods to execute presenter logic with built-in error interception.
/// </summary>
public class ActivityBase : Activity
{
    protected PresenterInvoker Invoker { get; private set; } = default!;

    public void SetInvoker(PresenterInvoker invoker)
    {
        Invoker = invoker ?? throw new ArgumentNullException(nameof(invoker));
    }

    public InvocationResult Invoke(Action action, string title)
    {
        if (Invoker == null)
            throw new InvalidOperationException("Invoker has not been set. Call SetInvoker() before invoking actions.");

        return Invoker.Invoke(action, title);
    }

    public InvocationResult<T> Invoke<T>(Func<T> func, string title)
    {
        if (Invoker == null)
            throw new InvalidOperationException("Invoker has not been set. Call SetInvoker() before invoking actions.");

        return Invoker.Invoke(func, title);
    }

    public Task<InvocationResult> InvokeAsync(Func<Task> func, string title)
    {
        if (Invoker == null)
            throw new InvalidOperationException("Invoker has not been set. Call SetInvoker() before invoking actions.");

        return Invoker.InvokeAsync(func, title);
    }

    public Task<InvocationResult<T>> InvokeAsync<T>(Func<Task<T>> func, string title)
    {
        if (Invoker == null)
            throw new InvalidOperationException("Invoker has not been set. Call SetInvoker() before invoking actions.");

        return Invoker.InvokeAsync(func, title);
    }
}

