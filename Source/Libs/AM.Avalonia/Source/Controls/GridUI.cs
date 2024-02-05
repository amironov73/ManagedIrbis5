// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* GridUI.cs -- пользовательский интерфейс в виде сетки
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
/// Пользовательский интерфейс в виде сетки.
/// Заполняется по колонкам слева направа, сверху вниз.
/// </summary>
[PublicAPI]
public class GridUI
    : Grid
{
    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public GridUI()
    {
        HorizontalAlignment = HorizontalAlignment.Stretch;
        VerticalAlignment = VerticalAlignment.Stretch;
    }

    #endregion

    #region Private members

    private int _currentColumn = -1;
    private int _currentRow = -1;
    private List<GridSplitter>? _verticalSplitters;

    #endregion

    #region Public methods

    /// <summary>
    /// Добавление кнопки.
    /// </summary>
    public GridUI AddButton
        (
            string text,
            EventHandler<RoutedEventArgs> action,
            HorizontalAlignment alignment = HorizontalAlignment.Stretch
        )
    {
        Sure.NotNull (action);

        var button = new Button
        {
            Content = text,
            HorizontalAlignment = alignment,
            HorizontalContentAlignment = HorizontalAlignment.Center
        };
        button.Click += action;

        return AddControl (button);
    }

    /// <summary>
    /// Добавление новой колонки.
    /// </summary>
    public GridUI AddColumn
        (
            ColumnDefinition? definition = null
        )
    {
        definition ??= new ColumnDefinition (GridLength.Star);
        ColumnDefinitions.Add (definition);
        _currentRow = 0;
        _currentColumn++;

        return this;
    }

    /// <summary>
    /// Добавление контрола в указанную ячейку.
    /// </summary>
    public GridUI AddControl
        (
            Control control,
            int column,
            int row
        )
    {
        Sure.NotNull (control);
        Sure.NonNegative (column);
        Sure.NonNegative (row);

        control.SetValue (ColumnProperty, column);
        control.SetValue (RowProperty, row);
        Children.Add (control);

        return this;
    }

    /// <summary>
    /// Добавление контрола в указанную ячейку.
    /// </summary>
    public GridUI AddControl
        (
            Control control,
            RowDefinition? definition = null,
            HorizontalAlignment? alignment = null
        )
    {
        Sure.NotNull (control);

        while (_currentRow >= RowDefinitions.Count)
        {
            definition ??= new RowDefinition (GridLength.Auto);
            RowDefinitions.Add (definition);
        }

        if (alignment.HasValue)
        {
            control.HorizontalAlignment = alignment.Value;
        }

        var result = AddControl (control, _currentColumn, _currentRow);
        _currentRow++;

        return result;
    }

    /// <summary>
    /// Добавление горизонтального сплиттера (НЕ на всю ширину грида).
    /// </summary>
    public GridUI AddHorizontalSplitter() => AddControl (new GridSplitter
        {
            ResizeDirection = GridResizeDirection.Rows,
            Background = AvaloniaUtility.GetThemeBackgroundBrush()
        });

    /// <summary>
    /// Добавление метки.
    /// </summary>
    public GridUI AddLabel (string text) =>
        AddControl (new Label { Content = text });

    /// <summary>
    /// Добавление поля для ввода текста.
    /// </summary>
    public GridUI AddTextBox
        (
            string label,
            IBinding binding,
            HorizontalAlignment alignment = HorizontalAlignment.Stretch
        )
    {
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

    /// <summary>
    /// Добавление вертикального сплиттера.
    /// </summary>
    public GridUI AddVerticalSplitter()
    {
        AddColumn (new ColumnDefinition (4.0, GridUnitType.Pixel));
        var splitter = new GridSplitter
        {
            [ColumnProperty] = _currentColumn,
            ResizeDirection = GridResizeDirection.Columns,
            Background = AvaloniaUtility.GetThemeBackgroundBrush()
        };
        Children.Add (splitter);

        _verticalSplitters ??= new List<GridSplitter>();
        _verticalSplitters.Add (splitter);

        return this;
    }

    /// <summary>
    /// Окончание строительства.
    /// </summary>
    public GridUI Build()
    {
        if (_verticalSplitters is not null)
        {
            foreach (var splitter in _verticalSplitters)
            {
                splitter.SetValue (RowProperty, 0);
                splitter.SetValue (RowSpanProperty, RowDefinitions.Count);
            }
        }

        return this;
    }

    /// <summary>
    /// Пропуск строки, например, из-за сплиттера.
    /// </summary>
    public GridUI SkipRow()
    {
        _currentRow++;
        return this;
    }

    #endregion
}
