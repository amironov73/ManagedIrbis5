// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* HexTokenizer.cs -- токенайзер для чисел в шестнадцатеричной системе
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text;

#endregion

#nullable enable

namespace AM.Kotik;

/// <summary>
/// Токенайзер для чисел в шестнадцатеричной системе счичления.
/// </summary>
public sealed class HexTokenizer
    : SubTokenizer
{
    #region SubTokenizer members

    /// <inheritdoc cref="SubTokenizer.Parse"/>
    public override Token? Parse()
    {
        var line = _navigator.Line;
        var column = _navigator.Column;
        var builder = new StringBuilder();
        
        // префикс '0x'
        var chr = PeekChar();
        if (chr == '0' && _navigator.LookAhead (1) is 'x' or 'X')
        {
            ReadChar();
            ReadChar();

            var hex = TokenKind.Hex32;
            while (!IsEof)
            {
                chr = PeekChar();

                if (chr is '_')
                {
                    ReadChar();
                    continue;
                }

                if (!"0123456789ABCDEFabcdef".Contains (chr))
                {
                    break;
                }

                builder.Append (ReadChar());
            }

            if (chr is 'L' or 'l')
            {
                ReadChar();
                hex = TokenKind.Hex64;
            }

            return new Token (hex, builder.ToString(), line, column);
        }

        return null;
    }

    #endregion
}
