using MVP.Common.Interceptors;
using MVP.Common.Interfaces.Presenters;
using MVP.Common.Interfaces.Views;

namespace MVP.IOS.ViewControllers;

public partial class MVPViewController :
    ViewControllerBase,
    IMPVView,
    IMPVViewActions
{
    private readonly IMVPPagePresenter _presenter;
    private UIActivityIndicatorView? _loadingIndicator;

    // Inject dependencies
    public MVPViewController(
        PresenterInvoker invoker,
        IMVPPagePresenter presenter) : base(invoker)
    {
        _presenter = presenter;
        _presenter.AttachView(this);
    }

    //UI Construction
    public override void ViewDidLoad()
    {
        base.ViewDidLoad();

        View!.BackgroundColor = UIColor.Yellow;

        var scrollView = new UIScrollView(View.Bounds)
        {
            AutoresizingMask = UIViewAutoresizing.FlexibleDimensions
        };

        _loadingIndicator = new UIActivityIndicatorView(UIActivityIndicatorViewStyle.Large)
        {
            Center = View.Center,
            HidesWhenStopped = true
        };
        View.AddSubview(_loadingIndicator);

        var buttonHeight = 50;
        var buttonWidth = 200;
        var verticalSpacing = 20;
        var startY = 100;
        var currentY = startY;

        void AddButton(string title, Action viewMethod)
        {
            var button = new UIButton(UIButtonType.System)
            {
                Frame = new CGRect(100, currentY, buttonWidth, buttonHeight)
            };
            button.SetTitle(title, UIControlState.Normal);
            button.TouchUpInside += (s, e) => viewMethod();
            scrollView.AddSubview(button);
            currentY += buttonHeight + verticalSpacing;
        }

        void AddAsyncButtonThatReturnsValue(string title, Func<Task> viewMethod)
        {
            var button = new UIButton(UIButtonType.System)
            {
                Frame = new CGRect(100, currentY, buttonWidth, buttonHeight)
            };
            button.SetTitle(title, UIControlState.Normal);
            button.TouchUpInside += async (s, e) => await viewMethod();
            scrollView.AddSubview(button);
            currentY += buttonHeight + verticalSpacing;
        }

        // Add Buttons
        AddButton("Handled Error", OnHandledErrorButtonClicked);
        AddButton("Null Reference", OnNullReferenceButtonClicked);
        AddButton("Invalid Operation", OnInvalidOperationButtonClicked);
        AddButton("Bad Argument", OnBadArgumentButtonClicked);
        AddButton("Service Failure", OnServiceFailureButtonClicked);
        AddButton("Get Data", OnGetDataButtonClicked);
        AddAsyncButtonThatReturnsValue("Async API Failure", OnAsyncApiFailureButtonClicked);

        scrollView.ContentSize = new CGSize(View.Bounds.Width, currentY + 100);
        View.AddSubview(scrollView);
    }


    //IMPVViewActions implementation
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
        _loadingIndicator?.StartAnimating();
        var result = await InvokeAsync(_presenter.SimulateAsyncApiFailure, "Async API Failure");
        _loadingIndicator?.StopAnimating();

        if (result.IsSuccess)
            ShowMessage($"Success: {result.Value}");
        else
            ShowMessage($"Error: {result.Exception?.Message ?? "Unknown Error"}");
    }

    //IMPVView implementation
    public void ShowMessage(string value)
    {
        var alert = UIAlertController.Create("Notice", value, UIAlertControllerStyle.Alert);
        alert.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
        PresentViewController(alert, true, null);
    }
}