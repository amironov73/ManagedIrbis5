// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* TermTokenizer.cs -- токенайзер для термов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text;

using AM.Text;

#endregion

#nullable enable

namespace AM.Kotik.Tokenizers;

/// <summary>
/// Токенайзер для термов.
/// </summary>
public sealed class TermTokenizer
    : Tokenizer
{
    #region SubTokenizer members

    /// <inheritdoc cref="Tokenizer.Parse"/>
    public override Token? Parse()
    {
        string? previousGood = null;
        var line = navigator.Line;
        var column = navigator.Column;
        var offset = navigator.Position;
        var builder = new StringBuilder();
        var knownTerms = Settings.KnownTerms;
        while (true)
        {
            var chr = navigator.LookAhead (builder.Length);
            if (chr == TextNavigator.EOF)
            {
                return MakeToken (previousGood);
            }

            builder.Append (chr);
            var text = builder.ToString();
            var count = 0;

            foreach (var known in knownTerms)
            {
                if (known.StartsWith (text))
                {
                    count++;
                }
            }

            if (count == 0)
            {
                if (builder.Length != 0)
                {
                    builder.Length--;
                }

                return MakeToken (previousGood);
            }

            if (count == 1)
            {
                foreach (var known in knownTerms)
                {
                    if (known == text)
                    {
                        return MakeToken (known);
                    }
                }
            }

            previousGood = null;
            foreach (var known in knownTerms)
            {
                if (known == text)
                {
                    previousGood = known;
                    break;
                }
            }
        }

        Token? MakeToken (string? tokenValue)
        {
            if (tokenValue is null)
            {
                return null;
            }

            for (var i = 0; i < builder.Length; i++)
            {
                ReadChar();
            }

            return new Token
                (
                    TokenKind.Term,
                    tokenValue,
                    line,
                    column,
                    offset
                );
        }
    }

    #endregion
}
