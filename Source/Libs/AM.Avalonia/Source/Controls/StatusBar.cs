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
using Avalonia.Interactivity;
using Avalonia.Layout;

using JetBrains.Annotations;

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

    #region Public methods

    /// <summary>
    /// Добавление кнопки.
    /// </summary>
    public StatusBar AddButton
        (
            string text,
            EventHandler<RoutedEventArgs> action
        )
    {
        Sure.NotNull (action);

        var button = new StatusButton (text);
        button.Click += action;

        return AddControl (button);
    }

    /// <summary>
    /// Добавление произвольного контрола.
    /// </summary>
    public StatusBar AddControl
        (
            Control control
        )
    {
        Sure.NotNull (control);

        Children.Add (control);

        return this;
    }

    /// <summary>
    /// Добавление метки.
    /// </summary>
    public StatusBar AddLabel (string text) => AddControl (new StatusLabel (text));

    #endregion

    #region StyledElement members

    /// <inheritdoc cref="StyledElement.StyleKeyOverride"/>
    protected override Type StyleKeyOverride => typeof (StackPanel);

    #endregion
}
