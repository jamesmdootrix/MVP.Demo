using System.Security.Cryptography;
using MVP.Common.Interfaces.Views;

namespace MVP.Common.Interfaces.Presenters;

/// <summary>
/// Defines a specific presenter interface that extends the shared IPresenter interface,
/// allowing additional methods to be exposed for interaction with the View.
/// 
/// This pattern provides a structured way to define presenter contracts in an MVP architecture,
/// where the base IPresenter handles View attachment and lifecycle,
/// and extended presenter interfaces declare the specific actions or operations the View can invoke.
/// 
/// By extending IPresenter, this interface ensures consistency across presenters,
/// while allowing Views to access presenter-specific functionality through a strongly-typed contract.
/// 
/// Usage:
/// - Create feature-specific presenter interfaces by extending IPresenter.
/// - Define methods here that the corresponding View will call.
/// - Implement this interface in Presenter classes to provide the actual logic.
/// </summary>
public interface IMVPPagePresenter : IPresenter<IMPVView>
{
    void SimulateHandledError();
    void SimulateNullReference();
    void SimulateInvalidOperation();
    void SimulateBadArgument();
    void SimulateServiceFailure();
    void SimulateBackgroundThreadNullReference();
    void SimulateBackgroundThreadUnhandledNullReference();
    Task<string> SimulateAsyncApiFailure();
    string GetData();
}
