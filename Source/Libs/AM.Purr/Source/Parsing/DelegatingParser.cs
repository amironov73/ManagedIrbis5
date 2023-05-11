// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* DelegateParser.cs -- парсер, поведение которого задается делегатом
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.Purr.Parsing;

/// <summary>
/// Парсер, поведение которого задается делегатом.
/// </summary>
[PublicAPI]
public sealed class DelegatingParser<TResult>
    : Parser<TResult>
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public DelegatingParser
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
            out TResult result
        )
    {
        return _function (state, out result);
    }

    #endregion
}
