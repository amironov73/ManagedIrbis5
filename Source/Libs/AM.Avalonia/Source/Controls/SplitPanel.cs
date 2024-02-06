// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* SplitPanel.cs -- панель, поделенная сплиттером на две части
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;

using JetBrains.Annotations;

using Grid = Avalonia.Controls.Grid;

#endregion

namespace AM.Avalonia.Controls;

/// <summary>
/// Панель, поделенная сплиттером на две части.
/// </summary>
[PublicAPI]
public class SplitPanel
    : Panel
{
    #region Properties

    /// <summary>
    /// Первая часть.
    /// </summary>
    public Control? First
    {
        get => _firstControl;
        set => SetPanelChild (_firstPanel, _firstControl = value);
    }

    /// <summary>
    /// Вторая часть.
    /// </summary>
    public Control? Second
    {
        get => _secondControl;
        set => SetPanelChild (_secondPanel, _secondControl = value);
    }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public SplitPanel()
        : this (true)
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public SplitPanel
        (
            bool vertical
        )
    {
        var grid = new Grid
        {
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch
        };

        if (vertical)
        {
            grid.ColumnDefinitions = ColumnDefinitions.Parse ("*,4,*");
        }
        else
        {
            grid.RowDefinitions = RowDefinitions.Parse ("*,4,*");
        }

        _firstPanel = new Panel
        {
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch
        };
        grid.Children.Add (_firstPanel);

        var splitter = new GridSplitter
        {
            ResizeDirection = vertical
                ? GridResizeDirection.Columns
                : GridResizeDirection.Rows,
            Background = AvaloniaUtility.GetThemeBackgroundBrush()
        };

        var property = vertical ? Grid.ColumnProperty : Grid.RowProperty;
        splitter.SetValue (property, 1);
        grid.Children.Add (splitter);

        _secondPanel = new Panel
        {
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch
        };
        _secondPanel.SetValue (property, 2);
        grid.Children.Add (_secondPanel);

        Children.Add (grid);
    }

    #endregion

    #region Private members

    private readonly Panel _firstPanel;
    private readonly Panel _secondPanel;
    private Control? _firstControl;
    private Control? _secondControl;

    private void SetPanelChild
        (
            Panel panel,
            Control? child
        )
    {
        panel.Children.Clear();
        if (child is not null)
        {
            panel.Children.Add (child);
        }
    }

    #endregion

    #region StyledElement members

    /// <inheritdoc cref="StyledElement.StyleKeyOverride"/>
    protected override Type StyleKeyOverride => typeof (Panel);

    #endregion
}
