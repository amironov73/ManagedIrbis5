// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* ExternalTokenizer.cs -- токенайзер для внешнего кода
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Text;

#endregion

#nullable enable

namespace AM.Kotik.Tokenizers;

/// <summary>
/// Токенайзер для внешнего кода.
/// </summary>
public sealed class ExternalTokenizer
    : SubTokenizer
{
    #region SubTokenizer members

    /// <inheritdoc cref="SubTokenizer.Parse"/>
    public override Token? Parse()
    {
        var offset = _navigator.Position;
        var line = _navigator.Line;
        var column = _navigator.Column;

        if (PeekChar() != '`')
        {
            return null;
        }

        ReadChar(); // съедаем открывающую кавычку
        char chr = default;
        var builder = StringBuilderPool.Shared.Get();
        while (!IsEof)
        {
            chr = ReadChar();
            if (chr == '`')
            {
                break;
            }

            builder.Append (chr);
        }

        if (chr != '`')
        {
            StringBuilderPool.Shared.Return (builder);
            throw new SyntaxException (_navigator);
        }

        return new Token 
            (
                TokenKind.External, 
                builder.ReturnShared(), 
                line, 
                column, 
                offset
            );
    }

    #endregion
}
