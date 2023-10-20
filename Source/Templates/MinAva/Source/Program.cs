// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

/* Program.cs -- точка входа в приложение
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using Avalonia;

#endregion

namespace MinAva;

internal static class Program
{
    [STAThread]
    public static void Main
        (
            string[] args
        )
    {
        BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime (args);
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    private static AppBuilder BuildAvaloniaApp()
    {
        return AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .LogToTrace();
    }
}
