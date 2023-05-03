// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UseNameofExpression

/* EscapeParser.cs -- умеет разбирать строковый литерал с экранированными символами
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Text;

#endregion

#nullable enable

namespace AM.Purr.Parsers;

/// <summary>
/// Умеет разбирать строковый литерал с экранированными символами.
/// </summary>
internal sealed class EscapeParser
    : Parser<string>
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="expected">Ожидаемый тип токена.</param>
    public EscapeParser
        (
            string expected
        )
    {
        Sure.NotNullNorEmpty (expected);

        _expected = expected;
    }

    #endregion

    #region Private members

    private readonly string _expected;

    #endregion

    #region Public methods

    /// <summary>
    /// Разбор входного потока.
    /// </summary>
    public override bool TryParse
        (
            ParseState state,
            out string result
        )
    {
        using var level = state.Enter (this);
        result = default!;
        DebugHook (state);
        if (!state.HasCurrent)
        {
            return DebugSuccess (state, false);
        }

        var current = state.Current;
        if (current.Kind != _expected)
        {
            return DebugSuccess (state, false);
        }

        state.Advance();
        result = TextUtility.UnescapeText (current.Value.ThrowIfNull());

        return DebugSuccess (state, true);
    }

    #endregion
}
