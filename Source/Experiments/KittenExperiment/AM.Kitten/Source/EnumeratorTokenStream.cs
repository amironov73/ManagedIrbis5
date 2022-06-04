// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedType.Global

/* EnumeratorTokenStream.cs -- поток токенов на базе IEnumerator
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

#endregion

#nullable enable

namespace AM.Kitten;

/// <summary>
/// Поток токенов на базе <see cref="IEnumerator{T}"/>
/// </summary>
public sealed class EnumeratorTokenStream<TToken>
    : ITokenStream<TToken>
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public EnumeratorTokenStream
        (
            IEnumerator<TToken> input
        )
    {
        Sure.NotNull (input);

        _input = input;
    }

    #endregion

    #region Private members

    private readonly IEnumerator<TToken> _input;

    #endregion

    #region ITokenStream members

    /// <inheritdoc cref="ITokenStream{TToken}.Read"/>
    public int Read
        (
            Span<TToken> buffer
        )
    {
        for (var i = 0; i < buffer.Length; i++)
        {
            var hasNext = _input.MoveNext();
            if (!hasNext)
            {
                return i;
            }

            buffer[i] = _input.Current;
        }

        return buffer.Length;
    }

    #endregion
}
