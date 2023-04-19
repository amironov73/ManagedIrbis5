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
    : Tokenizer
{
    #region Tokeninzer members

    /// <inheritdoc cref="Tokenizer.Parse"/>
    public override TokenizerResult Parse()
    {
        var line = navigator.Line;
        var column = navigator.Column;
        var position = navigator.SavePosition();

        var chr = PeekChar();
        var plus = false;
        if (chr == '+')
        {
            plus = true;
            ReadChar();
            navigator.SkipWhitespace();
            chr = PeekChar();
        }

        if (IsEof)
        {
            navigator.RestorePosition (position);
            return TokenizerResult.Error;
        }

        if (chr != '|')
        {
            navigator.RestorePosition (position);
            return TokenizerResult.Error;
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
            navigator.RestorePosition (position);
            return TokenizerResult.Error;
        }

        navigator.SkipWhitespace();
        chr = PeekChar();
        if (chr == '+')
        {
            ReadChar();
            plus = true;
        }

        var textLength = navigator.Position - position;
        var text = navigator.Substring (position, textLength).ToString();
        var token = new Token ("field", text, line, column, position)
        {
            UserData = new RepeatingNode (value.ToString(), plus)
        };

        return TokenizerResult.Success (token);
    }

    #endregion
}
