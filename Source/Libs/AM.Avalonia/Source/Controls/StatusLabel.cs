// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* StatusLabel.cs -- метка в строке статуса
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
/// Метка в строке статуса.
/// </summary>
[PublicAPI]
public class StatusLabel
    : Label
{
    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public StatusLabel()
    {
        Margin = new Thickness (6, 3);
        VerticalAlignment = VerticalAlignment.Center;
        VerticalContentAlignment = VerticalAlignment.Center;
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public StatusLabel
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
    protected override Type StyleKeyOverride => typeof (Label);

    #endregion
}
