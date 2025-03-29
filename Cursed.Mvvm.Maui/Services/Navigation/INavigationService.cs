namespace Cursed.Mvvm.Maui.Services.Navigation;

public interface INavigationService
{
    Task NavigateToAsync<TView>(Dictionary<string, object>? parameters = null)
        where TView : ContentPage;
    
    Task GoBackAsync(Dictionary<string, object>? parameters = null);
}