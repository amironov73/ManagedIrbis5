// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* SeparatedParser.cs -- парсер для повторяющихся значений, разделенных чем-нибудь
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

#endregion

#nullable enable

namespace AM.Kotik;

/// <summary>
/// Парсер для повторяющихся значений, разделенных между собой чем-нибудь.
/// </summary>
public sealed class SeparatedParser<TResult, TSeparator, TDelimiter>
    : Parser<IEnumerable<TResult>>
    where TResult : class
    where TSeparator : class
    where TDelimiter : class
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public SeparatedParser
        (
            Parser<TResult> itemParser,
            Parser<TSeparator> separatorParser,
            Parser<TDelimiter>? delimiterParser = null,
            int mininum = 0,
            int maximum = int.MaxValue
        )
    {
        Sure.NotNull (itemParser);
        Sure.NotNull (separatorParser);

        _mininum = mininum;
        _maximum = maximum;
        _itemParser = itemParser;
        _separatorParser = separatorParser;
        _delimiterParser = delimiterParser;
    }

    #endregion

    #region Private members

    private readonly int _mininum;
    private readonly int _maximum;
    private readonly Parser<TResult> _itemParser;
    private readonly Parser<TSeparator> _separatorParser;
    private readonly Parser<TDelimiter>? _delimiterParser;

    #endregion

    #region Parser<TResult> members

    /// <inheritdoc cref="Parser{TResult}.TryParse"/>
    public override bool TryParse
        (
            ParseState state,
            [MaybeNullWhen(false)] out IEnumerable<TResult> result
        )
    {
        using var level = state.Enter (this);
        result = default;
        DebugHook (state);

        var list = new List<TResult>();
        var location = state.Location;
        while (state.HasCurrent)
        {
            if (_delimiterParser is not null
                && _delimiterParser.TryParse (state, out _))
            {
                break;
            }

            if (list.Count >= _maximum)
            {
                if (_delimiterParser is not null)
                {
                    state.Location = location;
                    return DebugSuccess (state, false);
                }
                break;
            }

            if (!_itemParser.TryParse (state, out var item))
            {
                if (_delimiterParser is not null)
                {
                    state.Location = location;
                    return DebugSuccess (state, false);
                }
                break;
            }

            list.Add (item);

            if (!_separatorParser.TryParse (state, out _))
            {
                break;
            }
        }

        if (list.Count < _mininum)
        {
            state.Location = location;
            return DebugSuccess (state, false);
        }

        // state продвигается вложенными парсерами
        result = list;

        return DebugSuccess (state, true);
    }

    #endregion
}
