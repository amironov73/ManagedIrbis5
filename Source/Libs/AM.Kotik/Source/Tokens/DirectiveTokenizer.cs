// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* DirectiveTokenizer.cs -- токенайзер для директив
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Kotik;

/// <summary>
/// Токенайзер для директив.
/// </summary>
public sealed class DirectiveTokenizer
    : SubTokenizer
{
    #region SubTokenizer members

    /// <inheritdoc cref="SubTokenizer.Parse"/>
    public override Token? Parse()
    {
        var line = _navigator.Line;
        var column = _navigator.Column;
        var position = _navigator.Position;
        if (PeekChar() != '#')
        {
            return null;
        }

        // директива должна быть первым токеном в строке
        // до нее могут быть только пробелы
        var atStart = false;
        var sourceCode = _navigator.Text;
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
            throw new SyntaxException (_navigator);
        }

        ReadChar();
        var command = _navigator.ReadWord().ToString();
        if (string.IsNullOrEmpty (command))
        {
            return null;
        }

        _navigator.SkipWhile (' ', '\t');
        var argument = _navigator.ReadLine().ToString();

        return new Token (TokenKind.Directive, command, line, column)
        {
            UserData = argument
        };
    }

    #endregion
}
