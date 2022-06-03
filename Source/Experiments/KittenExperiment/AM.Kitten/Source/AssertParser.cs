// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* AssertParser.cs -- парсер с проверкой произвольного условия
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics.CodeAnalysis;

using AM;

#endregion

namespace AM.Kitten;

#nullable enable

/// <summary>
/// Парсер с проверкой произвольного условия.
/// </summary>
public sealed class AssertParser<TToken, TResult>
    : Parser<TToken, TResult>
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public AssertParser
        (
            Parser<TToken, TResult> inner,
            Func<TResult, bool> predicate,
            string message
        )
    {
        Sure.NotNull (inner);
        Sure.NotNull (predicate);
        Sure.NotNullNorEmpty (message);

        _inner = inner;
        _predicate = predicate;
        _message = message;
    }

    #endregion

    #region Private members

    private readonly Parser<TToken, TResult> _inner;
    private readonly Func<TResult, bool> _predicate;
    private readonly string _message;

    #endregion

    #region Parser<TToken, TResult> members

    /// <inheritdoc cref="Parser{TToken,TResult}.TryParse"/>
    public override bool TryParse
        (
            ParseState<TToken> state,
            [MaybeNullWhen (false)] out TResult result
        )
    {
        if (!_inner.TryParse (state, out result))
        {
            return false;
        }

        if (!_predicate (result!))
        {
            // state.SetError(Maybe.Nothing<TToken>(), false, state.Location, _message(result!));
            result = default;
            return false;
        }

        return true;
    }

    #endregion
}
