using Sentry;
using System;

namespace MVP.Shared.Demo.Presenters
{
    public class MVPPagePresenter
    {
        public void SimulateHandledError()
        {
            try
            {
                throw new InvalidOperationException("Simulated MVP presenter error");
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
        }

        public void SimulateFatalCrash()
        {
            throw new ApplicationException("Fatal crash from MVP Presenter");
        }
    }
}