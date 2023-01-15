// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* PostfixOperator.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics.CodeAnalysis;

#endregion

#nullable enable

namespace AM.Kotik;

/// <summary>
///
/// </summary>
public sealed class PostfixOperator<TResult>
    : Parser<Func<TResult, string, TResult>>
    where TResult: class
{
    #region Parser<TResult> members

    /// <inheritdoc cref="Parser{TResult}.TryParse"/>
    public override bool TryParse
        (
            ParseState state,
            [MaybeNullWhen (false)] out Func<TResult, string, TResult> result
        )
    {
        result = default;
        if (!state.HasCurrent)
        {
            return false;
        }

        // state продвигается вложенными парсерами
        return false;
    }

    #endregion
}
