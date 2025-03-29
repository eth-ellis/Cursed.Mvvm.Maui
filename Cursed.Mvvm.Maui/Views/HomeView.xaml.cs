using Cursed.Mvvm.Maui.ViewModels;
using Cursed.Mvvm.Maui.Views.Base;

namespace Cursed.Mvvm.Maui.Views;

public partial class HomeView : BaseView
{
    public HomeView(HomeViewModel viewModel)
    {
        InitializeComponent();
        
        this.BindingContext = viewModel;
    }
}