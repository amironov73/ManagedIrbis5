// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

/* TextWriterUtility.cs -- вспомогательные методы для записи текстовых данных
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.IO;
using System.Text;

#endregion

#nullable enable

namespace AM.IO;

/// <summary>
/// Вспомогательные методы для записи текстовых данных.
/// </summary>
public static class TextWriterUtility
{
    #region Public methods

    /// <summary>
    /// Open file for append.
    /// </summary>
    public static StreamWriter Append
        (
            string fileName,
            Encoding encoding
        )
    {
        Sure.NotNullNorEmpty (fileName);
        Sure.NotNull (encoding);

        var result = new StreamWriter
            (
                new FileStream (fileName, FileMode.Append),
                encoding
            );

        return result;
    }

    /// <summary>
    /// Open file for writing.
    /// </summary>
    public static StreamWriter Create
        (
            string fileName,
            Encoding encoding
        )
    {
        Sure.NotNullNorEmpty (fileName);
        Sure.NotNull (encoding);

        var result = new StreamWriter
            (
                new FileStream (fileName, FileMode.Create),
                encoding
            );

        return result;
    }

    /// <summary>
    /// Добавление объекта-перечисления.
    /// </summary>
    public static TextWriter WriteEnumerable
        (
            this TextWriter writer,
            IEnumerable<string> sequence,
            string? separator = ", ",
            string? open = "(",
            string? close = ")"
        )
    {
        var first = true;
        writer.Write (open);
        foreach (var item in sequence)
        {
            if (!first)
            {
                writer.Write (separator);
            }

            writer.Write (item);
            first = false;
        }

        writer.Write (close);

        return writer;
    }


    /// <summary>
    /// Добавление списка по принципу "первый, второй и последний".
    /// </summary>
    public static TextWriter WriteList
        (
            this TextWriter writer,
            IReadOnlyList<string> list,
            string? separator = ", ",
            string? union = " and "
        )
    {
        Sure.NotNull (writer);

        var count = list.Count;
        for (var index = 0; index < count; index++)
        {
            if (index == count - 1 && count > 1)
            {
                writer.Write (union);
            }
            else if (index != 0)
            {
                writer.Write (separator);
            }

            writer.Write (list[index]);
        }

        return writer;
    }

    /// <summary>
    /// Вывод с разделителем.
    /// </summary>
    public static bool WriteSeparated
        (
            this TextWriter writer,
            string? separator,
            string? text,
            bool wasSeparated
        )
    {
        Sure.NotNull (writer);

        if (string.IsNullOrEmpty (text))
        {
            return wasSeparated;
        }

        if (wasSeparated)
        {
            writer.Write (separator);
        }

        writer.Write (text);

        return true;
    }

    /// <summary>
    /// Вывод элементов с разделителем.
    /// </summary>
    public static TextWriter WriteSeparated
        (
            this TextWriter writer,
            string? separator,
            IEnumerable<string?> list
        )
    {
        Sure.NotNull (writer);
        Sure.NotNull (list);

        var was = false;
        foreach (var text in list)
        {
            was = WriteSeparated (writer, separator, text, was);
        }

        return writer;
    }

    /// <summary>
    /// Вывод элементов с разделителем.
    /// </summary>
    public static TextWriter WriteSeparated
        (
            this TextWriter writer,
            string? separator,
            string? first,
            string? second
        )
    {
        Sure.NotNull (writer);

        var was = WriteSeparated (writer, separator, first, false);
        WriteSeparated (writer, separator, second, was);

        return writer;
    }

    /// <summary>
    /// Вывод элементов с разделителем.
    /// </summary>
    public static TextWriter WriteSeparated
        (
            this TextWriter writer,
            string? separator,
            string? first,
            string? second,
            string? third
        )
    {
        Sure.NotNull (writer);

        var was = WriteSeparated (writer, separator, first, false);
        was = WriteSeparated (writer, separator, second, was);
        WriteSeparated (writer, separator, third, was);

        return writer;
    }

    #endregion
}
