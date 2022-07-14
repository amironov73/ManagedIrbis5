// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* DataGridViewColumnExtensions.cs -- методы расширения для DataGridViewColumn
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms.MarkupExtensions;

/// <summary>
/// Методы расширения для <see cref="DataGridViewColumn"/>.
/// </summary>
public static class DataGridViewColumnExtensions
{
    #region Public methods

    /// <summary>
    /// Установка режима автоматического изменения размеров.
    /// </summary>
    public static TColumn AutoSizeMode<TColumn>
        (
            this TColumn column,
            DataGridViewAutoSizeColumnMode mode
        )
        where TColumn: DataGridViewColumn
    {
        Sure.NotNull (column);
        Sure.Defined (mode);

        column.AutoSizeMode = mode;

        return column;
    }

    /// <summary>
    /// Установка режима автоматического изменения размеров.
    /// </summary>
    public static TColumn AutoSizeModeFill<TColumn>
        (
            this TColumn column,
            float fillWeight = 0
        )
        where TColumn: DataGridViewColumn
    {
        Sure.NotNull (column);

        column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        if (fillWeight > 0)
        {
            column.FillWeight = fillWeight;
        }

        return column;
    }

    /// <summary>
    /// Задание шаблона ячеек для колонки.
    /// </summary>
    public static TColumn CellTemplate<TColumn>
        (
            this TColumn column,
            DataGridViewCell template
        )
        where TColumn: DataGridViewColumn
    {
        Sure.NotNull (column);
        Sure.NotNull (template);

        column.CellTemplate = template;

        return column;
    }

    /// <summary>
    /// Контекстное меню для колонки.
    /// </summary>
    public static TColumn ContextMenu<TColumn>
        (
            this TColumn column,
            ContextMenuStrip menuStrip
        )
        where TColumn: DataGridViewColumn
    {
        Sure.NotNull (column);
        Sure.NotNull (menuStrip);

        column.ContextMenuStrip = menuStrip;

        return column;
    }

    /// <summary>
    /// Контекстное меню для колонки.
    /// </summary>
    public static TColumn ContextMenu<TColumn>
        (
            this TColumn column,
            params ToolStripItem[] items
        )
        where TColumn: DataGridViewColumn
    {
        Sure.NotNull (column);
        Sure.NotNull (items);

        column.ContextMenuStrip = new ContextMenuStrip().Items (items);

        return column;
    }

    /// <summary>
    /// Задание имени для свойства, поставляющего данные.
    /// </summary>
    public static TColumn DataPropertyName<TColumn>
        (
            this TColumn column,
            string name
        )
        where TColumn: DataGridViewColumn
    {
        Sure.NotNull (column);
        Sure.NotNullNorEmpty (name);

        column.DataPropertyName = name;

        return column;
    }

    /// <summary>
    /// Стиль по умолчанию для ячеек колонки.
    /// </summary>
    public static TColumn DefaultCellStyle<TColumn>
        (
            this TColumn column,
            DataGridViewCellStyle style
        )
        where TColumn: DataGridViewColumn
    {
        Sure.NotNull (column);
        Sure.NotNull (style);

        column.DefaultCellStyle = style;

        return column;
    }

    /// <summary>
    /// Ширина разделителя в пикселах.
    /// </summary>
    public static TColumn DividerWidth<TColumn>
        (
            this TColumn column,
            int width
        )
        where TColumn: DataGridViewColumn
    {
        Sure.NotNull (column);
        Sure.Positive (width);

        column.DividerWidth = width;

        return column;
    }

    /// <summary>
    /// Относительная ширина колонки.
    /// </summary>
    public static TColumn FillWeight<TColumn>
        (
            this TColumn column,
            float weight
        )
        where TColumn: DataGridViewColumn
    {
        Sure.NotNull (column);
        Sure.Positive (weight);

        column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        column.FillWeight = weight;

        return column;
    }

    /// <summary>
    /// Замороженная колонка?
    /// </summary>
    public static TColumn Frozen<TColumn>
        (
            this TColumn column,
            bool frozen = true
        )
        where TColumn: DataGridViewColumn
    {
        Sure.NotNull (column);

        column.Frozen = frozen;

        return column;
    }

    /// <summary>
    /// Ячейка для заголовка колонки.
    /// </summary>
    public static TColumn HeaderCell<TColumn>
        (
            this TColumn column,
            DataGridViewColumnHeaderCell cell
        )
        where TColumn: DataGridViewColumn
    {
        Sure.NotNull (column);
        Sure.NotNull (cell);

        column.HeaderCell = cell;

        return column;
    }

    /// <summary>
    /// Задание текста заголовка.
    /// </summary>
    public static TColumn HeaderText<TColumn>
        (
            this TColumn column,
            string text
        )
        where TColumn: DataGridViewColumn
    {
        Sure.NotNull (column);
        Sure.NotNullNorEmpty (text);

        column.HeaderText = text;

        return column;
    }

    /// <summary>
    /// Минимальная ширина колонки в пикселах.
    /// </summary>
    public static TColumn MinimumWidth<TColumn>
        (
            this TColumn column,
            int width
        )
        where TColumn: DataGridViewColumn
    {
        Sure.NotNull (column);
        Sure.Positive (width);

        column.MinimumWidth = width;

        return column;
    }

    /// <summary>
    /// Имя колонки.
    /// </summary>
    public static TColumn Name<TColumn>
        (
            this TColumn column,
            string name
        )
        where TColumn: DataGridViewColumn
    {
        Sure.NotNull (column);
        Sure.NotNullNorEmpty (name);

        column.Name = name;

        return column;
    }

    /// <summary>
    /// Только для чтения?
    /// </summary>
    public static TColumn ReadOnly<TColumn>
        (
            this TColumn column,
            bool readOnly = true
        )
        where TColumn: DataGridViewColumn
    {
        Sure.NotNull (column);

        column.ReadOnly = readOnly;

        return column;
    }

    /// <summary>
    /// Можно изменять ширину колонки?
    /// </summary>
    public static TColumn Resizeable<TColumn>
        (
            this TColumn column,
            DataGridViewTriState resizeable
        )
        where TColumn: DataGridViewColumn
    {
        Sure.NotNull (column);

        column.Resizable = resizeable;

        return column;
    }

    /// <summary>
    /// Режим сортировки для колонки.
    /// </summary>
    public static TColumn SortMode<TColumn>
        (
            this TColumn column,
            DataGridViewColumnSortMode sortMode
        )
        where TColumn: DataGridViewColumn
    {
        Sure.NotNull (column);
        Sure.Defined (sortMode);

        column.SortMode = sortMode;

        return column;
    }

    /// <summary>
    /// Текст тултипа.
    /// </summary>
    public static TColumn ToolTipText<TColumn>
        (
            this TColumn column,
            string toolTipText
        )
        where TColumn: DataGridViewColumn
    {
        Sure.NotNull (column);
        Sure.NotNullNorEmpty (toolTipText);

        column.ToolTipText = toolTipText;

        return column;
    }

    /// <summary>
    /// Тип значений.
    /// </summary>
    public static TColumn ValueType<TColumn>
        (
            this TColumn column,
            Type valueType
        )
        where TColumn: DataGridViewColumn
    {
        Sure.NotNull (column);

        column.ValueType = valueType;

        return column;
    }

    /// <summary>
    /// Текущая ширина колонки.
    /// </summary>
    public static TColumn Width<TColumn>
        (
            this TColumn column,
            int width
        )
        where TColumn: DataGridViewColumn
    {
        Sure.NotNull (column);
        Sure.Positive (width);

        column.Width = width;

        return column;
    }

    #endregion
}
