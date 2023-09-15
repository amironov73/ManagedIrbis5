// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* BeforeParser.cs -- парсит сочетание "нужное перед ненужным"
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using JetBrains.Annotations;

#endregion

namespace AM.Lexey.Parsing;

/// <summary>
/// Парсит сочетание "нужное перед ненужным".
/// Например, "число перед скобкой", когда число
/// нам нужно, а от скобки требуется лишь её существование.
/// Ещё пример применения - "нечто перед концом текста".
/// </summary>
[PublicAPI]
public sealed class BeforeParser<TBefore, TResult>
    : Parser<TResult>
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public BeforeParser
        (
            IParser<TResult> useful,
            IParser<TBefore> concomitant
        )
    {
        Sure.NotNull (useful);
        Sure.NotNull (concomitant);

        _concomitant = concomitant;
        _useful = useful;
    }

    #endregion

    #region Private members

    private readonly IParser<TResult> _useful;
    private readonly IParser<TBefore> _concomitant;

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

        var location = state.Location;
        if (!_useful.TryParse (state, out var temporary))
        {
            state.Location = location;
            return DebugSuccess (state, false);
        }

        if (!_concomitant.TryParse (state, out _))
        {
            state.Location = location;
            return DebugSuccess (state, false);
        }

        result = temporary!;

        return DebugSuccess (state, true);
    }

    #endregion
}
