// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* FieldTokenizer.cs -- разбирает спецификацию поля
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Kotik.Tokenizers;

using MicroPft.Ast;

#endregion

#nullable enable

namespace MicroPft.Tokenizers;

/// <summary>
/// Разбирает спецификацию поля.
/// </summary>
internal sealed class FieldTokenizer
    : Tokenizer
{
    #region Tokenizer members

    /// <inheritdoc cref="Tokenizer.Parse"/>
    public override TokenizerResult Parse()
    {
        var line = navigator.Line;
        var column = navigator.Column;
        var position = navigator.SavePosition();
        var chr = char.ToLowerInvariant (PeekChar());
        if (chr is not 'v' and not 'd' and not 'n')
        {
            return TokenizerResult.Error;
        }

        ReadChar();
        var command = chr;
        var tag = 0;
        while (!IsEof)
        {
            chr = PeekChar();
            if (!char.IsDigit (chr))
            {
                break;
            }

            tag = tag * 10 + chr - '0';
            ReadChar();
        }

        if (tag == 0)
        {
            navigator.RestorePosition (position);
            return TokenizerResult.Error;
        }

        var code = '\0';
        if (chr == '^')
        {
            ReadChar();
            if (IsEof)
            {
                navigator.RestorePosition (position);
                return TokenizerResult.Error;
            }

            code = ReadChar();
            chr = PeekChar();
        }

        var offset = 0;
        if (chr == '*')
        {
            if (IsEof)
            {
                navigator.RestorePosition (position);
                return TokenizerResult.Error;
            }

            ReadChar();
            while (!IsEof)
            {
                chr = PeekChar();
                if (!char.IsDigit (chr))
                {
                    break;
                }

                offset = offset * 10 + chr - '0';
                ReadChar();
            }

            if (offset == 0)
            {
                navigator.RestorePosition (position);
                return TokenizerResult.Error;
            }
        }

        var length = 0;
        if (chr == '.')
        {
            if (IsEof)
            {
                navigator.RestorePosition (position);
                return TokenizerResult.Error;
            }

            ReadChar();
            while (!IsEof)
            {
                chr = PeekChar();
                if (!char.IsDigit (chr))
                {
                    break;
                }

                length = length * 10 + chr - '0';
                ReadChar();
            }

            if (length == 0)
            {
                navigator.RestorePosition (position);
                return TokenizerResult.Error;
            }
        }

        if (chr == '*')
        {
            if (offset != 0 || IsEof)
            {
                navigator.RestorePosition (position);
                return TokenizerResult.Error;
            }

            ReadChar();
            while (!IsEof)
            {
                chr = PeekChar();
                if (!char.IsDigit (chr))
                {
                    break;
                }

                offset = offset * 10 + chr - '0';
                ReadChar();
            }

            if (offset == 0)
            {
                navigator.RestorePosition (position);
                return TokenizerResult.Error;
            }
        }

        var textLength = navigator.Position - position;
        var text = navigator.Substring (position, textLength).ToString();
        var token = new Token ("field", text, line, column, offset)
        {
            UserData = new FieldNode (command, tag, code, offset, length)
        };

        return TokenizerResult.Success (token);
    }

    #endregion
}
