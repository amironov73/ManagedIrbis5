// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* BeforeParser.cs -- парсит сочетание "нужное перед ненужным"
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

using System.Diagnostics.CodeAnalysis;

namespace AM.Kotik;

/// <summary>
/// Парсит сочетание "нужное перед ненужным".
/// </summary>
public sealed class BeforeParser<TBefore, TResult>
    : Parser<TResult>
    where TBefore: class
    where TResult: class
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public BeforeParser
        (
            Parser<TResult> parser,
            Parser<TBefore> other
        )
    {
        _other = other;
        _parser = parser;
    }

    #endregion

    #region Private members

    private readonly Parser<TResult> _parser;
    private readonly Parser<TBefore> _other;

    #endregion

    #region Parser<TResult> members

    /// <inheritdoc cref="Parser{TResult}.TryParse"/>
    public override bool TryParse
        (
            ParseState state,
            [MaybeNullWhen (false)] out TResult result
        )
    {
        result = default;
        DebugHook (state);
        if (!state.HasCurrent)
        {
            return DebugSuccess (state, false);
        }

        var location = state.Location;
        if (!_parser.TryParse (state, out var temporary))
        {
            state.Location = location;
            return DebugSuccess (state, false);
        }

        if (!_other.TryParse (state, out _))
        {
            state.Location = location;
            return DebugSuccess (state, false);
        }

        // state продвигается вложенными парсерами
        result = temporary;

        return DebugSuccess (state, true);
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="Parser{TResult}.ToString"/>
    public override string ToString() =>
        $"{GetType().Name}: {_parser} {_other}";

    #endregion
}
