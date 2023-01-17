// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* ParserInstance.cs -- обертка над парсером
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics.CodeAnalysis;

#endregion

#nullable enable

namespace AM.Kotik;

/// <summary>
/// Обертка над парсером (для облегчения отладки).
/// </summary>
public sealed class ParserInstance<TResult>
    : Parser<TResult>
    where TResult: class
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ParserInstance
        (
            Parser<TResult> inner,
            string label
        )
    {
        Sure.NotNullNorEmpty (label);

        _inner = inner;
        Label = label;
    }

    #endregion

    #region Private members

    private readonly Parser<TResult> _inner;

    #endregion

    #region Parser<TResult> members

    /// <inheritdoc cref="Parser{TResult}.TryParse"/>
    public override bool TryParse
        (
            ParseState state,
            [MaybeNullWhen (false)] out TResult result
        )
    {
        return _inner.TryParse (state, out result);
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="Parser{TResult}.ToString"/>
    public override string ToString() => Label!;

    #endregion
}
