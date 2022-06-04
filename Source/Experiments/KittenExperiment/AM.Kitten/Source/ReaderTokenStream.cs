// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedType.Global

/* ReaderTokenStream.cs -- поток токенов на базе TextReader
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;

#endregion

#nullable enable

namespace AM.Kitten;

/// <summary>
/// Поток токенов на базе <see cref="TextReader"/>.
/// </summary>
public sealed class ReaderTokenStream
    : ITokenStream<char>
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ReaderTokenStream
        (
            TextReader input
        )
    {
        Sure.NotNull (input);

        _input = input;
    }

    #endregion

    #region Private members

    private readonly TextReader _input;

    #endregion

    #region ITokenStream members

    /// <inheritdoc cref="ITokenStream{TToken}.Read"/>
    public int Read
        (
            Span<char> buffer
        )
    {
        return _input.Read (buffer);
    }

    #endregion
}
