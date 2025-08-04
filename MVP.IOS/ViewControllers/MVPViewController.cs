using MVP.Common.Interceptors;
using MVP.Common.Interfaces.Presenters;
using MVP.Common.Interfaces.Views;

namespace MVP.IOS.ViewControllers
{
    public partial class MVPViewController : ViewControllerBase, IMPVView
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

        //demo code to create a simple view
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

            void AddButton(string title, Action action)
            {
                var button = new UIButton(UIButtonType.System)
                {
                    Frame = new CGRect(100, currentY, buttonWidth, buttonHeight)
                };
                button.SetTitle(title, UIControlState.Normal);
                button.TouchUpInside += (s, e) =>
                {
                    //call action with invoke to wrap it in a try catch
                    var result = Invoke(action, title);
                    if (!result.IsSuccess)
                    {
                        ShowMessage($"Error: {result.Exception?.Message ?? "Unknown Error"}");
                    }
                    else
                    {
                        ShowMessage($"Success: {title} executed successfully.");
                    }
                };
                scrollView.AddSubview(button);
                currentY += buttonHeight + verticalSpacing;
            }

            void AddButtonThatReturnsValue<T>(string title, Func<T> func)
            {
                var button = new UIButton(UIButtonType.System)
                {
                    Frame = new CGRect(100, currentY, buttonWidth, buttonHeight)
                };
                button.SetTitle(title, UIControlState.Normal);
                button.TouchUpInside += (s, e) =>
                {
                    var result = Invoke(func, title);
                    if (result.IsSuccess)
                    {
                        ShowMessage($"Success: {result.Value}");
                    }
                    else
                    {
                        ShowMessage($"Error: {result.Exception?.Message ?? "Unknown Error"}");
                    }
                };
                scrollView.AddSubview(button);
                currentY += buttonHeight + verticalSpacing;
            }

            void AddAsyncButton(string title, Func<Task> asyncAction)
            {
                var button = new UIButton(UIButtonType.System)
                {
                    Frame = new CGRect(100, currentY, buttonWidth, buttonHeight)
                };
                button.SetTitle(title, UIControlState.Normal);
                button.TouchUpInside += async (s, e) =>
                {
                    _loadingIndicator?.StartAnimating();
                    var result = await InvokeAsync(asyncAction, title);
                    _loadingIndicator?.StopAnimating();

                    if (!result.IsSuccess)
                    {
                        ShowMessage($"Error: {result.Exception?.Message ?? "Unknown Error"}");
                    }
                    else
                    {
                        ShowMessage($"Success: {title} executed successfully.");
                    }
                };
                scrollView.AddSubview(button);
                currentY += buttonHeight + verticalSpacing;
            }

            void AddAsyncButtonThatReturnsValue<T>(string title, Func<Task<T>> asyncFunc)
            {
                var button = new UIButton(UIButtonType.System)
                {
                    Frame = new CGRect(100, currentY, buttonWidth, buttonHeight)
                };
                button.SetTitle(title, UIControlState.Normal);
                button.TouchUpInside += async (s, e) =>
                {
                    _loadingIndicator?.StartAnimating();
                    var result = await InvokeAsync(asyncFunc, title);
                    _loadingIndicator?.StopAnimating();

                    if (result.IsSuccess)
                    {
                        ShowMessage($"Success: {result.Value}");
                    }
                    else
                    {
                        ShowMessage($"Error: {result.Exception?.Message ?? "Unknown Error"}");
                    }
                };
                scrollView.AddSubview(button);
                currentY += buttonHeight + verticalSpacing;
            }

            // Add Buttons
            AddButton("Handled Error", _presenter.SimulateHandledError);
            AddButton("Null Reference", _presenter.SimulateNullReference);
            AddButton("Invalid Operation", _presenter.SimulateInvalidOperation);
            AddButton("Bad Argument", _presenter.SimulateBadArgument);
            AddButton("Service Failure", _presenter.SimulateServiceFailure);
            AddButtonThatReturnsValue("Get Data", _presenter.GetData);
            AddAsyncButtonThatReturnsValue("Async API Failure", _presenter.SimulateAsyncApiFailure);

            scrollView.ContentSize = new CGSize(View.Bounds.Width, currentY + 100);
            View.AddSubview(scrollView);
        }

        public void ShowMessage(string value)
        {
            var alert = UIAlertController.Create("Notice", value, UIAlertControllerStyle.Alert);
            alert.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
            PresentViewController(alert, true, null);
        }
    }
}