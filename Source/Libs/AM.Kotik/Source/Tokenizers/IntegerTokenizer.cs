// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* IntegerTokenizer.cs -- токенайзер для целых чисел
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Text;

#endregion

#nullable enable

namespace AM.Kotik.Tokenizers;

/// <summary>
/// Токенайзер для целых чисел.
/// </summary>
public sealed class IntegerTokenizer
    : Tokenizer
{
    /// <inheritdoc cref="Tokenizer.Parse"/>
    public override Token? Parse()
    {
        var offset = navigator.Position;
        var line = navigator.Line;
        var column = navigator.Column;

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

        var isLong = false;
        var isUnsigned = false;
        while (!IsEof)
        {
            chr = PeekChar();
            if (chr is 'l' or 'L')
            {
                if (isLong)
                {
                    StringBuilderPool.Shared.Return (builder);
                    throw new SyntaxException (navigator);
                }

                ReadChar();
                isLong = true;
            }
            else if (chr is 'u' or 'U')
            {
                if (isUnsigned)
                {
                    StringBuilderPool.Shared.Return (builder);
                    throw new SyntaxException (navigator);
                }

                ReadChar();
                isUnsigned = true;
            }
            else if (chr is '.' or 'e' or 'E' or 'f' or 'F' or 'm' or 'M')
            {
                // это число с плавающей (или фиксированной) точкой
                StringBuilderPool.Shared.Return (builder);
                navigator.RestorePosition (offset);
                return null;
            }
            else
            {
                break;
            }
        }

        var kind = isLong
            ? isUnsigned
                ? TokenKind.UInt64 : TokenKind.Int64
            : isUnsigned
                ? TokenKind.UInt32 : TokenKind.Int32;

        var result = new Token
            (
                kind,
                builder.ReturnShared(),
                line,
                column,
                offset
            );

        return result;
    }
}
