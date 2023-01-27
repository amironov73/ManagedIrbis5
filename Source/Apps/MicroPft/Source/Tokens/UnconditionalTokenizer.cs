// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* UnconditionalTokenizer.cs -- разбирает безусловный литерал
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Kotik;

using MicroPft.Ast;

#endregion

#nullable enable

namespace MicroPft.Tokens;

/// <summary>
/// Разбирает безусловный литерал.
/// </summary>
public sealed class UnconditionalTokenizer
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
        if (chr != '\'')
        {
            _navigator.RestorePosition (position);
            return null;
        }

        ReadChar();
        while (!IsEof)
        {
            chr = ReadChar();
            if (chr == '\'')
            {
                break;
            }
        }

        if (chr != '\'')
        {
            _navigator.RestorePosition (position);
            return null;
        }

        var textLength = _navigator.Position - position - 2;
        var text = _navigator.Substring (position + 1, textLength).ToString();
        return new Token ("field", text, line, column)
        {
            UserData = new UnconditionalNode (text)
        };
    }

    #endregion
}
