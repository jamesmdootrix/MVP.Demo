using MVP.Common.Interceptors;

namespace MVP.IOS.ViewControllers;

/// <summary>
/// A base ViewController that provides a unified way to invoke presenter actions
/// with error interception and contextual logging. 
/// It uses <see cref="PresenterInvoker"/> to ensure that exceptions are properly captured
/// and can be extended by other view controllers to simplify error handling.
/// 
/// Usage:
/// - Inherit from ViewControllerBase in your view controllers.
/// - Use the Invoke(action, context) or InvokeAsync(func, context) methods to execute presenter logic with built-in error interception.
/// </summary>
public class ViewControllerBase(PresenterInvoker invoker) : UIViewController
{
    private readonly PresenterInvoker _invoker = invoker;

    public InvocationResult Invoke(Action action, string title)
    {
        return _invoker.Invoke(action, title);
    }

    public InvocationResult<T> Invoke<T>(Func<T> func, string title)
    {
        return _invoker.Invoke(func, title);
    }

    public Task<InvocationResult> InvokeAsync(Func<Task> func, string title)
    {
        return _invoker.InvokeAsync(func, title);
    }

    public Task<InvocationResult<T>> InvokeAsync<T>(Func<Task<T>> func, string title)
    {
        return _invoker.InvokeAsync(func, title);
    }
}