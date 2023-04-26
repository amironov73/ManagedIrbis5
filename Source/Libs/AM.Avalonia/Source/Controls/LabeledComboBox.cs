// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* LabeledComboBox.cs -- комбобокс с текстовой меткой
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections;

using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Layout;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.Avalonia.Controls;

/// <summary>
/// Комбобокс с текстовой меткой.
/// </summary>
[PublicAPI]
public sealed class LabeledComboBox
    : UserControl
{
    #region Properties

    /// <summary>
    /// Элементы списка.
    /// </summary>
    public IEnumerable? ItemsSource
    {
        get => _innerComboBox?.Items;
        set => _innerComboBox?.SetValue (ItemsControl.ItemsSourceProperty, value);
    }

    /// <summary>
    /// Надпись на встроенной в контрол метке.
    /// </summary>
    public string? Label
    {
        get => (string?) _innerLabel?.Content;
        set => _innerLabel?.SetValue (ContentProperty, value);
    }

    /// <summary>
    /// Индекс выбранного элемента.
    /// </summary>
    public int SelectedIndex
    {
        get => _innerComboBox?.SelectedIndex ?? -1;
        set => _innerComboBox?.SetValue (SelectingItemsControl.SelectedIndexProperty, value);
    }

    /// <summary>
    /// Выбранный элемент.
    /// </summary>
    public object? SelectedItem
    {
        get => _innerComboBox?.SelectedItem;
        set => _innerComboBox?.SetValue (SelectingItemsControl.SelectedItemProperty, value);
    }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public LabeledComboBox()
    {
        _innerLabel = new Label
        {
            HorizontalAlignment = HorizontalAlignment.Stretch
        };

        _innerComboBox = new ComboBox
        {
            HorizontalAlignment = HorizontalAlignment.Stretch
        };

        Content = new StackPanel
        {
            Orientation = Orientation.Vertical,
            Children =
            {
                _innerLabel,
                _innerComboBox
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
    /// Встроенный в контрол комбобокс.
    /// </summary>
    private readonly ComboBox? _innerComboBox;

    #endregion
}
