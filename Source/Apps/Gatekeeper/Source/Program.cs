// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* Program.cs -- точка входа в программу
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM.Avalonia.AppServices;

using Avalonia;

#endregion

#nullable enable

namespace Gatekeeper;

internal sealed class Program
{
    #region Program entry point

    /// <summary>
    /// Точка входа в программу.
    /// </summary>
    /// <param name="args">Аргументы командной строки</param>
    [STAThread]
    public static void Main
        (
            string[] args
        )
    {
        DesktopApplication.BuildAvaloniaApp (args)
            .UseMainWindow<MainWindow>()
            .WithApplicationName()
            .WithNativeMenu()
            .Run();
    }

    #endregion
}
