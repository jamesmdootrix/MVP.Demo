namespace MVP.IOS.ViewControllers
{
    public partial class MVCViewController : UIViewController
    {
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            View.BackgroundColor = UIColor.Green;

            var scrollView = new UIScrollView(View.Bounds)
            {
                AutoresizingMask = UIViewAutoresizing.FlexibleDimensions
            };

            int buttonHeight = 50;
            int buttonWidth = 200;
            int verticalSpacing = 20;
            int startY = 100;
            int currentY = startY;

            void AddButton(string title, Action action)
            {
                var button = new UIButton(UIButtonType.System)
                {
                    Frame = new CoreGraphics.CGRect(100, currentY, buttonWidth, buttonHeight)
                };
                button.SetTitle(title, UIControlState.Normal);
                button.TouchUpInside += (s, e) => action();
                scrollView.AddSubview(button);
                currentY += buttonHeight + verticalSpacing;
            }

            void AddAsyncButton(string title, Func<Task> asyncAction)
            {
                var button = new UIButton(UIButtonType.System)
                {
                    Frame = new CoreGraphics.CGRect(100, currentY, buttonWidth, buttonHeight)
                };
                button.SetTitle(title, UIControlState.Normal);
                button.TouchUpInside += async (s, e) => await asyncAction();
                scrollView.AddSubview(button);
                currentY += buttonHeight + verticalSpacing;
            }

            // Simulate raw uncaught errors:
            AddButton("Null Reference", () =>
            {
                string? value = null;
                Console.WriteLine(value.ToString());  // Throws NullReferenceException
            });

            AddButton("Invalid Operation", () =>
            {
                throw new InvalidOperationException("Uncaught: Presenter is in an invalid state.");
            });

            AddButton("Bad Argument", () =>
            {
                throw new ArgumentException("Uncaught: Invalid argument passed to presenter method.");
            });

            AddButton("Service Failure", () =>
            {
                throw new HttpRequestException("Uncaught: Failed to reach backend service.");
            });

            AddAsyncButton("Async API Failure", async () =>
            {
                await Task.Delay(500);
                throw new HttpRequestException("Uncaught: Async API call failed due to timeout.");
            });

            scrollView.ContentSize = new CoreGraphics.CGSize(View.Bounds.Width, currentY + 100);
            View.AddSubview(scrollView);
        }
    }
}