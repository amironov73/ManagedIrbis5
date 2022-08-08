using System;

using AM;
using AM.Avalonia.AppServices;

using AvaloniaExample;

using Microsoft.Extensions.Logging;

internal static class Program
{
    [STAThread]
    public static int Main
        (
            string[] args
        )
    {
        return DesktopApplication.BuildAvaloniaApp (args)
            .UseMainWindow<MainWindow>()
            .Run();
    }
}
