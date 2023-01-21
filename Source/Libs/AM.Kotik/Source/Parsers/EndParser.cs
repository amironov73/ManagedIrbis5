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

namespace AM.Kotik;

/// <summary>
/// Парсер, ожидающий конец текста.
/// </summary>
/// <example>
/// Типичное применение выглядит так
///
/// <code>
/// someparser.Before (new EndParser()).ParseOrThrow (state);
/// </code>
/// </example>
internal sealed class EndParser
    : Parser<Unit>
{
    #region Parser<TResult> members

    /// <inheritdoc cref="Parser{TResult}.TryParse"/>
    public override bool TryParse
        (
            ParseState state,
            [MaybeNullWhen (false)] out Unit result
        )
    {
        using var _ = state.Enter (this);
        result = Unit.Value;
        DebugHook (state);

        return DebugSuccess (state, !state.HasCurrent);
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="Parser{TResult}.ToString"/>
    public override string ToString() => "End";

    #endregion
}
