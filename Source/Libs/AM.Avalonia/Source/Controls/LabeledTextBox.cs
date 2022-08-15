// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* LabeledTextBox.cs -- текстовый бокс с меткой
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using Avalonia.Controls;
using Avalonia.Layout;

#endregion

#nullable enable

namespace AM.Avalonia.Controls;

/// <summary>
/// Текстовый бокс с меткой.
/// </summary>
public sealed class LabeledTextBox
    : UserControl
{
    #region Properties

    /// <summary>
    /// Надпись на встроенной в контрол метке.
    /// </summary>
    public string? Label
    {
        get => (string?) _innerLabel?.Content;
        set => _innerLabel?.SetValue (ContentProperty, value);
    }

    /// <summary>
    /// Содержимое встроенного в контрол поля для текстового ввода.
    /// </summary>
    public string? Text
    {
        get => _innerTextBox?.Text;
        set => _innerTextBox?.SetValue (TextBox.TextProperty, value);
    }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public LabeledTextBox()
    {
        _innerLabel = new Label
        {
            HorizontalAlignment = HorizontalAlignment.Stretch
        };

        _innerTextBox = new TextBox
        {
            HorizontalAlignment = HorizontalAlignment.Stretch
        };

        Content = new StackPanel
        {
            Orientation = Orientation.Vertical,
            Children =
            {
                _innerLabel,
                _innerTextBox
            }
        };
    }

    #endregion

    #region Private members

    /// <summary>
    /// Встроенная в контрол метка.
    /// </summary>
    private readonly Label? _innerLabel;

    /// <summary>
    /// Встроенное в контрол текстовое поле ввода.
    /// </summary>
    private readonly TextBox? _innerTextBox;

    #endregion
}
