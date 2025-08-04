namespace MVP.Common.Interfaces.Views;

/// <summary>
/// Defines the user interaction actions (UI event handlers) that the View (Activity or ViewController)
/// must implement to handle user-triggered events such as button clicks.
///
/// These methods are called by the UI layer (e.g., button click handlers) and delegate user actions
/// to the Presenter by wrapping the calls in interceptors (like Invoke()) for centralized error handling.
///
/// Note:
/// - These methods are NOT called by the Presenter.
/// - The Presenter depends only on <see cref="IMPVView"/> for rendering outputs.
/// - This interface is for the View's internal UI wiring and user interaction logic.
/// </summary>
public interface IMPVViewActions
{
    void OnHandledErrorButtonClicked();
    void OnNullReferenceButtonClicked();
    void OnInvalidOperationButtonClicked();
    void OnBadArgumentButtonClicked();
    void OnServiceFailureButtonClicked();
    void OnGetDataButtonClicked();
    Task OnAsyncApiFailureButtonClicked();
}

