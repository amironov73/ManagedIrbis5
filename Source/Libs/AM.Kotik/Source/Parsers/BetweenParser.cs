// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* BetweenParser.cs -- парсит сочетание "перед - внутри - после"
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

using System.Diagnostics.CodeAnalysis;

namespace AM.Kotik;

/// <summary>
/// Парсит сочетание "перед - внутри - после".
/// Например, "число внутри скобок", когда число
/// нам нужно, а от скобок требуется лишь их существование.
/// </summary>
public sealed class BetweenParser<TBefore, TResult, TAfter>
    : Parser<TResult>
    where TBefore: class
    where TResult: class
    where TAfter: class
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public BetweenParser
        (
            Parser<TBefore> before,
            Parser<TResult> inside,
            Parser<TAfter> after
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

    private readonly Parser<TBefore> _before;
    private readonly Parser<TResult> _inside;
    private readonly Parser<TAfter> _after;

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
            return DebugSuccess (state, false);
        }

        var location = state.Location;
        if (!_before.TryParse (state, out _))
        {
            state.Location = location;
            return DebugSuccess (state, false);
        }

        if (!_inside.TryParse (state, out var temporary))
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
