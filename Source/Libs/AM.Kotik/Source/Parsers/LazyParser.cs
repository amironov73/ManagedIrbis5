// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* LazyParser.cs -- парсер с отложенной инициализацией
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics.CodeAnalysis;

#endregion

#nullable enable

namespace AM.Kotik;

/// <summary>
/// Парсер с отложенной инициализацией. Нужен для создания ссылок
/// при определении грамматики в коде.
/// </summary>
public sealed class LazyParser<TResult>
    : Parser<TResult>
    where TResult: class
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public LazyParser
        (
            Func<Parser<TResult>> function
        )
    {
        _lazy = new Lazy<Parser<TResult>> (function);
    }

    #endregion

    #region Private members

    private readonly Lazy<Parser<TResult>> _lazy;

    #endregion

    #region Parser<TResult> members

    /// <inheritdoc cref="Parser{TResult}.TryParse"/>
    public override bool TryParse
        (
            ParseState state,
            [MaybeNullWhen (false)] out TResult result
        )
    {
        using var _ = state.Enter (this);
        DebugHook (state);

        return DebugSuccess (state, _lazy.Value.TryParse (state, out result));
    }

    #endregion
}
