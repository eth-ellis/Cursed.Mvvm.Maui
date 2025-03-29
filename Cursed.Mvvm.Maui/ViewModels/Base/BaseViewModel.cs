using CommunityToolkit.Mvvm.ComponentModel;

namespace Cursed.Mvvm.Maui.ViewModels.Base;

public class BaseViewModel : ObservableObject
{
    internal bool HasInitialised { get; set; } = false;
    
    public virtual Task InitialiseAsync()
    {
        return Task.CompletedTask;
    }
    
    public virtual Task ReinitialiseAsync()
    {
        return Task.CompletedTask;
    }
}