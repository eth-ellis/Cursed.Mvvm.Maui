using System.Diagnostics;
using Cursed.Mvvm.Maui.ViewModels.Base;

namespace Cursed.Mvvm.Maui;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
    }
    
    protected override void OnNavigated(ShellNavigatedEventArgs args)
    {
        base.OnNavigated(args);

        Debug.WriteLine($"-----Debug: {args.Source}");
        
        if (this.CurrentPage.BindingContext is not BaseViewModel viewModel)
        {
            return;
        }
        
        switch (args.Source)
        {
            case ShellNavigationSource.ShellItemChanged:
            case ShellNavigationSource.ShellSectionChanged:
                if (viewModel.HasInitialised)
                {
                    return;
                }

                _ = viewModel.InitialiseAsync();
                
                viewModel.HasInitialised = true;

                return;
            
            case ShellNavigationSource.Push:
                _ = viewModel.InitialiseAsync();
                
                viewModel.HasInitialised = true;
                
                return;
            
            case ShellNavigationSource.Pop:
            case ShellNavigationSource.PopToRoot:
                _ = viewModel.ReinitialiseAsync();
                return;
        }
    }
}