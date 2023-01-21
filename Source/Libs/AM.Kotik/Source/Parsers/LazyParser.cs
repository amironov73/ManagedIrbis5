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
/// при определении грамматики на статических переменных.
/// Дело в том, что переменные инициализируются по одной, и к моменту,
/// когда нам понадобится сослаться на некий парсер, соответствующая
/// переменная может быть ещё не проинициализирована (компилятор,
/// к его чести, сообщает об этом), так что мы можем получить
/// <see cref="NullReferenceException"/>. Если сослаться на ленивую
/// обертку, то проблема пропрадает.
/// </summary>
/// <example>
/// <code>
/// static variable AtomParser Atom = (AtomParser) Parser.OneOf
///    (
///         new LazyParser (() =&gt; Expression),
///         new BracketsParser (() =&gt; Expression),
///    );
///
/// static variable AtomParser Expression = new ExpressionParser
///    (
///         newLazyParser (() =&gt; Atom),
///         new KnownOperations ("+", "-", "*", "/")
///    );
///
/// </code>
/// </example>
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
        Sure.NotNull (function);
        
        _function = new Lazy<Parser<TResult>> (function);
    }

    #endregion

    #region Private members

    private readonly Lazy<Parser<TResult>> _function;

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

        return DebugSuccess (state, _function.Value.TryParse (state, out result));
    }

    #endregion
}
