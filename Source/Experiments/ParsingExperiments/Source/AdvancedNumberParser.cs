// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* AdvancedNumber.cs -- продвинутое целое число, как в C#
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Text;

using Pidgin;

#endregion

namespace ParsingExperiments;

/// <summary>
/// Продвинутое целое число, как в C#, с подчеркиваниями.
/// </summary>
sealed class AdvancedNumberParser
    : Parser<char, string>
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public AdvancedNumberParser
        (
            int @base
        )
    {
        if (@base is < 2 or > 32)
        {
            throw new ArgumentOutOfRangeException (nameof (@base));
        }

        var allowed = new List<char> (@base);
        for (var i = 0; i < @base && i < 10; i++)
        {
            allowed.Add ((char) ('0' + i));
        }

        for (var i = 10; i < @base; i++)
        {
            allowed.Add ((char) ('a' + i - 10));
        }

        _allowed = allowed.ToArray();
    }

    #endregion

    #region Private members

    private readonly char[] _allowed;

    #endregion

    #region Parser<TToken, TResult> members

    /// <inheritdoc cref="Parser{TToken,T}.TryParse"/>
    public override bool TryParse
        (
            ref ParseState<char> state,
            ref PooledList<Expected<char>> expecteds,
            out string result
        )
    {
        result = null!;
        var builder = new StringBuilder();

        if (!state.HasCurrent)
        {
            // опаньки, текст закончился, а мы этого не ждали
            return false;
        }

        if (state.Current == '-')
        {
            builder.Append ('-');
            state.Advance();
        }

        var haveDigit = false;
        while (state.HasCurrent)
        {
            // state.DumpChar();
            var chr = char.ToLowerInvariant (state.Current);
            if (chr != '_')
            {
                if (Array.IndexOf (_allowed, chr) < 0)
                {
                    return false;
                }

                builder.Append (chr);
            }

            state.Advance ();
        }

        if (!haveDigit)
        {
            return false;
        }

        result = builder.ToString();

        return true;
    }

    #endregion
}
