// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* RepeatingParser.cs -- разбирает повторяющийся литерал
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text;

using Pidgin;

#endregion

#nullable enable

namespace ManagedIrbis.PftLite;

/// <summary>
/// Разбирает повторяющийся литерал.
/// </summary>
internal sealed class RepeatingParser
    : Parser<char, RepeatingNode>
{
    #region Parser members

    /// <inheritdoc cref="Parser{TToken,T}.TryParse"/>
    public override bool TryParse
        (
            ref ParseState<char> state,
            ref PooledList<Expected<char>> expecteds,
            out RepeatingNode result
        )
    {
        result = null!;

        if (!state.HasCurrent)
        {
            return false;
        }

        while (state.HasCurrent)
        {
            if (!char.IsWhiteSpace (state.Current))
            {
                break;
            }

            state.Advance();
        }

        if (!state.HasCurrent)
        {
            return false;
        }

        var plus = false;
        if (state.Current == '+')
        {
            plus = true;
            state.Advance();
        }

        if (!state.HasCurrent)
        {
            return false;
        }

        while (state.HasCurrent)
        {
            if (!char.IsWhiteSpace (state.Current))
            {
                break;
            }

            state.Advance();
        }

        if (state.Current != '|')
        {
            return false;
        }

        state.Advance();

        var value = new StringBuilder();
        while (state.HasCurrent)
        {
            if (state.Current == '|')
            {
                break;
            }

            value.Append (state.Current);
            state.Advance();
        }

        if (!state.HasCurrent)
        {
            return false;
        }

        if (state.Current != '|')
        {
            return false;
        }

        state.Advance();

        while (state.HasCurrent)
        {
            if (!char.IsWhiteSpace (state.Current))
            {
                break;
            }

            state.Advance();
        }

        if (state.HasCurrent)
        {
            if (state.Current == '+')
            {
                state.Advance();
                plus = true;
            }
        }

        result = new RepeatingNode (value.ToString(), plus);

        return true;
    }

    #endregion
}
