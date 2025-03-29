using System.Diagnostics;
using CommunityToolkit.Mvvm.Input;
using Cursed.Mvvm.Maui.Services.Navigation;
using Cursed.Mvvm.Maui.ViewModels.Base;
using Cursed.Mvvm.Maui.Views;

namespace Cursed.Mvvm.Maui.ViewModels;

public partial class DetailViewModel : BaseViewModel, IQueryAttributable
{
    private readonly INavigationService navigationService;
    
    public DetailViewModel(
        INavigationService navigationService)
    {
        this.navigationService = navigationService;
    }
    
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("Debug", out var debug) && debug is string debugAsString)
        {
            Debug.WriteLine($"-----Query Parameter: {debugAsString}");
        }
        
        query.Clear();
    }

    public override Task InitialiseAsync()
    {
        Debug.WriteLine("-----Initialise Detail");
        
        return base.InitialiseAsync();
    }

    public override Task ReinitialiseAsync()
    {
        Debug.WriteLine("-----Reinitialise Detail");
        
        return base.ReinitialiseAsync();
    }
    
    [RelayCommand]
    private async Task OpenDetail()
    {
        await this.navigationService.NavigateToAsync<DetailView>(new Dictionary<string, object>
        {
            { "Debug", "From Detail" }
        });
    }
    
    [RelayCommand]
    private async Task OpenModal()
    {
        await this.navigationService.NavigateToAsync<ModalView>(new Dictionary<string, object>
        {
            { "Debug", "From Detail" }
        });
    }
}