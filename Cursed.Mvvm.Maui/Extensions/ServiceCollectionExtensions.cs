using Cursed.Mvvm.Maui.ViewModels.Base;
using Cursed.Mvvm.Maui.Views.Base;

namespace Cursed.Mvvm.Maui.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTransientModalWithShellRoute<TView, TViewModel>(this IServiceCollection services, string route)
        where TView : BaseView
        where TViewModel : BaseViewModel
    {
        services.AddTransient<TView>();
        services.AddTransient<TViewModel>();
        
        Routing.RegisterRoute(route, typeof(ModalNavigationView<TView>));
        
        return services;
    }
}