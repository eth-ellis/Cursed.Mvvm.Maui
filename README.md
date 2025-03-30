# Cursed.Mvvm.Maui

A somewhat cursed but surprisingly functional and simple experimental .NET MAUI Shell app implementing:
 -  ViewModel lifecycle events (Initialisation & reinitialisation with parameter passing)
 -  Modal navigation support (Navigation within modals and navigation bar support)

## Implementation

### ViewModel lifecycle events

The core of the ViewModel lifecycle event functionality is implemented by overriding the `Shell.OnNavigated` method in `AppShell.xaml.cs`.

This works for non-modal navigation, tab navigation, flyout navigation and the basic modal navigation supported by Shell.

```csharp
protected override void OnNavigated(ShellNavigatedEventArgs args)
{
    base.OnNavigated(args);
    
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
```

The `HasInitialised` check is required to stop the ViewModel from initialising every time tabs are switched.

Each tab should only initialise once.

---

If you don't care about:
- being able to navigate within modals
- displaying the navigation bar on modals

then this solution seems to work great for all platforms (Android, Windows, iOS, MacCatalyst).

If you do require this functionality keep reading below.

### Modal navigation support

If not using Shell, modal navigation can be achieved by wrapping the modal page in a NavigationPage.

```csharp
Navigation.PushAsync(new NavigationPage(page))
```

Navigation within the modal can then be performed by pushing and popping pages on the modal NavigationPage instead of the root NavigationPage.

---

To achieve similar functionality in Shell, the app adds a custom navigation page and uses it when registering routes for modals in `MauiProgram.cs`.

This means the GoToAsync method can be used to navigate to the modal, and a navigation page will be included in the navigation stack.

The GoToAsync method can then also be used to navigate to pages within the modal, including other modals.

```csharp
private static MauiAppBuilder RegisterNavigation(this MauiAppBuilder builder)
{
    builder.Services.AddTransient<ModalView>();
    builder.Services.AddTransient<ModalViewModel>();

    Routing.RegisterRoute(nameof(ModalView), typeof(ModalNavigationView<ModalView>));

    return builder;
}
```

The custom navigation page implements `IQueryAttributable` as Shell will pass parameters here instead of the route ViewModel i.e. `ModalViewModel`.

We can then pass the parameters to the route ViewModels copy of `ApplyQueryAttributes`, if it exists.

```csharp
public class ModalNavigationView<TView> : NavigationPage, IQueryAttributable
    where TView : BaseView
{
    private readonly TView view;
    
    public ModalNavigationView(TView view) : base(view)
    {
        this.view = view;
        
        Shell.SetPresentationMode(this, PresentationMode.ModalAnimated);

        this.On<iOS>().SetModalPresentationStyle(view.On<iOS>().ModalPresentationStyle());
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        (this.view.BindingContext as IQueryAttributable)?.ApplyQueryAttributes(query);
    }

    protected override bool OnBackButtonPressed()
    {
        Shell.Current.GoToAsync("..");

        return true;
    }
}
```

**Platform specific modifications**

**Android**

NavigationPage does not use `Shell.Current.GoToAsync("..")` when navigating backwards with the navigation bar or hardware back buttons.

As a result `Shell.OnNavigated > ReinitialiseAsync` will not be called when navigating back from child pages of the modal navigation page.

To resolve this, the behaviour of the navigation bar and hardware back buttons need to be overridden.

For the navigation bar back button, a custom toolbar handler has been introduced:

```csharp
public class CustomToolbarHandler : ToolbarHandler
{
    public static void InitialiseMappers()
    {
        Mapper.AppendToMapping<Toolbar, CustomToolbarHandler>(nameof(Toolbar.BackButtonTitle), MapProperty);
        Mapper.AppendToMapping<Toolbar, CustomToolbarHandler>(nameof(Toolbar.BackButtonVisible), MapProperty);
    }

    public static void MapProperty(CustomToolbarHandler handler, Toolbar toolbar)
    {
        _ = Task.Run(async () =>
        {
            await Task.Delay(500);

            handler.PlatformView.SetNavigationOnClickListener(new OnClickListener(onClick: () =>
            {
                Shell.Current.GoToAsync("..");
            }));
        });
    }
}

public class OnClickListener(Action onClick) : JavaObject, View.IOnClickListener
{
    public void OnClick(View? v) => onClick();
}
```

Configuration of the handler in `MauiProgram.cs`.

```csharp
#if ANDROID
.ConfigureMauiHandlers(handlers =>
{
    handlers.AddHandler<Toolbar, CustomToolbarHandler>();

    CustomToolbarHandler.InitialiseMappers();
})
#endif
```

For the hardware back button, `OnBackButtonPressed` is overridden in the custom navigation page.

```csharp
protected override bool OnBackButtonPressed()
{
    Shell.Current.GoToAsync("..");

    return true;
}
```

**iOS/MacCatalyst**

NavigationPage does not use `Shell.Current.GoToAsync("..")` when navigating backwards with the navigation bar back button or swipe gestures.

As a result `Shell.OnNavigated > ReinitialiseAsync` will not be called when navigating back from child pages of the modal navigation page.

No solution has been implemented to resolve this yet.

If you have ideas on how this could be resolved feel free to start a discussion or raise a PR.

**Windows**

Windows somehow seems to work great without any further platform modifications.
