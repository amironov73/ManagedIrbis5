// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* ReservedWordParser.cs -- парсит зарезервированное слово
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Kotik;

/// <summary>
/// Парсит зарезервированное слово
/// </summary>
public sealed class ReservedWordParser
    : Parser<string>
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ReservedWordParser
        (
            string expected
        )
    {
        Sure.NotNull (expected);

        _expected = expected;
    }

    #endregion

    #region Private members

    private readonly string _expected;

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
        if (state.HasCurrent && state.Current.IsReservedWord(_expected))
        {
            result = _expected;
            state.Advance();
            return true;
        }

        return false;
    }

    #endregion
}
