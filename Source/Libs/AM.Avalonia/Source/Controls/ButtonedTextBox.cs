// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* ButtonedTextBox.cs -- текстовый бокс, снабженный кнопкой
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;

#endregion

#nullable enable

namespace AM.Avalonia.Controls;

/// <summary>
/// Текстовый бокс, снабженный кнопкой.
/// </summary>
public class ButtonedTextBox
    : UserControl
{
    #region Events

    /// <summary>
    /// Описание события "Щелчок по кнопке".
    /// </summary>
    public static readonly RoutedEvent<RoutedEventArgs> ButtonClickEvent =
        RoutedEvent.Register<ButtonedTextBox, RoutedEventArgs>(nameof (ButtonClick), RoutingStrategies.Bubble);

    /// <summary>
    /// Собственно событие "Щелчок по кнопке".
    /// </summary>
    public event EventHandler<RoutedEventArgs>? ButtonClick
    {
        add => AddHandler (ButtonClickEvent, value);
        remove => RemoveHandler (ButtonClickEvent, value);
    }

    #endregion

    #region Properties

    /// <summary>
    /// Описание свойства "Текст на кнопке".
    /// </summary>
    public static readonly StyledProperty<object?> CaptionProperty
        = AvaloniaProperty.Register<ButtonedTextBox, object?> (nameof (Caption));

    /// <summary>
    /// Описание свойства "Текст в боксе".
    /// </summary>
    public static readonly StyledProperty<string?> TextProperty
        = AvaloniaProperty.Register<ButtonedTextBox, string?> (nameof (Text));

    /// <summary>
    /// Собственно текстовый бокс.
    /// </summary>
    public TextBox TextBox { get; }

    /// <summary>
    /// Присобаченная кнопка.
    /// </summary>
    public Button Button { get; }

    /// <summary>
    /// Текст в боксе.
    /// </summary>
    public string? Text
    {
        get => TextBox.Text;
        set => TextBox.SetValue (TextBox.TextProperty, value);

    }

    /// <summary>
    /// Текст на кнопке.
    /// </summary>
    public object? Caption
    {
        get => Button.Content;
        set => Button.SetValue (ContentProperty, value);
    }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ButtonedTextBox()
    {
        TextBox = new TextBox();
        Button = new Button();
        Button.SetValue (Grid.ColumnProperty, 1);
        Button.Click += Button_OnClick;

        Content = new Grid
        {
            ColumnDefinitions = new ColumnDefinitions ("*,Auto"),
            Children =
            {
                TextBox,
                Button
            }
        };
    }

    #endregion

    #region Private members

    private void Button_OnClick
        (
            object? sender,
            RoutedEventArgs eventArgs
        )
    {
        sender.NotUsed();
        eventArgs.NotUsed();

        var e = new RoutedEventArgs (ButtonClickEvent);
        RaiseEvent (e);
    }

    #endregion
}
