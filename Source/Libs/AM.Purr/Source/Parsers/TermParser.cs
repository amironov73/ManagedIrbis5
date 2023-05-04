// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* TermParser.cs -- парсит термы
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Collections;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.Purr.Parsers;

/// <summary>
/// Парсит термы.
/// </summary>
[PublicAPI]
public sealed class TermParser
    : Parser<string>
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public TermParser
        (
            params string[] expected
        )
    {
        _expected = expected;
    }

    #endregion

    #region Private members

    private readonly string[]? _expected;

    #endregion

    #region Parser<TResult> members

    /// <inheritdoc cref="Parser{TResult}.TryParse"/>
    public override bool TryParse
        (
            ParseState state,
            out string result
        )
    {
        using var _ = state.Enter (this);
        result = default!;
        DebugHook (state);
        if (!state.HasCurrent)
        {
            return DebugSuccess (state, false);
        }

        var current = state.Current;
        if (!current.IsTerm())
        {
            return DebugSuccess (state, false);
        }

        if (_expected is null)
        {
            result = current.Value!;
            var final = DebugSuccess (state, true);
            state.Advance();
            return final;
        }

        if (current.IsTerm (_expected))
        {
            result = current.Value!;
            var final = DebugSuccess (state, true);
            state.Advance();
            return final;
        }

        return DebugSuccess (state, false);
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        if (_expected.IsNullOrEmpty())
        {
            return GetType().Name;
        }

        var expected = string.Join (", ", _expected);
        return $"{GetType().Name}: {expected}";
    }

    #endregion
}
