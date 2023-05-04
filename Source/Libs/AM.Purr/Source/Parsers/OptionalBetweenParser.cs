// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* OptionalBetweenParser.cs -- парсит опционональное сочетание "перед - внутри - после"
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.Purr.Parsers;

/// <summary>
/// Парсит сочетание "перед - внутри - после".
/// Например, "число внутри скобок", когда число
/// нам нужно, а от скобок требуется лишь их существование.
/// </summary>
[PublicAPI]
public class OptionalBetweenParser<TBefore, TResult, TAfter>
    : Parser<TResult>
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public OptionalBetweenParser
        (
            IParser<TBefore> before,
            IParser<TResult> inside,
            IParser<TAfter> after
        )
    {
        Sure.NotNull (before);
        Sure.NotNull (inside);
        Sure.NotNull (after);

        _before = before;
        _inside = inside;
        _after = after;
    }

    #endregion

    #region Private members

    private readonly IParser<TBefore> _before;
    private readonly IParser<TResult> _inside;
    private readonly IParser<TAfter> _after;

    #endregion

    #region Parser<TResult> members

    /// <inheritdoc cref="Parser{TResult}.TryParse"/>
    public override bool TryParse
        (
            ParseState state,
            out TResult result
        )
    {
        using var level = state.Enter (this);
        result = default!;
        DebugHook (state);
        if (!state.HasCurrent)
        {
            return DebugSuccess (state, false);
        }

        TResult? temporary;
        var location = state.Location;
        if (!_before.TryParse (state, out _))
        {
            state.Location = location;
            if (!_inside.TryParse (state, out temporary))
            {
                state.Location = location;
                return DebugSuccess (state, false);
            }

            result = temporary;

            return DebugSuccess (state, true);
        }

        if (!_inside.TryParse (state, out temporary))
        {
            state.Location = location;
            return DebugSuccess (state, false);
        }

        if (!_after.TryParse (state, out _))
        {
            state.Location = location;
            return DebugSuccess (state, false);
        }

        result = temporary;

        return DebugSuccess (state, true);
    }

    #endregion
}
