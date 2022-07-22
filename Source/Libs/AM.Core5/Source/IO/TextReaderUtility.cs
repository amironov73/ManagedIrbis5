// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* TextReaderUtility.cs -- вспомогательные методы для чтения текстовых данных
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.IO;
using System.Text;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace AM.IO;

/// <summary>
/// Вспомогательные методы для чтения текстовых данных.
/// </summary>
public static class TextReaderUtility
{
    #region Public methods

    /// <summary>
    /// Open file for reading.
    /// </summary>
    public static StreamReader OpenRead
        (
            string fileName,
            Encoding encoding
        )
    {
        Sure.FileExists (fileName);
        Sure.NotNull (encoding);

        var result = new StreamReader
            (
                File.OpenRead (fileName),
                encoding
            );

        return result;
    }

    /// <summary>
    /// Чтение строки непосредственно в <see cref="StringBuilder"/>.
    /// </summary>
    /// <param name="builder">Куда помещать результат.</param>
    /// <param name="reader">Поток, из которого считывается строка.</param>
    /// <param name="appendNewLine">Добавлять перевод строки в конец?</param>
    /// <returns><c>false</c>, если достигнут конец потока.</returns>
    public static bool ReadLine
        (
            this StringBuilder builder,
            TextReader reader,
            bool appendNewLine = false
        )
    {
        Sure.NotNull (builder);
        Sure.NotNull (reader);

        var first = true;
        while (true)
        {
            var chr = reader.Read();
            if (chr < 0)
            {
                return !first;
            }

            if (chr == '\n')
            {
                if (appendNewLine)
                {
                    builder.Append ((char)chr);
                }

                return true;
            }

            if (chr != '\r')
            {
                builder.Append ((char)chr);
            }

            first = false;
        }
    }

    /// <summary>
    /// Обязательное чтение строки.
    /// </summary>
    public static string RequireLine
        (
            this TextReader reader
        )
    {
        Sure.NotNull (reader);

        var result = reader.ReadLine();
        if (ReferenceEquals (result, null))
        {
            Magna.Logger.LogError
                (
                    nameof (TextReaderUtility)
                    + "::"
                    + nameof (RequireLine)
                    + ": unexpected end of stream"
                );

            throw new ArsMagnaException
                (
                    "Unexpected end of stream"
                );
        }

        return result;
    }

    #endregion
}
