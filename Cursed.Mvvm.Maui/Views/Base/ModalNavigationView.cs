using Cursed.Mvvm.Maui.Views.Base;

namespace Cursed.Mvvm.Maui.ViewModels.Base;

public class ModalNavigationView<TView> : NavigationPage, IQueryAttributable
    where TView : BaseView
{
    private readonly TView view;
    
    public ModalNavigationView(TView view) : base(view)
    {
        this.view = view;
        
        Shell.SetPresentationMode(this, PresentationMode.ModalAnimated);
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        (this.view.BindingContext as IQueryAttributable)?.ApplyQueryAttributes(query);
    }

    protected override bool OnBackButtonPressed()
    {
        Shell.Current.GoToAsync("..");

        return true;
    }
}