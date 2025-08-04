using Microsoft.Extensions.DependencyInjection;
using MVP.Android;
using MVP.Android.Activities;
using MVP.Common.Interceptors;
using MVP.Common.Interfaces.Presenters;
using MVP.Common.Interfaces.Views;
using Resource = MVP.Android.Resource;

[Activity(Label = "MVP Demo", MainLauncher = true)]
public class MainActivity :
    ActivityBase,
    IMPVView,
    IMPVViewActions
{
    private IMVPPagePresenter _presenter;
    private LinearLayout _buttonContainer;
    private ScrollView _scrollView;

    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        SetContentView(Resource.Layout.activity_main);

        _presenter = MainApplication.ServiceProvider!.GetRequiredService<IMVPPagePresenter>();
        _presenter.AttachView(this);

        var invoker = MainApplication.ServiceProvider!.GetRequiredService<PresenterInvoker>();
        SetInvoker(invoker);

        _scrollView = FindViewById<ScrollView>(Resource.Id.scrollView)!;
        _buttonContainer = FindViewById<LinearLayout>(Resource.Id.buttonContainer)!;

        AddButton("Handled Error", OnHandledErrorButtonClicked);
        AddButton("Null Reference", OnNullReferenceButtonClicked);
        AddButton("Invalid Operation", OnInvalidOperationButtonClicked);
        AddButton("Bad Argument", OnBadArgumentButtonClicked);
        AddButton("Service Failure", OnServiceFailureButtonClicked);
        AddButton("Get Data", OnGetDataButtonClicked);
        AddAsyncButton("Async API Failure", OnAsyncApiFailureButtonClicked);
    }

    private void AddButton(string text, Action action)
    {
        var button = new Button(this)
        {
            Text = text
        };
        button.Click += (sender, e) => action();
        _buttonContainer.AddView(button);
    }

    private void AddAsyncButton(string text, Func<Task> action)
    {
        var button = new Button(this)
        {
            Text = text
        };
        button.Click += async (sender, e) => await action();
        _buttonContainer.AddView(button);
    }

    // ---- View Interface Implementation (Presenter → View) ----
    public void ShowMessage(string message)
    {
        RunOnUiThread(() =>
        {
            Toast.MakeText(this, message, ToastLength.Short).Show();
        });
    }

    // ---- UI Event Handlers (ViewActions) ----
    public void OnHandledErrorButtonClicked()
    {
        var result = Invoke(_presenter.SimulateHandledError, "Handled Error");
        ShowMessage(result.IsSuccess ? "Success" : $"Error: {result.Exception?.Message ?? "Unknown Error"}");
    }

    public void OnNullReferenceButtonClicked()
    {
        var result = Invoke(_presenter.SimulateNullReference, "Null Reference");
        ShowMessage(result.IsSuccess ? "Success" : $"Error: {result.Exception?.Message ?? "Unknown Error"}");
    }

    public void OnInvalidOperationButtonClicked()
    {
        var result = Invoke(_presenter.SimulateInvalidOperation, "Invalid Operation");
        ShowMessage(result.IsSuccess ? "Success" : $"Error: {result.Exception?.Message ?? "Unknown Error"}");
    }

    public void OnBadArgumentButtonClicked()
    {
        var result = Invoke(_presenter.SimulateBadArgument, "Bad Argument");
        ShowMessage(result.IsSuccess ? "Success" : $"Error: {result.Exception?.Message ?? "Unknown Error"}");
    }

    public void OnServiceFailureButtonClicked()
    {
        var result = Invoke(_presenter.SimulateServiceFailure, "Service Failure");
        ShowMessage(result.IsSuccess ? "Success" : $"Error: {result.Exception?.Message ?? "Unknown Error"}");
    }

    public void OnGetDataButtonClicked()
    {
        var result = Invoke(_presenter.GetData, "Get Data");
        ShowMessage(result.IsSuccess ? $"Success: {result.Value}" : $"Error: {result.Exception?.Message ?? "Unknown Error"}");
    }

    public async Task OnAsyncApiFailureButtonClicked()
    {
        ShowMessage("Loading...");
        var result = await InvokeAsync(_presenter.SimulateAsyncApiFailure, "Async API Failure");
        if (result.IsSuccess)
            ShowMessage($"Success: {result.Value}");
        else
            ShowMessage($"Error: {result.Exception?.Message ?? "Unknown Error"}");
    }
}