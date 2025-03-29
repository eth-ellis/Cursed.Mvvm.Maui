using System.Diagnostics;
using CommunityToolkit.Mvvm.Input;
using Cursed.Mvvm.Maui.Services.Navigation;
using Cursed.Mvvm.Maui.ViewModels.Base;
using Cursed.Mvvm.Maui.Views;

namespace Cursed.Mvvm.Maui.ViewModels;

public partial class ModalViewModel : BaseViewModel, IQueryAttributable
{
    private readonly INavigationService navigationService;
    
    public ModalViewModel(
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
        Debug.WriteLine("-----Initialise Modal");
        
        return base.InitialiseAsync();
    }

    public override Task ReinitialiseAsync()
    {
        Debug.WriteLine("-----Reinitialise Modal");
        
        return base.ReinitialiseAsync();
    }
    
    [RelayCommand]
    private async Task OpenDetail()
    {
        await this.navigationService.NavigateToAsync<DetailView>(new Dictionary<string, object>
        {
            { "Debug", "From Modal" }
        });
    }
    
    [RelayCommand]
    private async Task OpenModal()
    {
        await this.navigationService.NavigateToAsync<ModalView>(new Dictionary<string, object>
        {
            { "Debug", "From Modal" }
        });
    }
    
    [RelayCommand]
    private async Task GoBack()
    {
        await this.navigationService.GoBackAsync(new Dictionary<string, object>
        {
            { "Debug", "From Modal" }
        });;
    }
}