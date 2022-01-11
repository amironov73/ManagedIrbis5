// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* FieldParser.cs -- разбирает спецификацию поля/подполя
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Scripting;

using Pidgin;

#endregion

#nullable enable

namespace ManagedIrbis.PftLite;

/// <summary>
/// Разбирает спецификацию поля/подполя, т. е.
/// конструкцию <c>"v123^4*5.6"</c>
/// </summary>
internal sealed class FieldParser
    : Parser<char, FieldNode>
{
    #region Parser members

    /// <inheritdoc cref="Parser{TToken,T}.TryParse"/>
    public override bool TryParse
        (
            ref ParseState<char> state,
            ref PooledList<Expected<char>> expecteds,
            out FieldNode result
        )
    {
        result = null!;

        if (!state.HasCurrent)
        {
            return false;
        }

        var chr = char.ToLowerInvariant (state.ReadChar());
        if (chr != 'v' && chr != 'd' && chr != 'n')
        {
            return false;
        }

        var command = chr;
        var tag = 0;
        while (state.HasCurrent)
        {
            chr = state.Current;
            if (!char.IsDigit (chr))
            {
                break;
            }

            tag = tag * 10 + chr - '0';
            state.Advance();
        }

        if (tag == 0)
        {
            return false;
        }

        var code = '\0';
        if (chr == '^')
        {
            state.Advance();
            if (!state.HasCurrent)
            {
                return false;
            }

            code = state.ReadChar();
            chr = state.HasCurrent ? state.Current : '\0';
        }

        var offset = 0;
        if (chr == '*')
        {
            if (!state.HasCurrent)
            {
                return false;
            }

            state.Advance();
            while (state.HasCurrent)
            {
                chr = state.Current;
                if (!char.IsDigit (chr))
                {
                    break;
                }

                offset = offset * 10 + chr - '0';
                state.Advance();
            }

            if (offset == 0)
            {
                return false;
            }
        }

        var length = 0;
        if (chr == '.')
        {
            if (!state.HasCurrent)
            {
                return false;
            }

            state.Advance();
            while (state.HasCurrent)
            {
                chr = state.Current;
                if (!char.IsDigit (chr))
                {
                    break;
                }

                length = length * 10 + chr - '0';
                state.Advance();
            }

            if (length == 0)
            {
                return false;
            }
        }

        if (chr == '*')
        {
            if (offset != 0 || !state.HasCurrent)
            {
                return false;
            }

            state.Advance();
            while (state.HasCurrent)
            {
                chr = state.Current;
                if (!char.IsDigit (chr))
                {
                    break;
                }

                offset = offset * 10 + chr - '0';
                state.Advance();
            }

            if (offset == 0)
            {
                return false;
            }
        }

        result = new FieldNode (command, tag, code, offset, length);

        return true;
    }

    #endregion
}
