// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* CharacterRecognizer.cs -- распознает литералы отдельных символов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM.Text;

using JetBrains.Annotations;

#endregion

namespace AM.Lexey.Tokenizing;

/// <summary>
/// Распознает литералы отдельных символов.
/// Отдельные символы выглядят так: 'a', 'b' и т. д.
/// </summary>
[PublicAPI]
public sealed class CharacterRecognizer
    : ITokenRecognizer
{
    #region ITokenizer members

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

        if (navigator.PeekChar() != '\'')
        {
            return null;
        }

        navigator.ReadChar(); // съедаем открывающий апостроф
        var chr = navigator.ReadChar();
        if (chr == '\\')
        {
            if (navigator.IsEOF)
            {
                throw new SyntaxException (navigator);
            }

            Span<char> buffer1 = stackalloc char[8];
            Span<char> buffer2 = stackalloc char[8];
            var builder1 = new ValueStringBuilder (buffer1);
            var builder2 = new ValueStringBuilder (buffer2);
            builder1.Append (chr);
            while (!navigator.IsEOF)
            {
                chr = navigator.PeekChar();
                if (chr == '\'')
                {
                    break;
                }

                navigator.ReadChar();
                builder1.Append (chr);
            }

            var raw = builder1.AsSpan();
            TextUtility.UnescapeText (ref builder2, raw);
            var unescaped = builder2.AsSpan();
            if (unescaped.Length != 1)
            {
                throw new SyntaxException (navigator);
            }
            chr = unescaped[0];
        }

        if (navigator.ReadChar() != '\'')
        {
            return null;
        }

        var value = chr.ToString();
        var offset = state.Position;
        var result = new Token (TokenKind.Char, value, line, column, offset)
        {
            UserData = value
        };

        return result;
    }

    #endregion
}
