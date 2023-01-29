// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* BigIntegerTokenizer.cs -- токенайзер для BigInteger
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Numerics;

using AM.Text;

#endregion

#nullable enable

namespace AM.Kotik;

/// <summary>
/// Токенайзер для <see cref="BigInteger"/>.
/// </summary>
public sealed class BigIntegerTokenizer
    : SubTokenizer
{
    #region SubTokenizer members

    /// <inheritdoc cref="SubTokenizer.Parse"/>
    public override Token? Parse()
    {
        var line = _navigator.Line;
        var column = _navigator.Column;
        var chr = PeekChar();
        if (!chr.IsArabicDigit())
        {
            return null;
        }

        var builder = StringBuilderPool.Shared.Get();
        while (!IsEof)
        {
            chr = PeekChar();

            if (chr is '_')
            {
                ReadChar();
                continue;
            }

            if (!chr.IsArabicDigit())
            {
                break;
            }

            builder.Append (ReadChar());
        }

        chr = PeekChar();
        if (chr is not 'b' and not 'B') // TODO подобрать подходящий суффикс
        {
            StringBuilderPool.Shared.Return (builder);
            return null;
        }

        ReadChar();

        var result = new Token
            (
                "big-integer",
                builder.ReturnShared(),
                line,
                column
            );

        return result;
    }

    #endregion
}
