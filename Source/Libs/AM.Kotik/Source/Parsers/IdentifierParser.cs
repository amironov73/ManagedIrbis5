// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* IdentifierParser.cs -- парсит идентификаторы
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics.CodeAnalysis;

#endregion

#nullable enable

namespace AM.Kotik;

/// <summary>
/// Парсит идентификаторы.
/// </summary>
public sealed class IdentifierParser
    : Parser<string>
{
    #region Parser<TResult> members

    /// <inheritdoc cref="Parser{TResult}.TryParse"/>
    public override bool TryParse
        (
            ParseState state,
            [MaybeNullWhen (false)]  out string result
        )
    {
        using var _ = state.Enter (this);
        result = default!;
        if (!state.HasCurrent)
        {
            return DebugSuccess (state, false);
        }

        var current = state.Current;
        if (current.IsIdentifier())
        {
            result = current.Value!;
            var final = DebugSuccess (state, true);
            state.Advance();
            return final;
        }

        return DebugSuccess (state, false);
    }

    #endregion
}
