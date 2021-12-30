// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* ReturnParser.cs -- парсер, возвращающий всегда одно и то же значение
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics.CodeAnalysis;

#endregion

#nullable enable

namespace AM.Scripting.Kitten;

/// <summary>
/// Парсер, возвращающий всегда одно и то же значение.
/// </summary>
internal sealed class ReturnParser<TToken, TResult>
    : Parser<TToken, TResult>
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ReturnParser
        (
            TResult result
        )
    {
        _result = result;
    }

    #endregion

    #region Private members

    private readonly TResult _result;

    #endregion

    #region Parser<TToken, TResult> members

    /// <inheritdoc cref="Parser{TToken,TResult}.TryParse"/>
    public override bool TryParse
        (
            ParseState<TToken> state,
            [MaybeNullWhen(false)] out TResult result
        )
    {
        result = _result;

        return true;
    }

    #endregion
}
