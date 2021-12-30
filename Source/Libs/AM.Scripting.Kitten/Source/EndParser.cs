// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* EndParser.cs -- парсер, ожидающий конца текста
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics.CodeAnalysis;

#endregion

#nullable enable

namespace AM.Scripting.Kitten;

/// <summary>
/// Парсер, ожидающий конец текста.
/// </summary>
internal sealed class EndParser<TToken>
    : Parser<TToken, Unit>
{
    #region Parser<TToken, TResult> members

    /// <inheritdoc cref="Parser{TToken,TResult}.TryParse"/>
    public override bool TryParse
        (
            ParseState<TToken> state,
            [MaybeNullWhen(false)] out Unit result
        )
    {
        result = Unit.Value;

        if (state.HasCurrent)
        {
            return false;
        }

        return true;
    }

    #endregion
}
