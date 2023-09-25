// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* RawStringRecognizer.cs -- распознает сырые строки
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Text;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.Lexey.Tokenizing;

/// <summary>
/// Распознает сырые строки.
/// </summary>
[PublicAPI]
public sealed class RawStringRecognizer
    : ITokenRecognizer
{
    #region Tokenizer members

    /// <inheritdoc cref="Tokenizer.Parse"/>
    public Token? RecognizeToken
        (
            TextNavigator navigator
        )
    {
        Sure.NotNull (navigator);

        var line = navigator.Line;
        var column = navigator.Column;

        if (navigator.PeekChar() != '"'
            || navigator.PeekChar (1) != '"'
            || navigator.PeekChar (2) != '"'
           )
        {
            return null;
        }

        navigator.ReadChar(); // съедаем открывающие кавычки
        navigator.ReadChar();
        navigator.ReadChar();

        var success = false;
        var builder = StringBuilderPool.Shared.Get();
        while (!navigator.IsEOF)
        {
            var chr = navigator.ReadChar();
            if (chr == '"'
                && navigator.PeekChar() == '"'
                && navigator.PeekChar (1) == '"')
            {
                navigator.ReadChar();
                navigator.ReadChar();
                success = true;
                break;
            }

            builder.Append (chr);
        }

        if (!success)
        {
            StringBuilderPool.Shared.Return (builder);
            return null;
        }

        var value = builder.ReturnShared().DosToUnix();
        var result = new Token
            (
                TokenKind.String,
                value,
                line,
                column
            )
            {
                UserData = value
            };

        return result;
    }

    #endregion
}
