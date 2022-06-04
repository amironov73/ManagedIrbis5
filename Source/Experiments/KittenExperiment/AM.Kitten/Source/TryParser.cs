// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* TryParser.cs -- парсер с откатом при неудаче
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics.CodeAnalysis;

#endregion

#nullable enable

namespace AM.Kitten;

/// <summary>
/// Парсер с откатом при неудаче.
/// </summary>
internal sealed class TryParser<TToken, TResult>
    : Parser<TToken, TResult>
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public TryParser
        (
            Parser<TToken, TResult> inner
        )
    {
        Sure.NotNull (inner);

        _inner = inner;
    }

    #endregion

    #region Private members

    private readonly Parser<TToken, TResult> _inner;

    #endregion

    #region Parser<TToken, TResult> members

    /// <inheritdoc cref="Parser{TToken,TResult}.TryParse"/>
    public override bool TryParse
        (
            ParseState<TToken> state,
            [MaybeNullWhen (false)] out TResult result
        )
    {
        state.PushBookmark();
        if (!_inner.TryParse (state, out result))
        {
            state.Rewind();
            return false;
        }

        state.PopBookmark();

        return true;
    }

    #endregion
}
