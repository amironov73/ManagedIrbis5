// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* BigIntegerRecognizer.cs -- распознает BigInteger
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Numerics;

using AM.Text;

using CommunityToolkit.HighPerformance.Buffers;

using JetBrains.Annotations;

#endregion

namespace AM.Lexey.Tokenizing;

/// <summary>
/// Распознает <see cref="BigInteger"/>.
/// Суффикс 'N'.
/// </summary>
[PublicAPI]
public sealed class BigIntegerRecognizer
    : ITokenRecognizer
{
    #region ITokenRecognier members

    /// <inheritdoc cref="ITokenRecognizer.RecognizeToken"/>
    public Token? RecognizeToken
        (
            TextNavigator navigator
        )
    {
        Sure.NotNull (navigator);

        var line = navigator.Line;
        var column = navigator.Column;
        var state = navigator.SaveState();
        var chr = navigator.PeekChar();
        if (!chr.IsArabicDigit())
        {
            return null;
        }

        Span<char> buffer = stackalloc char[16];
        var builder = new ValueStringBuilder (buffer);
        while (!navigator.IsEOF)
        {
            chr = navigator.PeekChar();

            if (chr is '_')
            {
                navigator.ReadChar();
                continue;
            }

            if (!chr.IsArabicDigit())
            {
                break;
            }

            builder.Append (navigator.ReadChar());
        }

        chr = navigator.PeekChar();
        if (chr is not 'n' and not 'N')
        {
            state.Restore();
            return null;
        }

        navigator.ReadChar();
        var span = builder.AsSpan();
        var value = StringPool.Shared.GetOrAdd (span);

        var result = new Token
            (
                TokenKind.BigInteger,
                value,
                line,
                column,
                state.Position
            );

        return result;
    }

    #endregion
}
