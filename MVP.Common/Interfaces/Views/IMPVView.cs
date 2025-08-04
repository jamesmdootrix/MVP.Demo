namespace MVP.Common.Interfaces.Views;

/// <summary>
/// Defines a specific View interface that extends the base IView interface,
/// exposing View-level actions that the Presenter can invoke.
/// 
/// This interface represents the contract for a View in the MVP architecture,
/// allowing the Presenter to request UI updates in a decoupled manner.
/// 
/// By extending IView, it remains consistent with the shared View structure,
/// while adding feature-specific methods that are relevant to this particular View.
/// 
/// Usage:
/// - Implement IMPVView in platform-specific View classes (e.g., ViewControllers, Pages).
/// - The Presenter will call ShowMessage() to instruct the View to display messages to the user.
/// </summary>
public interface IMPVView : IView
{
    void ShowMessage(string value);
}