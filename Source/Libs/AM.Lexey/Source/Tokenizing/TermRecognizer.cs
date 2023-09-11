// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

/* TermRecognizer.cs -- распознает термы
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Runtime.CompilerServices;

using AM.Text;

using JetBrains.Annotations;

#endregion

// поля, не являющиеся константными, не должны быть видимы
#pragma warning disable CA2211

namespace AM.Lexey.Tokenizing;

/// <summary>
/// Распознает термы.
/// </summary>
[PublicAPI]
public sealed class TermRecognizer
    : ITokenRecognizer
{
    #region Properties

    /// <summary>
    /// Распознаваемые термы.
    /// </summary>
    public static string[] KnownTerms =
    {
        "!", ";", ":", ",", "(", ")", "+", "-", "*", "/", "[", "]",
        "{", "}", "|", "%", "~", "=", "++", "--", "+=", "-=", "*=",
        "/=", "==", "<", ">", "<<", ">>", "<=", ">=", "||", "&&",
        ".", ",", "in", "is", "<=>", "@", "?", "??", "&",
        "!=", "===", "!==", "~~",
    };

    #endregion

    #region Public methods

    /// <summary>
    /// Создание экземпляра.
    /// </summary>
    public static ITokenRecognizer Create() => new TermRecognizer();

    #endregion

    #region Private members

    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    private static bool EqualsAsSpans
        (
            string text1,
            ReadOnlySpan<char> span2
        )
    {
        // встроенный оператор == у ReadOnlySpan:
        // "Returns true if left and right point at the same memory
        // and have the same length.  Note that this does *not*
        // check to see if the *contents* are equal."
        //
        // поэтом он нам не подходит

        return text1.Length == span2.Length && text1.AsSpan().SequenceEqual (span2);
    }

    #endregion

    #region ITermRecognizer

    /// <inheritdoc cref="ITokenRecognizer.RecognizeToken"/>
    public Token? RecognizeToken
        (
            TextNavigator navigator
        )
    {
        Sure.NotNull (navigator);

        string? previousGood = null;
        var line = navigator.Line;
        var column = navigator.Column;
        var offset = navigator.Position;
        Span<char> buffer = stackalloc char[16];
        var builder = new ValueStringBuilder (buffer);

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

            foreach (var known in KnownTerms)
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
                foreach (var known in KnownTerms)
                {
                    if (EqualsAsSpans (known, text))
                    {
                        return MakeToken (known, builder.Length);
                    }
                }
            }

            previousGood = null;
            foreach (var known in KnownTerms)
            {
                if (EqualsAsSpans (known, text))
                {
                    previousGood = known;
                    break;
                }
            }

        }

        Token? MakeToken
            (
                string? tokenValue,
                int length
            )
        {
            if (tokenValue is null)
            {
                return null;
            }

            for (var i = 0; i < length; i++)
            {
                navigator.ReadChar();
            }

            var result = new Token
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

            return result;
        }
    }

    #endregion
}
