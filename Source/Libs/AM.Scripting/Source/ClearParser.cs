// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* CleanParser.cs -- поглощает ненужные символы (пробелы) перед другим парсером
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics.CodeAnalysis;

using Pidgin;

#endregion

#nullable enable

namespace AM.Scripting;

/// <summary>
/// Поглощае ненужные символы (пробелы) перед другим парсером.
/// Производит "зачистку" территории для другого парсера. :)
/// </summary>
internal sealed class ClearParser<TResult>
    : Parser<char, TResult>
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ClearParser
        (
            Parser<char, TResult> inner
        )
    {
        Sure.NotNull (inner);

        _inner = inner;
    }

    #endregion

    #region Private members

    private readonly Parser<char, TResult> _inner;

    #endregion

    /// <inheritdoc cref="Parser{TToken,TResult}.TryParse"/>
    public override bool TryParse
        (
            ref ParseState<char> state,
            ref PooledList<Expected<char>> expecteds,
            [MaybeNullWhen(false)] out TResult result
        )
    {
        state.EatWhitespace();

        return _inner.TryParse (ref state, ref expecteds, out result);
    }
}
