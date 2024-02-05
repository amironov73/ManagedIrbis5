// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* StackUI.cs -- пользовательский интерфейс в виде стопки контролов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Interactivity;
using Avalonia.Layout;

using JetBrains.Annotations;

#endregion

namespace AM.Avalonia.Controls;

/// <summary>
/// Пользовательский интерфейс в виде стопки контролов.
/// </summary>
[PublicAPI]
public class StackUI
    : StackPanel
{
    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public StackUI()
    {
        Spacing = 5;
        Orientation = Orientation.Vertical;
        HorizontalAlignment = HorizontalAlignment.Stretch;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Добавление кнопки.
    /// </summary>
    public StackUI AddButton
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
    public StackUI AddControl
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
    public StackUI AddLabel (string text) => AddControl (new StatusLabel (text));

    /// <summary>
    /// Добавление текстового поля ввода.
    /// </summary>
    public StackUI AddTextBox
        (
            string label,
            IBinding binding
        )
    {
        Sure.NotNull (binding);

        var panel = new StackPanel
        {
            Orientation = Orientation.Vertical,
            Margin = new Thickness (5),
            HorizontalAlignment = HorizontalAlignment.Stretch,

            Children =
            {
                new Label { Content = label },
                new TextBox
                {
                    [!TextBox.TextProperty] = binding,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                }
            }
        };

        return AddControl (panel);
    }

    #endregion
}
