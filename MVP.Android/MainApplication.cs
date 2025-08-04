using System;
using Android.Runtime;
using Microsoft.Extensions.DependencyInjection;
using MVP.Common;
using MVP.Common.Interceptors;
using MVP.Common.Interfaces.Presenters;
using MVP.Common.Interfaces.Views;

namespace MVP.Android;

[Application]
public class MainApplication : Application
{
    public static IServiceProvider? ServiceProvider { get; private set; }

    public MainApplication(IntPtr handle, JniHandleOwnership transfer)
        : base(handle, transfer)
    {
    }

    public override void OnCreate()
    {
        base.OnCreate();

        SentrySdk.Init(o =>
        {
            o.Dsn = "https://f7ec04f282abe7e970961e3d811075cc@o4509769918644224.ingest.de.sentry.io/4509770669490256";
            o.Debug = true;
            o.TracesSampleRate = 1.0;
            o.SampleRate = 1.0f;
            o.IsGlobalModeEnabled = true;
        });

        var services = new ServiceCollection();
        services.AddSingleton<PresenterInvoker>();
        services.AddSingleton<IMVPPagePresenter, MVPPagePresenter>();

        ServiceProvider = services.BuildServiceProvider();
    }
}

