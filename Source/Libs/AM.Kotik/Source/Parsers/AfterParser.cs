// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* AfterParser.cs -- парсит сочетание "нужное после ненужного"
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Kotik;

/// <summary>
/// Парсит сочетание "нужное после ненужного".
/// </summary>
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
            Parser<TResult> parser,
            Parser<TAfter> other
        )
    {
        _other = other;
        _parser = parser;
    }

    #endregion

    #region Private members

    private readonly Parser<TResult> _parser;
    private readonly Parser<TAfter> _other;

    #endregion

    #region Parser<TResult> members

    /// <inheritdoc cref="Parser{TResult}.TryParse"/>
    public override bool TryParse
        (
            ParseState state,
            out TResult result
        )
    {
        result = default!;
        if (!state.HasCurrent)
        {
            return false;
        }

        var location = state.Location;
        if (!_other.TryParse (state, out _))
        {
            state.Location = location;
            return false;
        }

        if (!_parser.TryParse (state, out var temporary))
        {
            state.Location = location;
            return false;
        }

        result = temporary;

        return true;
    }

    #endregion
}
