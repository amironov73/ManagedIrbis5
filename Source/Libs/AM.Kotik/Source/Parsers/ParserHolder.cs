// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* ParserHolder.cs -- парсер, поведение которого может меняться
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

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
public sealed class ParserHolder<TResult>
    : Parser<TResult>
    where TResult: class
{
    #region Properties

    /// <summary>
    /// Хранимый парсер.
    /// </summary>
    public Parser<TResult> Value { get; set; }

    #endregion

    #region Construciton

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ParserHolder
        (
            Parser<TResult> inner
        )
    {
        /* Не проверяем на null! */

        Value = inner;
    }

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

        return Value.TryParse (state, out result);
    }

    #endregion
}
