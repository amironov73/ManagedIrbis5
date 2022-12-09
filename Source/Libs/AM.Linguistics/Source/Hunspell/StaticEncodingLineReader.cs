// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* StaticEncodingLineReader.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM.Linguistics.Hunspell.Infrastructure;

#endregion

#nullable enable

namespace AM.Linguistics.Hunspell;

/// <summary>
///
/// </summary>
public sealed class StaticEncodingLineReader
    : IHunspellLineReader, IDisposable
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="encoding"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public StaticEncodingLineReader
        (
            Stream stream,
            Encoding? encoding
        )
    {
        Sure.NotNull (stream);

        this.stream = stream ?? throw new ArgumentNullException (nameof (stream));
        reader = new StreamReader (stream, encoding ?? Encoding.UTF8, true);
    }

    private readonly Stream stream;
    private readonly StreamReader reader;

    /// <summary>
    ///
    /// </summary>
    public Encoding CurrentEncoding => reader.CurrentEncoding;

    /// <summary>
    ///
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="encoding"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static List<string> ReadLines (string filePath, Encoding encoding)
    {
        if (filePath == null) throw new ArgumentNullException (nameof (filePath));

        using (var stream = FileStreamEx.OpenReadFileStream (filePath))
        using (var reader = new StaticEncodingLineReader (stream, encoding))
        {
            return reader.ReadLines().ToList();
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="encoding"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static async Task<IEnumerable<string>> ReadLinesAsync (string filePath, Encoding encoding)
    {
        if (filePath == null) throw new ArgumentNullException (nameof (filePath));

        using (var stream = FileStreamEx.OpenAsyncReadFileStream (filePath))
        using (var reader = new StaticEncodingLineReader (stream, encoding))
        {
            return await reader.ReadLinesAsync().ConfigureAwait (false);
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public string? ReadLine() => reader.ReadLine();

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public Task<string?> ReadLineAsync() => reader.ReadLineAsync();

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public void Dispose()
    {
        reader.Dispose();
        stream.Dispose();
    }
}
