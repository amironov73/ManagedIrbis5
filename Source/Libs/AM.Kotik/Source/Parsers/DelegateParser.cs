// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* DelegateParser.cs -- парсер, поведение которого задается делегатом
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics.CodeAnalysis;

#endregion

#nullable enable

namespace AM.Kotik;

/// <summary>
/// Парсер, поведение которого задается делегатом.
/// </summary>
public sealed class DelegateParser<TResult>
    : Parser<TResult>
    where TResult: class
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public DelegateParser
        (
            ParserDelegate<TResult> function
        )
    {
        Sure.NotNull (function);

        _function = function;
    }

    #endregion

    #region Private members

    private readonly ParserDelegate<TResult> _function;

    #endregion

    #region Parser<TResult> members

    /// <inheritdoc cref="Parser{TResult}.TryParse"/>
    public override bool TryParse
        (
            ParseState state,
            [MaybeNullWhen(false)] out TResult result
        )
    {
        return _function (state, out result);
    }

    #endregion
}
