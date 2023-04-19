// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* FormatTokenizer.cs -- токенайзер для форматных строк
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Text;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.Kotik.Tokenizers;

/// <summary>
/// Токенайзер для форматных строк.
/// </summary>
[PublicAPI]
public sealed class FormatTokenizer
    : Tokenizer
{
    #region Tokenizer members

    /// <inheritdoc cref="Tokenizer.Parse"/>
    public override TokenizerResult Parse()
    {
        var offset = navigator.Position;
        var line = navigator.Line;
        var column = navigator.Column;

        if (PeekChar() != '$' || PeekChar (1) != '"')
        {
            return TokenizerResult.Error;
        }

        ReadChar(); // съедаем доллар
        ReadChar(); // съедаем открывающую кавычку
        char chr = default;
        var builder = StringBuilderPool.Shared.Get();
        while (!IsEof)
        {
            chr = ReadChar();
            if (chr == '\\')
            {
                builder.Append (chr);
                builder.Append (ReadChar());
                continue;
            }

            if (chr == '"')
            {
                break;
            }

            builder.Append (chr);
        }

        if (chr != '"')
        {
            StringBuilderPool.Shared.Return (builder);
            throw new SyntaxException (navigator);
        }

        var value = builder.ReturnShared();
        value = TextUtility.UnescapeText (value);
        var token = new Token
            (
                TokenKind.Format,
                value,
                line,
                column,
                offset
            )
            {
                UserData = value
            };

        return TokenizerResult.Success (token);
    }

    #endregion
}
