// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* DynamicParser.cs -- парсер, поведение которого может меняться
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics.CodeAnalysis;

#endregion

#nullable enable

namespace AM.Kotik;

/// <summary>
/// Парсер, поведение которого может меняться.
/// Придуман исключительно для разруливания проблемы циклических ссылок
/// при построении сложного парсера на стеке.
/// При построении парсера на статических переменных можно использовать
/// <see cref="LazyParser{TResult}"/>.
/// </summary>
public sealed class DynamicParser<TResult>
    : Parser<TResult>
    where TResult: class
{
    #region Properties

    /// <summary>
    /// Функция, вычисляющая парсер.
    /// Обратите внимание, функцию в любой момент можно поменять.
    /// </summary>
    public Func<Parser<TResult>> Function
    {
        get => _function;
        set
        {
            Sure.NotNull (value);

            _function = value;
        }
    }

    #endregion

    #region Construciton

    /// <summary>
    /// Конструктор.
    /// </summary>
    public DynamicParser
        (
            Func<Parser<TResult>> inner
        )
    {
        Sure.NotNull (inner);

        _function = inner;
    }

    #endregion

    #region Private members

    private Func<Parser<TResult>> _function;

    #endregion

    #region Parser<TResult> members

    /// <inheritdoc cref="Parser{TResult}.TryParse"/>
    public override bool TryParse
        (
            ParseState state,
            [MaybeNullWhen (false)] out TResult result
        )
    {
        result = default;

        return Function().TryParse (state, out result);
    }

    #endregion
}
