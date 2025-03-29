using Cursed.Mvvm.Maui.ViewModels;
using Cursed.Mvvm.Maui.Views.Base;

namespace Cursed.Mvvm.Maui.Views;

public partial class ModalView : BaseView
{
    public ModalView(ModalViewModel viewModel)
    {
        InitializeComponent();
        
        this.BindingContext = viewModel;
    }
}