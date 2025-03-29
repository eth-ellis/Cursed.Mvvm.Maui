using Cursed.Mvvm.Maui.ViewModels;
using Cursed.Mvvm.Maui.Views.Base;

namespace Cursed.Mvvm.Maui.Views;

public partial class DetailView : BaseView
{
    public DetailView(DetailViewModel viewModel)
    {
        InitializeComponent();
        
        this.BindingContext = viewModel;
    }
}