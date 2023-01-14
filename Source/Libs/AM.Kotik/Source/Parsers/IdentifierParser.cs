// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* IdentifierParser.cs -- парсит идентификаторы
 * Ars Magna project, http://arsmagna.ru
 */

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
            out string result
        )
    {
        result = default!;
        if (!state.HasCurrent)
        {
            return false;
        }

        var current = state.Current;
        if (current.IsIdentifier())
        {
            result = current.Value!;
            return true;
        }

        return false;
    }

    #endregion
}
