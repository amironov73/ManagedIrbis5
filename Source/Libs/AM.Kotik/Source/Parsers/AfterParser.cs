// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* AfterParser.cs -- парсит сочетание "нужное после ненужного"
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics.CodeAnalysis;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.Kotik;

/// <summary>
/// Парсит сочетание "нужное после ненужного".
/// Например, "число после скобки", когда число
/// нам нужно, а от скобки требуется лишь её существование.
/// </summary>
[PublicAPI]
public sealed class AfterParser<TAfter, TResult>
    : Parser<TResult>
    where TAfter: class
    where TResult: class
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public AfterParser
        (
            Parser<TResult> useful,
            Parser<TAfter> concomitant
        )
    {
        Sure.NotNull (useful);
        Sure.NotNull (concomitant);

        _concomitant = concomitant;
        _useful = useful;
    }

    #endregion

    #region Private members

    private readonly Parser<TResult> _useful;
    private readonly Parser<TAfter> _concomitant;

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
        if (!_concomitant.TryParse (state, out _))
        {
            state.Location = location;
            return DebugSuccess (state, false);
        }

        if (!_useful.TryParse (state, out var temporary))
        {
            state.Location = location;
            return DebugSuccess (state, false);
        }

        result = temporary;

        return DebugSuccess (state, true);
    }

    #endregion
}
