using System;

using Avalonia;

namespace ApplicationOne;

internal sealed class Program
{
    [STAThread]
    public static void Main (string[] args)
    {
        BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime (args);
    }

    public static AppBuilder BuildAvaloniaApp()
    {
        return AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .LogToTrace();
    }
}
