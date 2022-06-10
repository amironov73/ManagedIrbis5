// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

/* BarsikIdentifier.cs -- парсер барсиковых идентификаторов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using Pidgin;

#endregion

#nullable enable

namespace AM.Scripting;

/// <summary>
/// Парсер барсиковых идентификаторов.
/// </summary>
internal sealed class IdentifierParser
    : Parser<Token, string?>
{
    #region Parser<T1,T2> members

    /// <inheritdoc cref="Parser{TToken,T}.TryParse"/>
    public override bool TryParse
        (
            ref ParseState<Token> state,
            ref PooledList<Expected<Token>> expecteds,
            out string? result
        )
    {
        result = default;
        if (!state.HasCurrent)
        {
            return false;
        }

        var current = state.Current;
        if (current.Kind == TokenKind.Identifier)
        {
            result = current.Value;
            state.Advance();
            return true;
        }

        return false;
    }

    #endregion
}
