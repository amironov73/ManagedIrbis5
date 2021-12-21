// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* CellExtensions.cs -- удобные расширения для работы с ячейками
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Drawing;

using DevExpress.Spreadsheet;

using Range = DevExpress.Spreadsheet.CellRange;

#endregion

#nullable enable

namespace AM.Windows.DevExpress;

/// <summary>
/// Удобные расширения для работы с ячейками Excel-таблиц.
/// </summary>
public static class CellExtensions
{
    /// <summary>
    /// Установка цвета ячейки.
    /// </summary>
    public static Cell TextColor
        (
            this Cell cell,
            Color color
        )
    {
        cell.Font.Color = color;

        return cell;
    }

    /// <summary>
    /// Установка цвета диапазона ячеек.
    /// </summary>
    public static Range TextColor
        (
            this Range range,
            Color color
        )
    {
        range.Font.Color = color;

        return range;
    }

    /// <summary>
    /// Установка цвета строки ячеек.
    /// </summary>
    public static Row TextColor
        (
            this Row row,
            Color color
        )
    {
        row.Font.Color = color;

        return row;
    }

    /// <summary>
    /// Центрирование текста в ячейке.
    /// </summary>
    public static Cell Center
        (
            this Cell cell
        )
    {
        cell.Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
        cell.Alignment.Vertical = SpreadsheetVerticalAlignment.Center;

        return cell;
    }

    /// <summary>
    /// Выделение текста в ячейке жирным.
    /// </summary>
    public static Cell Bold
        (
            this Cell cell
        )
    {
        cell.Font.Bold = true;

        return cell;
    }

    /// <summary>
    /// Выделение текста в диапазоне ячеек жирным.
    /// </summary>
    public static Range Bold
        (
            this Range range
        )
    {
        range.Font.Bold = true;

        return range;
    }

    /// <summary>
    /// Выделение текста во всей строке ячеек жирным.
    /// </summary>
    public static Row Bold
        (
            this Row row
        )
    {
        row.Font.Bold = true;

        return row;
    }

    /// <summary>
    /// Выравнивание текста вправо.
    /// </summary>
    public static Cell RightJustify
        (
            this Cell cell
        )
    {
        cell.Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;

        return cell;
    }

    /// <summary>
    /// Установка границ ячейки.
    /// </summary>
    public static Cell SetBorders
        (
            this Cell cell
        )
    {
        cell.Borders.SetAllBorders (Color.Gray, BorderLineStyle.Thin);

        return cell;
    }

    /// <summary>
    /// Установка границ в диапазоне ячеек.
    /// </summary>
    public static Range SetBorders
        (
            this Range range
        )
    {
        range.Borders.SetAllBorders (Color.Gray, BorderLineStyle.Thin);

        return range;
    }

    /// <summary>
    /// Установка цвета фона ячейки.
    /// </summary>
    public static Cell Background
        (
            this Cell cell,
            Color color
        )
    {
        cell.FillColor = color;

        return cell;
    }

    /// <summary>
    /// Установка цвета фона в диапазоне ячеек.
    /// </summary>
    public static Range Background
        (
            this Range range,
            Color color
        )
    {
        range.FillColor = color;

        return range;
    }

    /// <summary>
    /// Установка цвета фона в строке ячеек.
    /// </summary>
    public static Row Background
        (
            this Row row,
            Color color
        )
    {
        row.FillColor = color;

        return row;
    }

    /// <summary>
    /// Условное форматирование ячеек в два цвета.
    /// </summary>
    public static Range Conditional2Colors
        (
            this Range range,
            Color minColor,
            Color maxColor
        )
    {
        minColor.NotUsed();
        maxColor.NotUsed();

        var conditionalFormattings = range.Worksheet.ConditionalFormattings;
        var minPoint = conditionalFormattings.CreateValue (ConditionalFormattingValueType.MinMax);
        var maxPoint = conditionalFormattings.CreateValue (ConditionalFormattingValueType.MinMax);
        conditionalFormattings.AddColorScale2ConditionalFormatting
            (
                range,
                minPoint,
                minColor,
                maxPoint,
                maxColor
            );

        return range;
    }
}
