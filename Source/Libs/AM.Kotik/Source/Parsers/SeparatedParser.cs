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

#endregion

#nullable enable

namespace AM.Kotik;

/// <summary>
/// Парсер для повторяющихся значений, разделенных между собой чем-нибудь.
/// </summary>
public sealed class SeparatedParser<TResult, TSeparator>
    : Parser<IEnumerable <TResult>>
    where TResult: class
    where TSeparator: class
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public SeparatedParser
        (
            Parser<TResult> itemParser,
            Parser<TSeparator> separatorParser
        )
    {
        Sure.NotNull (itemParser);
        Sure.NotNull (separatorParser);

        _itemParser = itemParser;
        _separatorParser = separatorParser;
    }

    #endregion

    #region Private members

    private readonly Parser<TResult> _itemParser;
    private readonly Parser<TSeparator> _separatorParser;

    #endregion

    #region Parser<TResult> members

    /// <inheritdoc cref="Parser{TResult}.TryParse"/>
    public override bool TryParse
        (
            ParseState state,
            out IEnumerable <TResult> result
        )
    {
        result = default!;

        var list = new List<TResult>();
        var location = state.Location;
        while (state.HasCurrent)
        {
            if (!_itemParser.TryParse (state, out var item))
            {
                break;
            }

            list.Add (item);

            if (!_separatorParser.TryParse (state, out _))
            {
                break;
            }
        }

        if (list.Count == 0)
        {
            state.Location = location;
            return false;
        }

        result = list;

        return true;
    }

    #endregion
}
