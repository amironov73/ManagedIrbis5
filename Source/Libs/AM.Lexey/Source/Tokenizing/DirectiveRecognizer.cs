// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* DirectiveRecognizer.cs -- распознает директивы
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using CommunityToolkit.HighPerformance.Buffers;

using AM.Text;

using JetBrains.Annotations;

#endregion

namespace AM.Lexey.Tokenizing;

/// <summary>
/// Распознает директивы. Директива имеет вид
/// <code>
/// #имя аргументы
/// </code>
/// Еще предусмотрена псевдодиректива
/// <code>
/// #!/usr/bin/env barsik
/// </code>
/// для совместимости с shebang.
/// </summary>
[PublicAPI]
public sealed class DirectiveRecognizer
    : ITokenRecognizer
{
    #region Constants

    /// <summary>
    /// Признак начала директивы.
    /// </summary>
    public const char DirectiveStartSign = '#';

    #endregion

    #region ITokenRecognizer members

    /// <inheritdoc cref="ITokenRecognizer.RecognizeToken"/>
    public Token? RecognizeToken
        (
            TextNavigator navigator
        )
    {
        Sure.NotNull (navigator);

        var line = navigator.Line;
        var column = navigator.Column;
        var offset = navigator.Position;

        if (navigator.PeekChar() != DirectiveStartSign)
        {
            return default;
        }

        // директива должна быть первым токеном в строке
        // до нее могут быть только пробелы
        var atStart = false;
        var sourceCode = navigator.Text;
        for (var lineStart = offset - 1;; lineStart--)
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
            // не в начале строки не считается
            return default;
        }

        navigator.ReadChar();
        // обрабатываем специальным образом псевдодирективу `#!`
        var command = navigator.PeekChar() == '!'
            ? "!"
            : navigator.ReadWord().ToString();
        if (string.IsNullOrEmpty (command))
        {
            return default;
        }

        navigator.SkipWhile (' ', '\t');
        var memory = navigator.ReadLine();
        var argument = StringPool.Shared.GetOrAdd (memory.Span);
        var result = new Token
            (
                TokenKind.Directive,
                command,
                line, column,
                offset
            )
            {
                UserData = argument
            };

        return result;
    }

    #endregion
}
