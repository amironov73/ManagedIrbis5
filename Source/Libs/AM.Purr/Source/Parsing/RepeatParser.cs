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

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.Purr.Parsing;

/// <summary>
/// Парсер для повторяющихся значений.
/// </summary>
[PublicAPI]
public sealed class RepeatParser<TResult>
    : Parser<IList<TResult>>
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public RepeatParser
        (
            IParser<TResult> parser,
            int minCount = 0,
            int maxCount = int.MaxValue
        )
    {
        Sure.NotNull (parser);
        Sure.NonNegative (minCount);

        _parser = parser;
        _minCount = minCount;
        _maxCount = maxCount;
    }

    #endregion

    #region Private members

    private readonly int _minCount;
    private readonly int _maxCount;

    private readonly IParser<TResult> _parser;

    #endregion

    #region Parser<TResult> members

    /// <inheritdoc cref="Parser{TResult}.TryParse"/>
    public override bool TryParse
        (
            ParseState state,
            out IList<TResult> result
        )
    {
        using var _ = state.Enter (this);
        result = default!;
        DebugHook (state);

        var list = new List<TResult>();
        var location = state.Location;
        for (var i = 0; i < _minCount; i++)
        {
            if (!_parser.TryParse (state, out var temporary))
            {
                state.Location = location;
                return DebugSuccess (state, false);
            }

            list.Add (temporary);
        }

        while (list.Count < _maxCount)
        {
            if (!_parser.TryParse (state, out var temporary))
            {
                break;
            }

            list.Add (temporary);
        }

        result = list;

        return DebugSuccess (state, true);
    }

    #endregion
}
