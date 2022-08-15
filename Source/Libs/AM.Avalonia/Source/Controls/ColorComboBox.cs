// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

/* ColorComboBox.cs -- комбобокс, позволяющий выбрать цвет из списка
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Media;
using Avalonia.Styling;

#endregion

#nullable enable

namespace AM.Avalonia.Controls;

/// <summary>
/// Комбобокс, позволяющий выбрать цвет из списка.
/// </summary>
public sealed class ColorComboBox
    : ComboBox, IStyleable
{
    #region Properties

    /// <summary>
    /// Регистрация свойства "выбранный цвет".
    /// </summary>
    public static StyledProperty<Color> SelectedColorProperty
        = AvaloniaProperty.Register<ColorComboBox, Color> (nameof (SelectedColor));

    /// <summary>
    /// Выбранный цвет.
    /// </summary>
    public Color SelectedColor
    {
        get => GetValue (SelectedItemProperty) is Color color ? color : Colors.Black;
        set
        {
            foreach (var item in Items)
            {
                if (item is Color color && color == value)
                {
                    SetValue (SelectedItemProperty, item);
                    break;
                }
            }
        }
    }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public ColorComboBox()
    {
        ItemTemplate = new FuncDataTemplate<Color> ((value, _) =>
            new TextBlock
            {
                Background = new SolidColorBrush (value),
                Foreground = GetContrastBrush (value),
                [!TextBlock.TextProperty] = new Binding()
            }
        );

        Items = new[]
        {
            Colors.Black,
            Colors.White,
            Colors.Red,
            Colors.Green,
            Colors.Blue,
            Colors.DarkGray,
            Colors.Gray,
            Colors.Cyan,
            Colors.Magenta,
            Colors.DarkRed,
            Colors.DarkGreen,
            Colors.DarkBlue,
            Colors.Brown
        };
    }

    #endregion

    #region Private members

    private static ISolidColorBrush GetContrastBrush (Color color)
    {
        var summa = color.B + color.G + color.R;
        return summa > 300 ? Brushes.Black : Brushes.White;
    }

    /// <inheritdoc cref="IStyleable.StyleKey"/>
    Type IStyleable.StyleKey => typeof (ComboBox);

    #endregion
}
