using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;
using NavigationPage = Microsoft.Maui.Controls.NavigationPage;

namespace Cursed.Mvvm.Maui.Views.Base;

public class ModalNavigationView<TView> : NavigationPage, IQueryAttributable
    where TView : BaseView
{
    private readonly TView view;
    
    public ModalNavigationView(TView view) : base(view)
    {
        this.view = view;
        
        Shell.SetPresentationMode(this, PresentationMode.ModalAnimated);

        this.On<iOS>().SetModalPresentationStyle(view.On<iOS>().ModalPresentationStyle());
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