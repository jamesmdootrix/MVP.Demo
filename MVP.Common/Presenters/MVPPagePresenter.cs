using MVP.Common.Interfaces.Presenters;
using MVP.Common.Interfaces.Views;
using MVP.Common.Invokers;
using MVP.Common.Presenters;

namespace MVP.Common;

/// <summary>
/// A concrete implementation of the IMVPPagePresenter interface, responsible for handling
/// the presentation logic in an MVP architecture.
/// 
/// This presenter manages the interaction between the View (IMPVView) and the underlying logic,
/// exposing various demo methods to simulate handled and unhandled errors, service failures,
/// and data retrieval. These methods are designed to demonstrate how the Presenter can coordinate
/// UI updates and error handling in a decoupled and testable manner.
/// 
/// Inherits from:
/// - PresenterBase: Provides View attachment and access.
/// Implements:
/// - IMVPPagePresenter: Defines specific presenter actions that the View can trigger.
/// 
/// Usage:
/// - The View will call methods like SimulateHandledError(), SimulateServiceFailure(), etc.,
///   to simulate real-world presenter logic.
/// - The presenter will handle logic execution, error handling, and instruct the View when to update.
/// </summary>
public class MVPPagePresenter : PresenterBase<IMPVView>, IMVPPagePresenter
{
    public void SimulateHandledError()
    {
        try
        {
            throw new InvalidOperationException("Simulated MVP presenter handled exception");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Handled Exception (Presenter): {ex.Message}");
            SentrySdk.CaptureException(ex);

            View!.ShowMessage("Simulated a handled error");
        }
    }

    public void SimulateNullReference()
    {
        string? value = null;
        Console.WriteLine(value.ToString());  //will throw NullReferenceException
    }

    public void SimulateInvalidOperation()
    {
        throw new InvalidOperationException("Presenter is in an invalid state.");
    }

    public void SimulateBadArgument()
    {
        throw new ArgumentException("Invalid argument passed to presenter method.");
    }

    public void SimulateServiceFailure()
    {
        throw new HttpRequestException("Failed to reach backend service.");
    }

    public async Task<string> SimulateAsyncApiFailure()
    {
        await Task.Delay(500);  // Simulate network call delay
        throw new HttpRequestException("Async API call failed due to timeout.");
    }

    public string GetData()
    {
        return "Here's some data from the Presenter";
    }

    //this will createa background thread, and will catch the error inside the thread on non awaited Task.Run's
    //and log to sentry.
    //if task.run is awaited, then just use regular task.run, no need for SafeTaskRunner.Run
    public void SimulateBackgroundThreadNullReference()
    {
        SafeTaskRunner.Run(() =>
        {
            //simulate long running background thread
            Thread.Sleep(3000);
            // Background task logic
            throw new NullReferenceException("Caught and bubbled up to caller");
        });
    }

    //this will be caught by sentry as it is awaited
    public async Task SimulateBackgroundThreadNullReferenceWillCatch()
    {
        await Task.Run(() =>
        {
            //simulate long running background thread
            Thread.Sleep(3000);
            // Background task logic
            throw new NullReferenceException("Caught and bubbled up to caller");
        });
    }

    //sentry will not pick up this crash
    public void SimulateBackgroundThreadUnhandledNullReference()
    {
        Task.Run(() => throw new NullReferenceException("This won't be caught"));
    }
}