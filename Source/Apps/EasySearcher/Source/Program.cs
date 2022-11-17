// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable CoVariantArrayConversion
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

/* Program.cs -- точка входа в приложение
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM.Avalonia.AppServices;

#endregion

#nullable enable

namespace EasySearcher;

internal static class Program
{
    [STAThread]
    public static void Main (string[] args)
    {
        DesktopApplication
            .BuildAvaloniaApp (args)
            .UseMainWindow<MainWindow>()
            .Run();
    }
}
