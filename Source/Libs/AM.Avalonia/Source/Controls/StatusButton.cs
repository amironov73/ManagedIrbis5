// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* StatusButton.cs -- кнопка в строке статуса
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;

using JetBrains.Annotations;

#endregion

namespace AM.Avalonia.Controls;

/// <summary>
/// Кнопка в строке статуса.
/// </summary>
[PublicAPI]
public class StatusButton
    : Button
{
    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public StatusButton()
    {
        Margin = new Thickness (6, 3);
        HorizontalContentAlignment = HorizontalAlignment.Center;
        VerticalContentAlignment = VerticalAlignment.Center;
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public StatusButton
        (
            object? content
        )
        : this()
    {
        Content = content;
    }

    #endregion

    #region StyledElement members

    /// <inheritdoc cref="StyledElement.StyleKeyOverride"/>
    protected override Type StyleKeyOverride => typeof (Button);

    #endregion
}
