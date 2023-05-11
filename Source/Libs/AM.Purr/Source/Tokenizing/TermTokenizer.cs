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

using System;
using System.Runtime.CompilerServices;

using AM.Text;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.Purr.Tokenizing;

/// <summary>
/// Токенайзер для термов.
/// </summary>
[PublicAPI]
public sealed class TermTokenizer
    : Tokenizer
{
    #region Private members

    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    private static bool EqualsAsSpans
        (
            string text1,
            ReadOnlySpan<char> span2
        )
    {
        return text1.Length == span2.Length && text1.AsSpan().SequenceEqual (span2);

        //if (text1.Length != span2.Length)
        //{
        //    return false;
        //}

        //var span1 = text1.AsSpan();
        //for (var i = 0; i < span1.Length; i++)
        //{
        //    if (span1[i] != span2[i])
        //    {
        //        return false;
        //    }
        //}

        //return true;
    }

    #endregion

    #region Tokenizer members

    /// <inheritdoc cref="Tokenizer.Parse"/>
    public override TokenizerResult Parse()
    {
        string? previousGood = null;
        var line = navigator.Line;
        var column = navigator.Column;
        var offset = navigator.Position;
        Span<char> buffer = stackalloc char[16];
        var builder = new ValueStringBuilder (buffer);
        var firstIdentifierLetter = Settings.FirstIdentifierLetter;
        var nextIdentifierLetter = Settings.NextIdentifierLetter;
        var knownTerms = Settings.KnownTerms;

        while (true)
        {
            var chr = navigator.LookAhead (builder.Length);
            if (chr == TextNavigator.EOF)
            {
                return MakeToken (previousGood, builder.Length);
            }

            builder.Append (chr);
            var text = builder.AsSpan();
            var count = 0;

            foreach (var known in knownTerms)
            {
                if (known.AsSpan().StartsWith (text))
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

                return MakeToken (previousGood, builder.Length);
            }

            if (count == 1)
            {
                foreach (var known in knownTerms)
                {
                    if (EqualsAsSpans (known, text))
                    {
                        return MakeToken (known, builder.Length);
                    }
                }
            }

            previousGood = null;
            foreach (var known in knownTerms)
            {
                if (EqualsAsSpans (known, text))
                {
                    previousGood = known;
                    break;
                }
            }
        }

        bool CheckCharIsIdentifier (char chr) =>
            Array.IndexOf (nextIdentifierLetter, chr) >= 0;

        bool CheckTextIsIdentifier (string suspect)
        {
            if (Array.IndexOf (firstIdentifierLetter, suspect[0]) < 0)
            {
                return false;
            }

            for (var i = 1; i < suspect.Length; i++)
            {
                if (Array.IndexOf (nextIdentifierLetter!, suspect[i]) < 0)
                {
                    return false;
                }
            }

            return true;
        }

        TokenizerResult MakeToken (string? tokenValue, int length)
        {
            if (tokenValue is null)
            {
                return TokenizerResult.Error;
            }

            if (CheckTextIsIdentifier (tokenValue)
                && CheckCharIsIdentifier (navigator.LookAhead (tokenValue.Length)))
            {
                // это идентификатор, а не терм
                return TokenizerResult.Error;
            }

            for (var i = 0; i < length; i++)
            {
                ReadChar();
            }

            var token = new Token
                (
                    TokenKind.Term,
                    tokenValue,
                    line,
                    column,
                    offset
                )
                {
                    UserData = tokenValue
                };

            return TokenizerResult.Success (token);
        }
    }

    #endregion
}
