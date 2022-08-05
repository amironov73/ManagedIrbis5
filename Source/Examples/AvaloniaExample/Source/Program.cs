using System;

using AM;
using AM.Avalonia.AppServices;

using AvaloniaExample;

using Microsoft.Extensions.Logging;

internal sealed class Program
    : AvaloniaApplication
{
    public Program
        (
            string[] args
        )
    {
        UseArgs (args);
    }

    [STAThread]
    public static void Main
        (
            string[] args
        )
    {
        new Program (args)
            .UseMainWindow<MainWindow>()
            .Run (app =>
            {
                app.Logger.LogInformation ("Это Авалония");

                return 0;
            });
    }
}
