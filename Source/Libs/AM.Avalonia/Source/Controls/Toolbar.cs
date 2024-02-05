// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* Toolbar.cs -- панель инструментов
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
/// Панель инструментов.
/// </summary>
[PublicAPI]
public class Toolbar
    : StackPanel
{
    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public Toolbar()
    {
        Spacing = 5;
        Margin = new Thickness (5);
        Orientation = Orientation.Horizontal;
        HorizontalAlignment = HorizontalAlignment.Stretch;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Добавление метки.
    /// </summary>
    public Toolbar AddButton
        (
            string text,
            EventHandler<RoutedEventArgs> action
        )
    {
        Sure.NotNull (action);

        var button = new Button { Content = text };
        button.Click += action;

        return AddControl (button);
    }

    /// <summary>
    /// Добавление произвольного контрола.
    /// </summary>
    public Toolbar AddControl
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
    public Toolbar AddLabel (string text) => AddControl (new Label { Content = text });

    #endregion

    #region StyledElement members

    /// <inheritdoc cref="StyledElement.StyleKeyOverride"/>
    protected override Type StyleKeyOverride => typeof (StackPanel);

    #endregion
}
