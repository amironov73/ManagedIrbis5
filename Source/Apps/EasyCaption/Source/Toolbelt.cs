// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable CoVariantArrayConversion
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

/* Toolbelt.cs -- полезные инструменты
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM;
using AM.Avalonia;
using AM.Avalonia.AppServices;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.ReactiveUI;

#endregion

namespace EasyCaption;

/// <summary>
/// Полезные инструменты.
/// </summary>
public sealed class Toolbelt
    : UserControl
{
    #region Construction

    public Toolbelt()
    {
        var dummyButton = new Button
        {
            Content = "Dummy"
        };

        Content = new StackPanel
        {
            Spacing = 5,
            Margin = new Thickness (5),
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch,
            Children =
            {
                dummyButton
            }
        };
    }

    #endregion
}
