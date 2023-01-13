// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* ChainParser.cs -- парсер для последовательностей
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics.CodeAnalysis;

#endregion

#nullable enable

namespace AM.Kotik;

/// <summary>
/// Парсер для последовательностей.
/// </summary>
public sealed class ChainParser<TResult>
    : Parser<TResult>
    where TResult: class
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ChainParser
        (
            params Parser<TResult>[] parsers
        )
    {
        Sure.AssertState (parsers.Length != 0);

        _parsers = parsers;
    }

    #endregion

    #region Private members

    private readonly Parser<TResult>[] _parsers;

    #endregion

    #region Parser<TResult> members

    /// <inheritdoc cref="Parser{TResult}.TryParse"/>
    public override bool TryParse
        (
            ParseState state,
            out TResult? result
        )
    {
        result = default;

        var location = state.Location;
        foreach (var parser in _parsers)
        {
            if (!parser.TryParse (state, out result))
            {
                state.Location = location;
                return false;
            }

            state.Advance();
        }

        return true;
    }

    #endregion
}
