// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* DirectiveTokenizer.cs -- токенайзер для директив
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using CommunityToolkit.HighPerformance.Buffers;

#endregion

#nullable enable

namespace AM.Kotik.Tokenizers;

/// <summary>
/// Токенайзер для директив.
/// </summary>
public sealed class DirectiveTokenizer
    : Tokenizer
{
    #region Tokenizer members

    /// <inheritdoc cref="Tokenizer.Parse"/>
    public override Token? Parse()
    {
        var line = navigator.Line;
        var column = navigator.Column;
        var position = navigator.Position;
        if (PeekChar() != '#')
        {
            return null;
        }

        // директива должна быть первым токеном в строке
        // до нее могут быть только пробелы
        var atStart = false;
        var sourceCode = navigator.Text;
        for (var lineStart = position - 1;; lineStart--)
        {
            if (lineStart < 0)
            {
                atStart = true;
                break;
            }

            var chr = sourceCode[lineStart];
            if (chr is '\r' or '\n')
            {
                atStart = true;
                break;
            }

            if (!char.IsWhiteSpace (chr))
            {
                break;
            }
        }

        if (!atStart)
        {
            throw new SyntaxException (navigator);
        }

        ReadChar();
        // обрабатываем специальным образом псевдодирективу `#!`
        var command = navigator.PeekChar() == '!'
            ? "!"
            : navigator.ReadWord().ToString();
        if (string.IsNullOrEmpty (command))
        {
            return null;
        }

        navigator.SkipWhile (' ', '\t');
        var memory = navigator.ReadLine();
        var argument = StringPool.Shared.GetOrAdd (memory.Span);

        return new Token (TokenKind.Directive, command, line, column, position)
        {
            UserData = argument
        };
    }

    #endregion
}
