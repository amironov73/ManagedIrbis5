// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo

/* Table.cs -- собственно таблица
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Text;

using AM.Text;

#endregion

#nullable enable

namespace AM.ConsoleIO.Tables;

/// <summary>
/// Собственно таблица.
/// </summary>
public sealed class Table
{
    #region Properties

    /// <summary>
    /// Колонки в таблице.
    /// </summary>
    public IReadOnlyList<ColumnHeader> Columns => _columns;

    /// <summary>
    /// Строки в таблице.
    /// </summary>
    public IReadOnlyList<object[]> Rows => _rows;

    /// <summary>
    /// Разделитель колонок.
    /// </summary>
    public string Delimiter { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public Table()
    {
        _columns = new List<ColumnHeader>();
        _rows = new List<object[]>();
        Delimiter = "|";
    }

    #endregion

    #region Private members

    private readonly List<ColumnHeader> _columns;
    private readonly List<object[]> _rows;

    /// <summary>
    /// Подсчет ширины колонок.
    /// </summary>
    /// <returns>Массив с шириной колонок.</returns>
    internal int[] CountWidth()
    {
        var result = new int[_columns.Count];
        for (var i = 0; i < _columns.Count; i++)
        {
            var max = _columns[i].ToString().Length;
            foreach (var row in _rows)
            {
                var current = row[i].ToString()?.Length ?? 0;
                if (current > max)
                {
                    max = current;
                }
            }

            result[i] = max;
        }

        return result;
    }

    /// <summary>
    /// Вывод подчеркивания для строки заголовка.
    /// </summary>
    /// <param name="output">Куда помещать результат.</param>
    /// <param name="widths">Массив с шириной колонок.</param>
    internal void DividerLine
        (
            StringBuilder output,
            int[] widths
        )
    {
        foreach (var width in widths)
        {
            output.Append (Delimiter);
            output.Append ('-', width + 2);
        }

        output.Append (Delimiter);
    }

    /// <summary>
    /// Форматирование одной строки с данными.
    /// </summary>
    /// <param name="output">Куда помещать результат.</param>
    /// <param name="widths">Массив с шириной колонок.</param>
    /// <param name="values">Данные для форматирования.</param>
    internal void FormatRow
        (
            StringBuilder output,
            int[] widths,
            IEnumerable<object> values
        )
    {
        var index = 0;
        foreach (var value in values)
        {
            output.Append (Delimiter);
            output.Append (' ');
            var text = value.ToString() ?? string.Empty;
            output.Append (text.PadRight (widths[index]));
            output.Append (' ');
            index++;
        }

        output.Append (Delimiter);
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Добавление колонки в таблицу.
    /// </summary>
    /// <param name="title">Заголовок колонки.</param>
    /// <returns><c>this</c></returns>
    public Table AddColumn
        (
            object title
        )
    {
        Sure.NotNull (title);

        var titleText = title.ToInvariantString().ThrowIfNull();
        _columns.Add (new ColumnHeader (titleText));

        return this;
    }

    /// <summary>
    /// Добавление строки в таблицу.
    /// </summary>
    /// <param name="values">Массив добавляемых значений.</param>
    /// <returns><c>this</c></returns>
    public Table AddRow
        (
            params object[] values
        )
    {
        Sure.NotNull (values);

        _rows.Add (values);

        return this;
    }

    /// <summary>
    /// Формирует таблицу из значений с помощью отражения.
    /// </summary>
    /// <param name="values">Значения для строк таблицы.</param>
    /// <typeparam name="T">Тип значений.</typeparam>
    /// <returns>Построенную таблицу.</returns>
    public static Table From<T>
        (
            IEnumerable<T> values
        )
    {
        var result = new Table();
        var properties = typeof (T).GetProperties();
        foreach (var property in properties)
        {
            result.AddColumn (property.Name);
        }

        foreach (var value in values)
        {
            var row = new object[properties.Length];
            for (var i = 0; i < properties.Length; i++)
            {
                row[i] = properties[i].GetValue (value) ?? string.Empty;
            }

            result.AddRow (row);
        }

        return result;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString" />
    public override string ToString()
    {
        var builder = StringBuilderPool.Shared.Get();
        var widths = CountWidth();
        FormatRow (builder, widths, _columns);
        builder.AppendLine();
        DividerLine (builder, widths);
        builder.AppendLine();
        foreach (var row in _rows)
        {
            FormatRow (builder, widths, row);
            builder.AppendLine();
        }

        var result = builder.ToString();
        StringBuilderPool.Shared.Return (builder);

        return result;
    }

    #endregion
}
