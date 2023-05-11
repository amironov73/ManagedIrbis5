// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* ReturnParser.cs -- парсер, возвращающий всегда одно и то же значение
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.Purr.Parsing;

/// <summary>
/// Парсер, возвращающий всегда одно и то же значение.
/// </summary>
[PublicAPI]
public sealed class ReturnParser<TResult>
    : Parser<TResult>
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

    /// <inheritdoc cref="Parser{TResult}.TryParse"/>
    public override bool TryParse
        (
            ParseState state,
            out TResult result
        )
    {
        using var _ = state.Enter (this);
        result = _result;
        DebugHook (state);

        return DebugSuccess (state, true);
    }

    #endregion
}
