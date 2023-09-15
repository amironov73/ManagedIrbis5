// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* HexRecognizer.cs -- распознает числа в шестнадцатеричной системе
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM.Text;

using CommunityToolkit.HighPerformance.Buffers;

using JetBrains.Annotations;

#endregion

namespace AM.Lexey.Tokenizing;

/// <summary>
/// Распознает числа в шестнадцатеричной системе счисления.
/// </summary>
[PublicAPI]
public sealed class HexTokenizer
    : ITokenRecognizer
{
    #region ITokenRecognizer members

    /// <inheritdoc cref="ITokenRecognizer.RecognizeToken"/>
    public Token? RecognizeToken
        (
            TextNavigator navigator
        )
    {
        Sure.NotNull (navigator);

        var offset = navigator.Position;
        var line = navigator.Line;
        var column = navigator.Column;

        // префикс '0x'
        var chr = navigator.PeekChar();
        if (chr != '0' || navigator.LookAhead (1) is not ('x' or 'X'))
        {
            return default;
        }

        Span<char> buffer = stackalloc char[16];
        var builder = new ValueStringBuilder (buffer);
        var kind = TokenKind.Hex32;
        navigator.ReadChar();
        navigator.ReadChar();
        while (!navigator.IsEOF)
        {
            chr = navigator.PeekChar();

            if (chr is '_')
            {
                navigator.ReadChar();
                continue;
            }

            if (!"0123456789ABCDEFabcdef".Contains (chr))
            {
                break;
            }

            builder.Append (navigator.ReadChar());
        }

        if (chr is 'L' or 'l')
        {
            navigator.ReadChar();
            kind = TokenKind.Hex64;
        }

        var span = builder.AsSpan();
        var value = StringPool.Shared.GetOrAdd (span);
        var result = new Token
            (
                kind,
                value,
                line,
                column,
                offset
            )
            {
                UserData = value
            };

        return result;
    }

    #endregion
}
