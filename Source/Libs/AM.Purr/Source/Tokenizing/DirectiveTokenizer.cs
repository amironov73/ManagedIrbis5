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

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.Purr.Tokenizing;

/// <summary>
/// Токенайзер для директив.
/// </summary>
[PublicAPI]
public sealed class DirectiveTokenizer
    : Tokenizer
{
    #region Tokenizer members

    /// <inheritdoc cref="Tokenizer.Parse"/>
    public override TokenizerResult Parse()
    {
        var line = navigator.Line;
        var column = navigator.Column;
        var position = navigator.Position;
        if (PeekChar() != '#')
        {
            return TokenizerResult.Error;
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
            return TokenizerResult.Error;
        }

        ReadChar();
        // обрабатываем специальным образом псевдодирективу `#!`
        var command = navigator.PeekChar() == '!'
            ? "!"
            : navigator.ReadWord().ToString();
        if (string.IsNullOrEmpty (command))
        {
            return TokenizerResult.Error;
        }

        navigator.SkipWhile (' ', '\t');
        var memory = navigator.ReadLine();
        var argument = StringPool.Shared.GetOrAdd (memory.Span);
        var token = new Token
            (
                TokenKind.Directive,
                command,
                line, column,
                position
            )
            {
                UserData = argument
            };

        return TokenizerResult.Success (token);
    }

    #endregion
}
