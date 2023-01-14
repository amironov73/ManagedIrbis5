// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* TermParser.cs -- парсит термы
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Kotik;

/// <summary>
/// Парсит термы.
/// </summary>
public sealed class TermParser
    : Parser<string>
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public TermParser
        (
            string[]? expected
        )
    {
        _expected = expected;
    }

    #endregion

    #region Private members

    private readonly string[]? _expected;

    #endregion

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
        if (!current.IsTerm())
        {
            return false;
        }

        if (_expected is null)
        {
            result = current.Value!;
            return true;
        }

        if (current.IsTerm (_expected))
        {
            result = current.Value!;
            return true;
        }

        return false;
    }

    #endregion
}
