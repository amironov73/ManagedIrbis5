// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* ConditionalTokenizer.cs -- разбирает условный литерал
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Kotik.Tokenizers;

using MicroPft.Ast;

#endregion

#nullable enable

namespace MicroPft.Tokenizers;

/// <summary>
/// Разбирает условный литерал.
/// </summary>
public sealed class ConditionalTokenizer
    : Tokenizer
{
    #region Tokeninzer members

    /// <inheritdoc cref="Tokenizer.Parse"/>
    public override Token? Parse()
    {
        var line = navigator.Line;
        var column = navigator.Column;
        var position = navigator.SavePosition();

        var chr = PeekChar();
        if (chr != '"')
        {
            navigator.RestorePosition (position);
            return null;
        }

        ReadChar();
        while (!IsEof)
        {
            chr = ReadChar();
            if (chr == '"')
            {
                break;
            }
        }

        if (chr != '"')
        {
            navigator.RestorePosition (position);
            return null;
        }

        var textLength = navigator.Position - position - 2;
        var text = navigator.Substring (position + 1, textLength).ToString();
        return new Token ("conditional", text, line, column, position)
        {
            UserData = new ConditionalNode (text)
        };
    }

    #endregion
}
