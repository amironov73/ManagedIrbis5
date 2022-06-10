// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

/* TermParser.cs -- парсер барсиковых токенов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using Pidgin;

#endregion

#nullable enable

namespace AM.Scripting;

/// <summary>
/// Парсер барсиковых токенов.
/// </summary>
internal sealed class TermParser
    : Parser<Token,string?>
{
    #region Properties

    /// <summary>
    /// Ожидаемые значения термов.
    /// </summary>
    public string[] ExpectedValues { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="expectedValues">Ожидаемые значения термов</param>
    public TermParser
        (
            params string[] expectedValues
        )
    {
        Sure.NotNull (expectedValues);

        ExpectedValues = expectedValues;
    }

    #endregion

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
        if (current.Kind == TokenKind.Term)
        {
            foreach (var value in ExpectedValues)
            {
                if (current.Value == value)
                {
                    result = current.Value;
                    state.Advance();
                    return true;
                }
            }
        }

        return false;
    }

    #endregion
}
