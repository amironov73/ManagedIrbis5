// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* RawStringTokenizer.cs -- токенайзер для сырых строк
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Text;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.Kotik.Tokenizers;

/// <summary>
/// Токенайзер для сырых строк.
/// </summary>
[PublicAPI]
public sealed class RawStringTokenizer
    : Tokenizer
{
    #region Tokenizer members

    /// <inheritdoc cref="Tokenizer.Parse"/>
    public override TokenizerResult Parse()
    {
        var line = navigator.Line;
        var column = navigator.Column;

        if (PeekChar() != '"' || PeekChar (1) != '"'
            || PeekChar (2) != '"')
        {
            return TokenizerResult.Error;
        }

        ReadChar(); // съедаем открывающие кавычки
        ReadChar();
        ReadChar();

        var success = false;
        var builder = StringBuilderPool.Shared.Get();
        while (!IsEof)
        {
            var chr = ReadChar();
            if (chr == '"' && PeekChar() == '"' && PeekChar (1) == '"')
            {
                ReadChar();
                ReadChar();
                success = true;
                break;
            }

            builder.Append (chr);
        }

        if (!success)
        {
            StringBuilderPool.Shared.Return (builder);
            return TokenizerResult.Error;
        }

        var value = builder.ReturnShared();
        var token = new Token
            (
                TokenKind.String,
                value,
                line,
                column
            )
            {
                UserData = value
            };

        return TokenizerResult.Success (token);
    }

    #endregion
}
