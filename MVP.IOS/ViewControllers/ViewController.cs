using MVP.Common.Interfaces.Views;

namespace MVP.IOS.ViewControllers
{
    public partial class ViewController : UIViewController
    {
        private IMPVView _mPVView;

        public ViewController(IMPVView mPVView) : base()
        {
            _mPVView = mPVView;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            View.BackgroundColor = UIColor.SystemBackground;

            var btnMVC = new UIButton(UIButtonType.System)
            {
                Frame = new CoreGraphics.CGRect(157, 300, 100, 50),
                BackgroundColor = UIColor.Green
            };
            btnMVC.SetTitle("MVC Page", UIControlState.Normal);
            btnMVC.TouchUpInside += (sender, e) =>
            {
                var mvcVC = new MVCViewController();
                NavigationController?.PushViewController(mvcVC, true);
            };

            var btnMVP = new UIButton(UIButtonType.System)
            {
                Frame = new CoreGraphics.CGRect(157, 400, 100, 50),
                BackgroundColor = UIColor.Yellow
            };
            btnMVP.SetTitle("MVP Page", UIControlState.Normal);
            btnMVP.TouchUpInside += (sender, e) =>
            {
                var mvpVC = (MVPViewController)_mPVView;
                NavigationController?.PushViewController(mvpVC, true);
            };

            View.AddSubviews(btnMVC, btnMVP);
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}