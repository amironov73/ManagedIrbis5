// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* AssertParser.cs -- парсер с проверкой произвольного условия
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.Purr.Parsing;

/// <summary>
/// Парсер с проверкой произвольного условия.
/// </summary>
[PublicAPI]
public sealed class AssertParser<TResult>
    : Parser<TResult>
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public AssertParser
        (
            IParser<TResult> inner,
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

    private readonly IParser<TResult> _inner;
    private readonly Func<TResult, bool> _predicate;
    private readonly string _message;

    #endregion

    #region Parser<TToken, TResult> members

    /// <inheritdoc cref="Parser{TResult}.TryParse"/>
    public override bool TryParse
        (
            ParseState state,
            out TResult result
        )
    {
        using var _ = state.Enter (this);
        result = default!;
        DebugHook (state);

        if (!_inner.TryParse (state, out var temporary))
        {
            return DebugSuccess (state, false);
        }

        if (!_predicate (temporary))
        {
            return DebugSuccess (state, false);
        }

        result = temporary;

        return DebugSuccess (state, true);
    }

    #endregion
}
