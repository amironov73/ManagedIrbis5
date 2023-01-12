// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* CompactTextBox.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Media;

using ReactiveUI;

#endregion

#nullable enable


namespace AM.Avalonia.Controls;

/// <summary>
/// Компактный TextBox.
/// </summary>
public class CompactTextBox
    : UserControl
{
    #region Properties

    /// <summary>
    /// Описание свойства <see cref="Value"/>.
    /// </summary>
    public static readonly DirectProperty<CompactTextBox, string?> ValueProperty =
        AvaloniaProperty.RegisterDirect<CompactTextBox, string?>
            (
                nameof (Value),
                x => x.Value,
                (x, v) => x.Value = v,
                defaultBindingMode: BindingMode.TwoWay,
                enableDataValidation: true
            );

    /// <summary>
    /// Описание свойства <see cref="Caption"/>.
    /// </summary>
    public static readonly DirectProperty<CompactTextBox, string?> CaptionProperty =
        AvaloniaProperty.RegisterDirect<CompactTextBox, string?>
            (
                nameof (Caption),
                x => x.Caption,
                (x, v) => x.Caption = v,
                defaultBindingMode: BindingMode.TwoWay
            );

    /// <summary>
    /// Заголовок-описание.
    /// </summary>
    public string? Caption
    {
        get => (string?) _captionLabel.Content;
        set => _captionLabel.Content = value;
    }

    /// <summary>
    /// Значение.
    /// </summary>
    public string? Value
    {
        get => (string?) _valueLabel.Content;
        set => _valueLabel.Content = value;
    }

    /// <summary>
    /// Выноска.
    /// </summary>
    public Flyout Flyout { get; }

    #endregion

    #region Construction

    /// <summary>
    ///
    /// </summary>
    public CompactTextBox()
    {
        var hand = new Cursor (StandardCursorType.Hand);
        Cursor = hand;

        _captionLabel = new Label
        {
            Cursor = hand,
            VerticalContentAlignment = global::Avalonia.Layout.VerticalAlignment.Center
        };
        _captionLabel.PointerPressed += Pointer_Pressed;

        _valueLabel = new Label
        {
            Cursor = hand,
            FontWeight = FontWeight.DemiBold,
            VerticalContentAlignment = global::Avalonia.Layout.VerticalAlignment.Center
        };
        _valueLabel.PointerPressed += Pointer_Pressed;

        _textBox = new TextBox
        {
            Width = 130
        };

        _flyoutLabel = new Label();

        Flyout = new Flyout
        {
            Content = new Border
            {
                //Padding = new Thickness (5),
                BorderBrush = Brushes.Black,
                CornerRadius = new CornerRadius (5),
                BorderThickness = new Thickness (1),
                BoxShadow = new BoxShadows (new BoxShadow
                {
                    Color = Colors.DarkGray,
                    OffsetX = 5,
                    OffsetY = 5,
                    Blur = 3
                }),

                Child = new StackPanel
                {
                    Children =
                    {
                        _flyoutLabel,
                        _textBox,
                        new Button
                        {
                            Content = "x",
                            Command = ReactiveCommand.Create (CloseFlyout)
                        }
                    }
                }
            }
        };

        Content = AvaloniaUtility.HorizontalGroup
            (
                _captionLabel,
                _valueLabel
            );
    }

    #endregion

    #region Private members

    private readonly Label _flyoutLabel;
    private readonly Label _captionLabel;
    private readonly Label _valueLabel;
    private readonly TextBox _textBox;

    private void Pointer_Pressed (object? sender, PointerPressedEventArgs eventArgs)
    {
        OpenFlyout();
    }

    /// <summary>
    /// Opens the button's flyout.
    /// </summary>
    private void OpenFlyout()
    {
        _flyoutLabel.Content = Caption;
        _textBox.Text = Value;
        Flyout.ShowAt(this);
    }

    /// <summary>
    /// Closes the button's flyout.
    /// </summary>
    private void CloseFlyout()
    {
        Flyout.Hide();
        SetValue (ValueProperty, _textBox.Text);
    }

    #endregion

}
