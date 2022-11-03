// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedParameter.Local

/* GridEx.cs -- грид с удобной разметкой.
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

using Avalonia;
using Avalonia.Controls;

#endregion

namespace GridExtra.Avalonia;

/// <summary>
/// Грид с удобной разметкой.
/// </summary>
public class GridEx
    : Control
{
    /// <summary>
    /// Статический конструктор.
    /// </summary>
    static GridEx()
    {
        TemplateAreaProperty.Changed.Subscribe (OnTemplateAreaChanged);
        AreaNameProperty.Changed.Subscribe (OnAreaNameChanged);
        AreaProperty.Changed.Subscribe (OnAreaChanged);
        AffectsMeasure<GridEx> (TemplateAreaProperty);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static IList<NamedAreaDefinition> GetAreaDefinitions
        (
            AvaloniaObject obj
        )
    {
        return (IList<NamedAreaDefinition>) obj.GetValue (AreaDefinitionsProperty);
    }

    private static void SetAreaDefinitions
        (
            AvaloniaObject obj,
            IList<NamedAreaDefinition> value
        )
    {
        obj.SetValue (AreaDefinitionsProperty, value);
    }

    public static readonly AvaloniaProperty<IList<NamedAreaDefinition>> AreaDefinitionsProperty =
        AvaloniaProperty.RegisterAttached<GridEx, Control, IList<NamedAreaDefinition>> ("AreaDefinitions", null);

    public static string? GetTemplateArea
        (
            AvaloniaObject obj
        )
    {
        return (string?) obj.GetValue (TemplateAreaProperty);
    }

    public static void SetTemplateArea
        (
            AvaloniaObject obj,
            string value
        )
    {
        obj.SetValue (TemplateAreaProperty, value);
    }

    private static void EvaluateTemplateArea
        (
            object? sender,
            VisualTreeAttachmentEventArgs eventArgs
        )
    {
        if (sender is Grid grid)
        {
            InitializeTemplateArea (grid, (string?) grid.GetValue (TemplateAreaProperty));
        }
    }

    public static readonly AvaloniaProperty<string> TemplateAreaProperty =
        AvaloniaProperty.RegisterAttached<GridEx, Control, string> ("TemplateArea", null);

    private static void OnTemplateAreaChanged
        (
            AvaloniaPropertyChangedEventArgs e
        )
    {
        var grid = e.Sender as Grid;
        var param = e.NewValue as string;

        if (grid == null)
        {
            return;
        }

        // Remove old event bindings and reattach.
        grid.AttachedToVisualTree -= EvaluateTemplateArea;
        grid.AttachedToVisualTree += EvaluateTemplateArea;

        // Clear old row&col defintions.
        grid.RowDefinitions.Clear();
        grid.ColumnDefinitions.Clear();

        if (param != null)
        {
            InitializeTemplateArea (grid, param);
        }
    }

    private static void InitializeTemplateArea
        (
            Grid grid,
            string? param
        )
    {
        var columns = param.Split (new[] { '\n', '/' })
            .Select (o => o.Trim())
            .Where (o => !string.IsNullOrWhiteSpace (o))
            .Select (o => o.Split (new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));

        var num = columns.FirstOrDefault().Count();
        var isValidRowColumn = columns.All (o => o.Count() == num);
        if (!isValidRowColumn)
        {
            throw new ArgumentException ("Invalid Row/Column definition.");
        }

        var rowShortage = columns.Count() - grid.RowDefinitions.Count;
        for (var i = 0; i < rowShortage; i++)
        {
            grid.RowDefinitions.Add (new RowDefinition());
        }

        var columnShortage = num - grid.ColumnDefinitions.Count;
        for (var i = 0; i < columnShortage; i++)
        {
            grid.ColumnDefinitions.Add (new ColumnDefinition());
        }

        var areaList = ParseAreaDefinition (columns);
        SetAreaDefinitions (grid, areaList);

        foreach (Control child in grid.Children)
        {
            UpdateItemPosition (child);
        }
    }

    private static IList<NamedAreaDefinition> ParseAreaDefinition
        (
            IEnumerable<string[]> columns
        )
    {
        var result = new List<NamedAreaDefinition>();

        var flatten = columns.SelectMany (
                (item, index) => item.Select ((o, xIndex) => new { row = index, column = xIndex, name = o })
            );

        var groups = flatten.GroupBy (o => o.name);
        foreach (var group in groups)
        {
            var left = group.Min (o => o.column);
            var top = group.Min (o => o.row);
            var right = group.Max (o => o.column);
            var bottom = group.Max (o => o.row);

            var isValid = true;
            for (var y = top; y <= bottom; y++)
            for (var x = left; x <= right; x++)
            {
                isValid = isValid && group.Any (o => o.column == x && o.row == y);
            }

            if (!isValid)
            {
                throw new ArgumentException ($"\"{group.Key}\" is invalid area definition.");
            }

            result.Add (new NamedAreaDefinition (group.Key, top, left, bottom - top + 1, right - left + 1));
        }

        return result;
    }

    private static GridLengthDefinition StringToGridLengthDefinition
        (
            string source
        )
    {
        var r = new System.Text.RegularExpressions.Regex (@"(^[^\(\)]+)(?:\((.*)-(.*)\))?");
        var m = r.Match (source);

        var length = m.Groups[1].Value;
        var min = m.Groups[2].Value;
        var max = m.Groups[3].Value;

        double temp;
        var result = new GridLengthDefinition()
        {
            GridLength = StringToGridLength (length),
            Min = double.TryParse (min, out temp) ? temp : (double?)null,
            Max = double.TryParse (max, out temp) ? temp : (double?)null
        };

        return result;
    }

    private static GridLength StringToGridLength
        (
            string source
        )
    {
        var glc = TypeDescriptor.GetConverter (typeof (GridLength));

        return (GridLength) glc.ConvertFromString (source);
    }

    public static string? GetAreaName
        (
            AvaloniaObject obj
        )
    {
        return (string?) obj.GetValue (AreaNameProperty);
    }

    public static void SetAreaName
        (
            AvaloniaObject obj,
            string value
        )
    {
        obj.SetValue (AreaNameProperty, value);
    }

    public static readonly AvaloniaProperty<string> AreaNameProperty =
        AvaloniaProperty.RegisterAttached<GridEx, Control, string> ("AreaName");

    private static void OnAreaNameChanged
        (
            AvaloniaPropertyChangedEventArgs e
        )
    {
        if (e.Sender is not Control ctrl)
        {
            return;
        }

        if (ctrl.Parent is not Grid _)
        {
            return;
        }

        UpdateItemPosition (ctrl);
    }

    public static string GetArea
        (
            AvaloniaObject obj
        )
    {
        return (string) obj.GetValue (AreaProperty);
    }

    public static void SetArea
        (
            AvaloniaObject obj,
            string value
        )
    {
        obj.SetValue (AreaProperty, value);
    }

    public static readonly AvaloniaProperty<string> AreaProperty =
        AvaloniaProperty.RegisterAttached<GridEx, Control, string> ("Area", null);

    private static void OnAreaChanged
        (
            AvaloniaPropertyChangedEventArgs e
        )
    {
        if (e.Sender is not Control ctrl)
        {
            return;
        }

        if (ctrl.Parent is not Grid _)
        {
            return;
        }

        UpdateItemPosition (ctrl);
    }

    private static void UpdateItemPosition
        (
            Control element
        )
    {
        var area = GetAreaNameRegion (element) ?? GetAreaRegion (element);
        if (area != null)
        {
            Grid.SetRow (element, area.Row);
            Grid.SetColumn (element, area.Column);
            Grid.SetRowSpan (element, area.RowSpan);
            Grid.SetColumnSpan (element, area.ColumnSpan);
        }
    }

    private static AreaDefinition? GetAreaNameRegion (Control element)
    {
        var name = GetAreaName (element);
        var grid = element.Parent as Grid;
        if (grid == null || name == null)
        {
            return null;
        }

        var areaList = GetAreaDefinitions (grid);
        if (areaList == null)
        {
            return null;
        }

        var area = areaList.FirstOrDefault (o => o.Name == name);
        if (area == null)
        {
            return null;
        }

        return new AreaDefinition (area.Row, area.Column, area.RowSpan, area.ColumnSpan);
    }

    private static AreaDefinition? GetAreaRegion
        (
            Control element
        )
    {
        var param = GetArea (element);
        if (param == null)
        {
            return null;
        }

        var list = param.Split (',')
            .Select (o => o.Trim())
            .Select (o => int.Parse (o))
            .ToList();

        // Row, Column, RowSpan, ColumnSpan
        if (list.Count() != 4)
        {
            return null;
        }

        return new AreaDefinition (list[0], list[1], list[2], list[3]);
    }
}
