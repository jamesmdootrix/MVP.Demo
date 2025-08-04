namespace MVP.Common.Interfaces.Views;

/// <summary>
/// Represents a base interface for all Views in the MVP architecture.
/// 
/// This serves as a common marker interface that allows Presenters to interact with Views
/// in a decoupled and platform-agnostic manner. By using a shared IView interface,
/// presenters can work with any View implementation without depending on specific UI frameworks.
/// 
/// Usage:
/// - All View interfaces should extend IView to maintain a consistent structure.
/// - Allows Presenters to define generic interactions with Views.
/// </summary>
public interface IView
{
}
