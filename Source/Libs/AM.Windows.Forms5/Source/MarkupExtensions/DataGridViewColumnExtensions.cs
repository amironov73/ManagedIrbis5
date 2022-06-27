// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
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
            TColumn column,
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
            TColumn column,
            int fillWeight = 0
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
    /// Задание имени для свойства, поставляющего данные.
    /// </summary>
    public static TColumn DataPropertyName<TColumn>
        (
            TColumn column,
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
    /// Замороженная колонка?
    /// </summary>
    public static TColumn Frozen<TColumn>
        (
            TColumn column,
            bool frozen = true
        )
        where TColumn: DataGridViewColumn
    {
        Sure.NotNull (column);

        column.Frozen = frozen;

        return column;
    }

    /// <summary>
    /// Задание текста заголовка.
    /// </summary>
    public static TColumn HeaderText<TColumn>
        (
            TColumn column,
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
    /// Только для чтения?
    /// </summary>
    public static TColumn ReadOnly<TColumn>
        (
            TColumn column,
            bool readOnly = true
        )
        where TColumn: DataGridViewColumn
    {
        Sure.NotNull (column);

        column.ReadOnly = readOnly;

        return column;
    }

    /// <summary>
    /// Тип значений.
    /// </summary>
    public static TColumn ValueType<TColumn>
        (
            TColumn column,
            Type valueType
        )
        where TColumn: DataGridViewColumn
    {
        Sure.NotNull (column);

        column.ValueType = valueType;

        return column;
    }

    #endregion
}
