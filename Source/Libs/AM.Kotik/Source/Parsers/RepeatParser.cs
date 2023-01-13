// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* RepeatParser.cs -- парсер для повторяющихся значений
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

#endregion

#nullable enable

namespace AM.Kotik;

/// <summary>
/// Парсер для повторяющихся значений.
/// </summary>
public sealed class RepeatParser<TResult>
    : Parser<IEnumerable<TResult>>
    where TResult: class
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public RepeatParser
        (
            Parser<TResult> parser,
            int count
        )
    {
        Sure.NotNull (parser);
        Sure.Positive (count);

        _parser = parser;
        _count = count;
    }

    #endregion

    #region Private members

    private readonly int _count;

    private readonly Parser<TResult> _parser;

    #endregion

    #region Parser<TResult> members

    /// <inheritdoc cref="Parser{TResult}.TryParse"/>
    public override bool TryParse
        (
            ParseState state,
            out IEnumerable<TResult> result
        )
    {
        result = default!;

        var list = new List<TResult>();
        var location = state.Location;
        for (var i = 0; i < _count; i++)
        {
            if (i != 0 && !state.Advance())
            {
                state.Location = location;
                return false;
            }

            if (!_parser.TryParse (state, out var temporary))
            {
                state.Location = location;
                return false;
            }

            list.Add (temporary);
        }

        result = list;

        return true;
    }

    #endregion
}
