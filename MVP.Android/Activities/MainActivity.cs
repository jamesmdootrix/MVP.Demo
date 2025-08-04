using Microsoft.Extensions.DependencyInjection;
using MVP.Android.Activities;
using MVP.Common.Interceptors;
using MVP.Common.Interfaces.Presenters;
using MVP.Common.Interfaces.Views;

namespace MVP.Android;

[Activity(Label = "MVP Demo", MainLauncher = true)]
public class MainActivity : ActivityBase, IMPVView
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

        // Set Invoker in base class
        SetInvoker(invoker);

        _scrollView = FindViewById<ScrollView>(Resource.Id.scrollView)!;
        _buttonContainer = FindViewById<LinearLayout>(Resource.Id.buttonContainer)!;


        AddButton("Handled Error", () => Invoke(_presenter.SimulateHandledError, "Handled Error"));
        AddButton("Null Reference", () => Invoke(_presenter.SimulateNullReference, "Null Reference"));
        AddButton("Invalid Operation", () => Invoke(_presenter.SimulateInvalidOperation, "Invalid Operation"));
        AddButton("Bad Argument", () => Invoke(_presenter.SimulateBadArgument, "Bad Argument"));
        AddButton("Service Failure", () => Invoke(_presenter.SimulateServiceFailure, "Service Failure"));
        AddButtonThatReturnsValue("Get Data", _presenter.GetData);
    }

    private void AddButton(string text, Action action)
    {
        var button = new Button(this)
        {
            Text = text
        };
        button.Click += (sender, e) =>
        {
            var result = Invoke(action, text);
            if (result.IsSuccess)
            {
                ShowMessage($"Success: {text} executed successfully.");
            }
            else
            {
                ShowMessage($"Error: {result.Exception?.Message ?? "Unknown Error"}");
            }
        };
        _buttonContainer.AddView(button);
    }

    private void AddButtonThatReturnsValue<T>(string text, Func<T> func)
    {
        var button = new Button(this)
        {
            Text = text
        };
        button.Click += (sender, e) =>
        {
            var result = Invoke(func, text);
            if (result.IsSuccess)
            {
                ShowMessage($"Success: {result.Value}");
            }
            else
            {
                ShowMessage($"Error: {result.Exception?.Message ?? "Unknown Error"}");
            }
        };
        _buttonContainer.AddView(button);
    }

    public void ShowMessage(string message)
    {
        RunOnUiThread(() =>
        {
            Toast.MakeText(this, message, ToastLength.Short).Show();
        });
    }
}

