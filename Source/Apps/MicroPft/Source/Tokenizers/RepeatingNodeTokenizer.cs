// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* RepeatingNodeTokenizer.cs -- разбирает повторяющийся литерал
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text;

using AM.Kotik.Tokenizers;

using MicroPft.Ast;

#endregion

#nullable enable

namespace MicroPft.Tokenizers;

/// <summary>
/// Разбирает повторяющийся литерал.
/// </summary>
public sealed class RepeatingNodeTokenizer
    : SubTokenizer
{
    #region SubTokeninzer members

    /// <inheritdoc cref="SubTokenizer.Parse"/>
    public override Token? Parse()
    {
        var line = _navigator.Line;
        var column = _navigator.Column;
        var position = _navigator.SavePosition();

        var chr = PeekChar();
        var plus = false;
        if (chr == '+')
        {
            plus = true;
            ReadChar();
            _navigator.SkipWhitespace();
            chr = PeekChar();
        }

        if (IsEof)
        {
            _navigator.RestorePosition (position);
            return null;
        }

        if (chr != '|')
        {
            _navigator.RestorePosition (position);
            return null;
        }

        ReadChar();

        var value = new StringBuilder();
        while (!IsEof)
        {
            chr = ReadChar();
            if (chr == '|')
            {
                break;
            }

            value.Append (chr);
        }

        if (chr != '|')
        {
            _navigator.RestorePosition (position);
            return null;
        }

        _navigator.SkipWhitespace();
        chr = PeekChar();
        if (chr == '+')
        {
            ReadChar();
            plus = true;
        }

        var textLength = _navigator.Position - position;
        var text = _navigator.Substring (position, textLength).ToString();
        return new Token ("field", text, line, column)
        {
            UserData = new RepeatingNode (value.ToString(), plus)
        };
    }

    #endregion
}
