// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* StatusBar.cs -- строка статуса
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;

using JetBrains.Annotations;

using HorizontalAlignment = Avalonia.Layout.HorizontalAlignment;

#endregion

namespace AM.Avalonia.Controls;

/// <summary>
/// Строка статуса.
/// </summary>
[PublicAPI]
public class StatusBar
    : StackPanel
{
    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public StatusBar()
    {
        Spacing = 5;
        Orientation = Orientation.Horizontal;
        HorizontalAlignment = HorizontalAlignment.Stretch;
    }

    #endregion

    #region StyledElement members

    /// <inheritdoc cref="StyledElement.StyleKeyOverride"/>
    protected override Type StyleKeyOverride => typeof (StackPanel);

    #endregion
}
