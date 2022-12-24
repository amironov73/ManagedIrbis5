// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable CoVariantArrayConversion
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

/* Window.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Reactive.Linq;

using AM;
using AM.Avalonia;
using AM.Avalonia.AppServices;

using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.ReactiveUI;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

#endregion

#nullable enable

namespace AvaloniaApp;

/// <summary>
///
/// </summary>
public sealed class MainWindow
    : ReactiveWindow<Model>
{
    #region Window members

    /// <summary>
    /// Вызывается, когда окно проинициализировано фреймворком.
    /// </summary>
    protected override void OnInitialized()
    {
        base.OnInitialized();

        Title = "Калькулятор Avalonia";
        Width = MinWidth = 400;
        Height = MinHeight = 250;

        DataContext = new Model();

        Content = new DockPanel
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            Children =
            {
            }
        };
    }

    #endregion
}
