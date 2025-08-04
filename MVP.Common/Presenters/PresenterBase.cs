using MVP.Common.Interfaces.Views;

namespace MVP.Common.Presenters;

public class PresenterBase<TView> where TView : class, IView
{
    public TView? View { get; private set; }


    public void AttachView(TView view)
    {
        View = view;
    }
}
