namespace Cursed.Mvvm.Maui.Services.Navigation;

public class NavigationService : INavigationService
{
    public async Task NavigateToAsync<TView>(Dictionary<string, object>? parameters = null)
        where TView : ContentPage
    {
        parameters ??= [];
        
        await Shell.Current.GoToAsync(typeof(TView).Name, parameters);
    }

    public async Task GoBackAsync(Dictionary<string, object>? parameters = null)
    {
        parameters ??= [];

        await Shell.Current.GoToAsync("..", parameters);
    }
}