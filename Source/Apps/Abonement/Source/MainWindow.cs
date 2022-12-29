// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable CoVariantArrayConversion
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

/* MainWindow.cs -- главное окно приложения
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM;
using AM.Avalonia;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.ReactiveUI;
using Avalonia.Styling;
using Avalonia.Threading;

#endregion

#nullable enable

namespace Abonement;

/// <summary>
/// Главное окно приложения.
/// </summary>
internal sealed class MainWindow
    : Window
{
    #region Construction

    /// <summary>
    /// Конструктор
    /// </summary>
    public MainWindow()
    {
        this.AttachDevTools();

        Title = "АРМ Абонемент";
        Width = MinWidth = 800;
        Height = MinHeight = 450;

        this.SetWindowIcon ("Assets/guard.ico");
    }

    #endregion

    #region Window members


    #endregion

    #region Private members

    #endregion
}
