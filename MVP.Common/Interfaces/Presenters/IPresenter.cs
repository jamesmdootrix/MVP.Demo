using MVP.Common.Interfaces.Views;

namespace MVP.Common.Interfaces.Presenters;

/// <summary>
/// Defines a base interface for all Presenter classes in the MVP architecture.
/// A presenter implementing this interface is responsible for managing the connection to a View,
/// allowing the View to be attached and providing access to it.
/// This ensures a consistent way for Presenters to interact with their corresponding Views,
/// while keeping the presenter logic decoupled from platform-specific UI code.
///
/// Usage:
/// - Implement IPresenter<TView> in any presenter that should manage a View.
/// - Use AttachView() to establish the View reference.
/// - Access the View via the View property (nullable).
///
/// Example:
/// public class SamplePresenter : IPresenter<ISampleView> { ... }
/// </summary>
public interface IPresenter<TView> where TView : IView
{
    void AttachView(TView view);
    TView? View { get; }
}
