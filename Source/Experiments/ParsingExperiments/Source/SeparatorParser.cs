// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* SeparatorParser.cs -- поглощает ненужные символы (пробелы и разделители)
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using Pidgin;

#endregion

namespace ParsingExperiments;

/// <summary>
/// Поглощает ненужные символы
/// </summary>
public sealed class SeparatorParser
    : Parser<char, Unit>
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public SeparatorParser
        (
            params char[] additional
        )
    {
        _additional = additional;
    }

    #endregion

    #region Private members

    private readonly char[] _additional;

    #endregion

    #region Parser<TToken, TResult> members

    /// <inheritdoc cref="Parser{TToken,T}.TryParse"/>
    public override bool TryParse
        (
            ref ParseState<char> state,
            ref PooledList<Expected<char>> expecteds,
            out Unit result
        )
    {
        result = Unit.Value;

        if (!state.HasCurrent)
        {
            return false;
        }

        while (state.HasCurrent)
        {
            var chr = state.Current;
            //state.DumpChar();

            if (!char.IsWhiteSpace (chr)
                && Array.IndexOf (_additional, chr) < 0)
            {
                // мы встретили символ-не-разделитель
                break;
            }

            state.Advance();
        }

        return true;
    }

    #endregion
}
