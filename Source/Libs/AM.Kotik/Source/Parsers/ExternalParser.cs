// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* ExternalParser.cs -- парсит внешний код
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics.CodeAnalysis;

using AM.Kotik.Tokenizers;

#endregion

#nullable enable

namespace AM.Kotik;

/// <summary>
/// Парсит внешний код (на самом деле, просто складывает его в строку).
/// </summary>
public sealed class ExternalParser
    : Parser<string>
{
    #region Parser<TResult> members

    /// <inheritdoc cref="Parser{TResult}.TryParse"/>
    public override bool TryParse 
        (
            ParseState state, 
            [MaybeNullWhen (false)] out string result
        )
    {
        result = default;
        DebugHook (state);
        if (!state.HasCurrent)
        {
            return DebugSuccess (state, false);
        }
        
        var current = state.Current;
        if (current.Kind == TokenKind.External)
        {
            result = current.Value;
            state.Advance();
            return DebugSuccess (state, true);
        }

        return DebugSuccess (state, false);
    }

    #endregion
}
