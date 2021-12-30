// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* StreamTokenStream.cs -- поток токенов на базе Stream
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;

#endregion

#nullable enable

namespace AM.Scripting.Kitten;

/// <summary>
/// Поток токенов на базе <see cref="Stream"/>.
/// </summary>
public sealed class StreamTokenStream
    : ITokenStream<byte>
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public StreamTokenStream
        (
            Stream input
        )
    {
        Sure.NotNull (input);

        _input = input;
    }

    #endregion

    #region Private members

    private readonly Stream _input;

    #endregion

    #region ITokenStream members

    /// <inheritdoc cref="ITokenStream{TToken}.Read"/>
    public int Read
        (
            Span<byte> buffer
        )
    {
        return _input.Read (buffer);
    }

    #endregion
}
