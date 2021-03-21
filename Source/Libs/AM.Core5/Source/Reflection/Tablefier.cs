// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* Tablefier.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;
using AM.Text;

#endregion

#nullable enable

namespace AM.Reflection
{
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
            public string Title { get; set; }

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
            public PropertyInfo Property { get; set; }
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
            writer.Write(text);
            for (int i = text.Length; i < width; i++)
            {
                writer.Write(' ');
            }
        }

        private void _RightAlign
            (
                TextWriter writer,
                string text,
                int width
            )
        {
            for (int i = text.Length; i < width; i++)
            {
                writer.Write(' ');
            }
            writer.Write(text);
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
            int height = cells.Length;
            for (int i = 0; i < columns.Length; i++)
            {
                int width = columns[i].Title.Length;
                for (int j = 0; j < height; j++)
                {
                    width = Math.Max(width, cells[j][i].Length);
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
            var result = new List<string[]>();
            int length = columns.Length;
            foreach (object? item in items)
            {
                string[] line = new string[length];
                for (int i = 0; i < length; i++)
                {
                    object? value = columns[i].Property.GetValue(item, null);
                    string? text = value.ToVisibleString();
                    line[i] = text;
                }
                result.Add(line);
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
            List<Column> result = new List<Column>();
            PropertyInfo[] properties
                = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (PropertyInfo property in properties)
            {
                var code = Type.GetTypeCode(property.PropertyType);
                int index = 0;
                var moa = ReflectionUtility
                    .GetCustomAttribute<MemberOrderAttribute>(property);
                if (!ReferenceEquals(moa, null))
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
                Column column = new Column
                {
                    Title = property.Name,
                    Index = index,
                    Property = property,
                    RightAlign = rightAlign
                };
                result.Add(column);
            }

            return result
                .OrderBy(column => column.Index)
                .ThenBy(column => column.Title)
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
            for (int i = 0; i < columns.Length; i++)
            {
                if (i != 0)
                {
                    writer.Write(' ');
                }
                _LeftAlign(writer, columns[i].Title, columns[i].Width);
            }
            writer.WriteLine();

            for (int i = 0; i < columns.Length; i++)
            {
                if (i != 0)
                {
                    writer.Write(' ');
                }
                writer.Write(new string('-', columns[i].Width));
            }
            writer.WriteLine();

            for (int i = 0; i < cells.Length; i++)
            {
                for (int j = 0; j < columns.Length; j++)
                {
                    if (j != 0)
                    {
                        writer.Write(' ');
                    }

                    if (columns[j].RightAlign)
                    {
                        _RightAlign(writer, cells[i][j], columns[j].Width);
                    }
                    else
                    {
                        _LeftAlign(writer, cells[i][j], columns[j].Width);
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
            Type type = typeof(T);
            Column[] columns = GetColumns(type);
            string[][] cells = GetCells(items, columns);
            AdjustColumns(columns, cells);
            Print(writer, columns, cells);
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
            if (properties.Length == 0)
            {
                throw new ArgumentException();
            }

            Type type = typeof(T);
            Column[] columns = GetColumns(type);
            if (properties.Any(p => columns.All(c => c.Title != p)))
            {
                throw new ArgumentException();
            }

            columns = columns.Where(c => c.Title.IsOneOf(properties)).ToArray();
            string[][] cells = GetCells(items, columns);
            AdjustColumns(columns, cells);
            Print(writer, columns, cells);
        }

        /// <summary>
        /// Print.
        /// </summary>
        public string Print<T>
            (
                IEnumerable<T> items,
                params string[] properties
            )
        {
            var result = new StringWriter();
            if (ReferenceEquals(properties, null) || properties.Length == 0)
            {
                Print(result, items);
            }
            else
            {
                Print(result, items, properties);
            }

            return result.ToString();
        }

        #endregion
    }
}
