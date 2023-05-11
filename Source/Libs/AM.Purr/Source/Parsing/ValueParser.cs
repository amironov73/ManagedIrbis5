// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UseNullableAnnotationInsteadOfAttribute

/* ValueParser.cs -- парсер, хранящий результат парсинга
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics.CodeAnalysis;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.Purr.Parsing;

/// <summary>
/// Парсер, хранящий результат парсинга.
/// </summary>
[PublicAPI]
public sealed class ValueParser<TResult>
    : Parser<TResult>
{
    #region Properties

    /// <summary>
    /// Признак успешного разбора.
    /// </summary>
    public bool IsSuccess { get; private set; }

    /// <summary>
    /// Хранимое значение
    /// </summary>
    [MaybeNull]
    public TResult Value { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ValueParser
        (
            IParser<TResult> inner
        )
    {
        Sure.NotNull (inner);

        _inner = inner;
    }

    #endregion

    #region Private members

    private readonly IParser<TResult> _inner;

    #endregion

    #region Parser<TReslt> members

    /// <inheritdoc cref="Parser{TResult}.TryParse"/>
    public override bool TryParse
        (
            ParseState state,
            [MaybeNull] out TResult result
        )
    {
        result = default;

        // состояние отслеживает вложенный парсер
        if (_inner.TryParse (state, out var intermediate))
        {
            Value = intermediate!;
            result = intermediate;
            IsSuccess = true;
            return true;
        }

        return false;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="Parser{TResult}.ToString"/>
    public override string ToString() =>
        _inner + ": " + IsSuccess + ": " + Convert.ToString (Value);

    #endregion
}
