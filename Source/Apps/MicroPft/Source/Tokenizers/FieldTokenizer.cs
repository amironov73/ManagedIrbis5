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
    : SubTokenizer
{
    #region SubTokenizer members

    /// <inheritdoc cref="SubTokenizer.Parse"/>
    public override Token? Parse()
    {
        var line = _navigator.Line;
        var column = _navigator.Column;
        var position = _navigator.SavePosition();

        var chr = char.ToLowerInvariant (PeekChar());
        if (chr is not 'v' and not 'd' and not 'n')
        {
            return null;
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
            _navigator.RestorePosition (position);
            return null;
        }

        var code = '\0';
        if (chr == '^')
        {
            ReadChar();
            if (IsEof)
            {
                _navigator.RestorePosition (position);
                return null;
            }

            code = ReadChar();
            chr = PeekChar();
        }

        var offset = 0;
        if (chr == '*')
        {
            if (IsEof)
            {
                _navigator.RestorePosition (position);
                return null;
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
                _navigator.RestorePosition (position);
                return null;
            }
        }

        var length = 0;
        if (chr == '.')
        {
            if (IsEof)
            {
                _navigator.RestorePosition (position);
                return null;
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
                _navigator.RestorePosition (position);
                return null;
            }
        }

        if (chr == '*')
        {
            if (offset != 0 || IsEof)
            {
                _navigator.RestorePosition (position);
                return null;
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
                _navigator.RestorePosition (position);
                return null;
            }
        }

        var textLength = _navigator.Position - position;
        var text = _navigator.Substring (position, textLength).ToString();
        return new Token ("field", text, line, column)
        {
            UserData = new FieldNode (command, tag, code, offset, length)
        };
    }

    #endregion
}
