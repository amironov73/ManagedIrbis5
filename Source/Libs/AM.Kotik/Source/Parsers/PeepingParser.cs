// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* PeepingParser.cs -- парсер, подглядывающий на несколько токенов вперед
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics.CodeAnalysis;

#endregion

#nullable enable

namespace AM.Kotik;

/// <summary>
/// Парсер, подглядывающий на несколько токенов вперед.
/// </summary>
public sealed class PeepingParser<TPeep, TResult>
    : Parser<TResult>
    where TPeep: class
    where TResult: class
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="peepFor">Парсер, который должен увидеть нечто важное впереди.</param>
    /// <param name="mainParser">Парсер, выдающий результат.</param>
    public PeepingParser
        (
            Parser<TPeep> peepFor,
            Parser<TResult> mainParser
        )
    {
        Sure.NotNull (peepFor);
        Sure.NotNull (mainParser);

        _peepFor = peepFor;
        _mainParser = mainParser;
    }

    #endregion

    #region Private members

    private readonly Parser<TPeep> _peepFor;
    private readonly Parser<TResult> _mainParser;

    #endregion

    #region Parser<TResult> members

    /// <inheritdoc cref="Parser{TResult}.TryParse"/>
    public override bool TryParse
        (
            ParseState state,
            [MaybeNullWhen (false)] out TResult result
        )
    {
        using var level = state.Enter (this);
        result = default;
        DebugHook (state);
        if (!state.HasCurrent)
        {
            return false;
        }

        var offset = state.Location;
        var flag = false;
        for (var delta = 0; delta < int.MaxValue; delta++)
        {
            state.Location = offset + delta;
            if (_peepFor.TryParse (state, out _))
            {
                flag = true;
                break;
            }
        }

        if (!flag)
        {
            state.Location = offset;
            return DebugSuccess (state, false);
        }

        var slice = state.Slice (offset, state.Location - offset);
        if (_mainParser.TryParse (slice, out result))
        {
            state.Advance();
            return DebugSuccess (state, true);
        }

        state.Location = offset;
        return DebugSuccess (state, false);
    }

    #endregion
}
