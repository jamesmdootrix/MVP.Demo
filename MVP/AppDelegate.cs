using Microsoft.Extensions.DependencyInjection;
using MVP.Common;
using MVP.Common.Interceptors;
using MVP.Common.Interfaces.Presenters;
using MVP.Common.Interfaces.Views;
using MVP.IOS.ViewControllers;

namespace MVP
{
    [Register("AppDelegate")]
    public class AppDelegate : UIApplicationDelegate
    {
        public override UIWindow? Window
        {
            get;
            set;
        }

        public override bool FinishedLaunching(UIApplication application, NSDictionary? launchOptions)
        {
            SentrySdk.Init(o =>
            {
                o.Dsn = "https://f7ec04f282abe7e970961e3d811075cc@o4509769918644224.ingest.de.sentry.io/4509770669490256";
                o.Debug = true;
                o.TracesSampleRate = 1.0;
                o.SampleRate = 1.0f;
                o.IsGlobalModeEnabled = true; // Ensure background threads hook in
            });

            var services = new ServiceCollection();
            services.AddSingleton<PresenterInvoker>();
            services.AddSingleton<IMVPPagePresenter, MVPPagePresenter>();
            services.AddTransient<IMPVView, MVPViewController>();

            // Create the window and set root ViewController manually
            Window = new UIWindow(UIScreen.MainScreen.Bounds);

            var serviceProvider = services.BuildServiceProvider();

            var mvpViewController = serviceProvider.GetRequiredService<IMPVView>();
            var rootViewController = new ViewController(mvpViewController);  // your custom ViewController.cs
            var navigationController = new UINavigationController(rootViewController);

            Window.RootViewController = navigationController;
            Window.MakeKeyAndVisible();


            return true;
        }
    }
}
