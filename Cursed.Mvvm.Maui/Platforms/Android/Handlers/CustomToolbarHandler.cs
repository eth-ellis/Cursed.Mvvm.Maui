using Microsoft.Maui.Handlers;
using JavaObject = Java.Lang.Object;
using View = Android.Views.View;

namespace Cursed.Mvvm.Maui.Platforms.Android.Handlers;

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