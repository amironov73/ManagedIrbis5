// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* IHunspellLineReader.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

#endregion

#nullable enable

namespace AM.Linguistics.Hunspell;

/// <summary>
/// Defines operations to read affix or dictionary lines from a stream sequentially.
/// </summary>
public interface IHunspellLineReader
{
    /// <summary>
    /// Reads the next line from a stream.
    /// </summary>
    /// <returns>A task that represents the asynchronous read operation. The reult value will contain the contents of the next line as a string or the value <c>null</c> indicating there are no more lines to be read.</returns>
    Task<string?> ReadLineAsync();

    /// <summary>
    /// Reads the next line from a stream.
    /// </summary>
    /// <returns></returns>
    string? ReadLine();

    /// <summary>
    /// Gets the current encoding that the reader is using to decode text.
    /// </summary>
    Encoding CurrentEncoding { get; }
}

/// <summary>
///
/// </summary>
public static class HunspellLineReaderExtensions
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="reader"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static async Task<IEnumerable<string>> ReadLinesAsync
        (
            this IHunspellLineReader reader
        )
    {
        Sure.NotNull (reader);

        var lines = new List<string>();

        while (await reader.ReadLineAsync().ConfigureAwait (false) is { } line)
        {
            lines.Add (line);
        }

        return lines;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="reader"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IEnumerable<string> ReadLines
        (
            this IHunspellLineReader reader
        )
    {
        Sure.NotNull (reader);

        while (reader.ReadLine() is { } line)
        {
            yield return line;
        }
    }
}
