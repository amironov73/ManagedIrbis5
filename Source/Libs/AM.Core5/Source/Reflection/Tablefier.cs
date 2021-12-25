// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* Tablefier.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using AM.Collections;

#endregion

#nullable enable

namespace AM.Reflection;

/// <summary>
///
/// </summary>
public sealed class Tablefier
{
    #region Inner classes

    /// <summary>
    /// Column.
    /// </summary>
    public class Column
    {
        /// <summary>
        /// Title.
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Index.
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Width.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Align to right?
        /// </summary>
        public bool RightAlign { get; set; }

        /// <summary>
        /// Property info.
        /// </summary>
        public PropertyInfo? Property { get; set; }
    }

    #endregion

    #region Private members

    private void _LeftAlign
        (
            TextWriter writer,
            string text,
            int width
        )
    {
        writer.Write (text);
        for (var i = text.Length; i < width; i++)
        {
            writer.Write (' ');
        }
    }

    private void _RightAlign
        (
            TextWriter writer,
            string text,
            int width
        )
    {
        for (var i = text.Length; i < width; i++)
        {
            writer.Write (' ');
        }

        writer.Write (text);
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Adjust columns.
    /// </summary>
    public void AdjustColumns
        (
            Column[] columns,
            string[][] cells
        )
    {
        Sure.NotNull (columns);
        Sure.NotNull (cells);

        var height = cells.Length;
        for (var i = 0; i < columns.Length; i++)
        {
            var width = columns[i].Title?.Length ?? 8;
            for (var j = 0; j < height; j++)
            {
                width = Math.Max (width, cells[j][i].Length);
            }

            columns[i].Width = width;
        }
    }

    /// <summary>
    /// Get cells.
    /// </summary>
    public string[][] GetCells<T>
        (
            IEnumerable<T> items,
            Column[] columns
        )
    {
        Sure.NotNull ((object?) items);
        Sure.NotNull (columns);

        var result = new List<string[]>();
        var length = columns.Length;
        foreach (var item in items)
        {
            var line = new string[length];
            for (var i = 0; i < length; i++)
            {
                var value = columns[i].Property?.GetValue (item, null);
                var text = value.ToVisibleString();
                line[i] = text;
            }

            result.Add (line);
        }

        return result.ToArray();
    }

    /// <summary>
    /// Get columns.
    /// </summary>
    public Column[] GetColumns
        (
            Type type
        )
    {
        Sure.NotNull (type);

        var result = new List<Column>();
        var properties = type.GetProperties (BindingFlags.Instance | BindingFlags.Public);
        foreach (var property in properties)
        {
            var code = Type.GetTypeCode (property.PropertyType);
            var index = 0;
            var moa = ReflectionUtility
                .GetCustomAttribute<MemberOrderAttribute> (property);
            if (!ReferenceEquals (moa, null))
            {
                index = moa.Index;
            }

            bool rightAlign;
            switch (code)
            {
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Single:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    rightAlign = true;
                    break;

                default:
                    rightAlign = false;
                    break;
            }

            var column = new Column
            {
                Title = property.Name,
                Index = index,
                Property = property,
                RightAlign = rightAlign
            };
            result.Add (column);
        }

        return result
            .OrderBy (column => column.Index)
            .ThenBy (column => column.Title)
            .ToArray();
    }

    /// <summary>
    /// Print the table.
    /// </summary>
    public void Print
        (
            TextWriter writer,
            Column[] columns,
            string[][] cells
        )
    {
        Sure.NotNull (writer);
        Sure.NotNull (columns);
        Sure.NotNull (cells);

        for (var i = 0; i < columns.Length; i++)
        {
            if (i != 0)
            {
                writer.Write (' ');
            }

            _LeftAlign (writer, columns[i].Title ?? string.Empty, columns[i].Width);
        }

        writer.WriteLine();

        for (var i = 0; i < columns.Length; i++)
        {
            if (i != 0)
            {
                writer.Write (' ');
            }

            writer.Write (new string ('-', columns[i].Width));
        }

        writer.WriteLine();

        foreach (var cell in cells)
        {
            for (var j = 0; j < columns.Length; j++)
            {
                if (j != 0)
                {
                    writer.Write (' ');
                }

                if (columns[j].RightAlign)
                {
                    _RightAlign (writer, cell[j], columns[j].Width);
                }
                else
                {
                    _LeftAlign (writer, cell[j], columns[j].Width);
                }
            }

            writer.WriteLine();
        }
    }

    /// <summary>
    /// Print the table.
    /// </summary>
    public void Print<T>
        (
            TextWriter writer,
            IEnumerable<T> items
        )
    {
        Sure.NotNull (writer);
        Sure.NotNull ((object?) items);

        var type = typeof (T);
        var columns = GetColumns (type);
        var cells = GetCells (items, columns);
        AdjustColumns (columns, cells);
        Print (writer, columns, cells);
    }

    /// <summary>
    /// Print the table.
    /// </summary>
    public void Print<T>
        (
            TextWriter writer,
            IEnumerable<T> items,
            string[] properties
        )
    {
        Sure.NotNull (writer);
        Sure.NotNull ((object?) items);
        Sure.NotNull (properties);

        if (properties.Length == 0)
        {
            throw new ArgumentException();
        }

        var type = typeof (T);
        var columns = GetColumns (type);
        if (properties.Any (p => columns.All (c => c.Title != p)))
        {
            throw new ArgumentException();
        }

        columns = columns.Where
                (
                    column =>
                    {
                        var title = column.Title;

                        return !string.IsNullOrEmpty (title) && title.IsOneOf (properties);
                    }
                )
            .ToArray();
        var cells = GetCells (items, columns);
        AdjustColumns (columns, cells);
        Print (writer, columns, cells);
    }

    /// <summary>
    /// Print.
    /// </summary>
    public string Print<T>
        (
            IEnumerable<T> items,
            params string[]? properties
        )
    {
        Sure.NotNull ((object?) items);

        var result = new StringWriter();
        if (properties.IsNullOrEmpty())
        {
            Print (result, items);
        }
        else
        {
            Print (result, items, properties);
        }

        return result.ToString();
    }

    #endregion
}
