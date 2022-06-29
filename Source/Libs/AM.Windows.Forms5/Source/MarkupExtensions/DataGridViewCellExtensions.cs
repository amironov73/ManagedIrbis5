// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* DataGridViewCellExtensions.cs -- методы расширения для DataGridViewCell
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms.MarkupExtensions;

/// <summary>
/// Методы расширения для <see cref="DataGridViewCell"/>.
/// </summary>
public static class DataGridViewCellExtensions
{
    #region Public methods

    /// <summary>
    /// Контекстное меню для ячейки.
    /// </summary>
    public static TDataGridViewCell ContextMenu<TDataGridViewCell>
        (
            this TDataGridViewCell cell,
            ContextMenuStrip menuStrip
        )
        where TDataGridViewCell: DataGridViewCell
    {
        Sure.NotNull (cell);
        Sure.NotNull (menuStrip);

        cell.ContextMenuStrip = menuStrip;

        return cell;
    }

    /// <summary>
    /// Контекстное меню для ячейки.
    /// </summary>
    public static TDataGridViewCell ContextMenu<TDataGridViewCell>
        (
            this TDataGridViewCell cell,
            params ToolStripItem[] items
        )
        where TDataGridViewCell: DataGridViewCell
    {
        Sure.NotNull (cell);
        Sure.NotNull (items);

        cell.ContextMenuStrip = new ContextMenuStrip().Items (items);

        return cell;
    }

    /// <summary>
    /// Сообщение об ошибке для ячейки.
    /// </summary>
    public static TDataGridViewCell ErrorText<TDataGridViewCell>
        (
            this TDataGridViewCell cell,
            string? errorText
        )
        where TDataGridViewCell: DataGridViewCell
    {
        Sure.NotNull (cell);

        cell.ErrorText = errorText;

        return cell;
    }

    /// <summary>
    /// Ячейка только для чтения?
    /// </summary>
    public static TDataGridViewCell ReadOnly<TDataGridViewCell>
        (
            this TDataGridViewCell cell,
            bool readOnly = true
        )
        where TDataGridViewCell: DataGridViewCell
    {
        Sure.NotNull (cell);

        cell.ReadOnly = readOnly;

        return cell;
    }

    /// <summary>
    /// Ячейка выбрана?
    /// </summary>
    public static TDataGridViewCell Selected<TDataGridViewCell>
        (
            this TDataGridViewCell cell,
            bool selected = true
        )
        where TDataGridViewCell: DataGridViewCell
    {
        Sure.NotNull (cell);

        cell.Selected = selected;

        return cell;
    }

    /// <summary>
    /// Задание стиля для ячейки.
    /// </summary>
    public static TDataGridViewCell Style<TDataGridViewCell>
        (
            this TDataGridViewCell cell,
            DataGridViewCellStyle style
        )
        where TDataGridViewCell: DataGridViewCell
    {
        Sure.NotNull (cell);
        Sure.NotNull (style);

        cell.Style = style;

        return cell;
    }

    /// <summary>
    /// Произвольные дополнительные данные.
    /// </summary>
    public static TDataGridViewCell Tag<TDataGridViewCell>
        (
            this TDataGridViewCell cell,
            object? tag
        )
        where TDataGridViewCell: DataGridViewCell
    {
        Sure.NotNull (cell);

        cell.Tag = tag;

        return cell;
    }

    /// <summary>
    /// Текст тултипа.
    /// </summary>
    public static TDataGridViewCell ToolTipText<TDataGridViewCell>
        (
            this TDataGridViewCell cell,
            string text
        )
        where TDataGridViewCell: DataGridViewCell
    {
        Sure.NotNull (cell);
        Sure.NotNullNorEmpty (text);

        cell.ToolTipText = text;

        return cell;
    }

    /// <summary>
    /// Значение, хранящееся в ячейке.
    /// </summary>
    public static TDataGridViewCell Value<TDataGridViewCell>
        (
            this TDataGridViewCell cell,
            object? value
        )
        where TDataGridViewCell: DataGridViewCell
    {
        Sure.NotNull (cell);

        cell.Value = value;

        return cell;
    }

    /// <summary>
    /// Тип значения, хранящегося в ячейке.
    /// </summary>
    public static TDataGridViewCell ValueType<TDataGridViewCell>
        (
            this TDataGridViewCell cell,
            Type valueType
        )
        where TDataGridViewCell: DataGridViewCell
    {
        Sure.NotNull (cell);
        Sure.NotNull (valueType);

        cell.ValueType = valueType;

        return cell;
    }

    #endregion
}
