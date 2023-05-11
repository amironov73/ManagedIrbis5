// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* FailParser.cs -- вечно фейлящийся парсер
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.Purr.Parsing;

/// <summary>
/// Вечно фейлящийся парсер.
/// Введен "на всякий случай".
/// </summary>
[PublicAPI]
public sealed class FailParser<TResult>
    : Parser<TResult>
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public FailParser
        (
            string message
        )
    {
        Sure.NotNullNorEmpty (message);

        _message = message;
    }

    #endregion

    #region Private members

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

        return DebugSuccess (state, false);
    }

    #endregion
}
