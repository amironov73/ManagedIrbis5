// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* FailParser.cs -- вечно фейлящийся парсер
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics.CodeAnalysis;

#endregion

#nullable enable

namespace AM.Kitten;

/// <summary>
/// Вечно фейлящийся парсер.
/// </summary>
internal sealed class FailParser<TToken, TResult>
    : Parser<TToken, TResult>
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

    /// <inheritdoc cref="Parser{TToken,TResult}.TryParse"/>
    public override bool TryParse
        (
            ParseState<TToken> state,
            [MaybeNullWhen (false)] out TResult result
        )
    {
        result = default;

        return false;
    }

    #endregion
}
