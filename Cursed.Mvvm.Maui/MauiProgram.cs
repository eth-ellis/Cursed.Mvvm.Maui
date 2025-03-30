using CommunityToolkit.Maui;
using Cursed.Mvvm.Maui.Services.Navigation;
using Cursed.Mvvm.Maui.Extensions;
using Cursed.Mvvm.Maui.ViewModels;
using Cursed.Mvvm.Maui.Views;

#if ANDROID
using Cursed.Mvvm.Maui.Handlers;
#endif

namespace Cursed.Mvvm.Maui;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            })
            .ConfigureMauiHandlers(handlers =>
            {
#if ANDROID
                handlers.AddHandler<Toolbar, CustomToolbarHandler>();
                
                CustomToolbarHandler.InitialiseMappers();
#endif
            })
            .RegisterNavigation()
            .RegisterServices();

        return builder.Build();
    }
    
    private static MauiAppBuilder RegisterNavigation(this MauiAppBuilder builder)
    {
        builder.Services.AddTransientWithShellRoute<HomeView, HomeViewModel>(nameof(HomeView));
        builder.Services.AddTransientWithShellRoute<DetailView, DetailViewModel>(nameof(DetailView));
        
        builder.Services.AddTransientModalWithShellRoute<ModalView, ModalViewModel>(nameof(ModalView));
        
        return builder;
    }
    
    private static MauiAppBuilder RegisterServices(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<INavigationService, NavigationService>();
        
        return builder;
    }
}