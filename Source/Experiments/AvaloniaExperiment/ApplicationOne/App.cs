using System;

using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml.Styling;

namespace ApplicationOne;

public class App
    : Application
{
    public override void Initialize()
    {
        Current!.Styles.Add (new StyleInclude (new Uri ("avares://ControlCatalog/Styles"))
        {
            Source = new Uri ("avares://Avalonia.Themes.Fluent/FluentLight.xaml")
            // Source = new Uri ("avares://Avalonia.Themes.Fluent/FluentDark.xaml")
            // Source = new Uri ("avares://Avalonia.Themes.Default/DefaultTheme.xaml")
        });
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow();
        }

        base.OnFrameworkInitializationCompleted();
    }

}
