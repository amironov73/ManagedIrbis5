// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UseNameofExpression

/* Escapist.cs -- умеет разбирать строковый литерал с экранированными символами
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics.CodeAnalysis;

using AM.Text;

using Pidgin;

#endregion

#nullable enable

namespace AM.Scripting;

/// <summary>
/// Умеет разбирать строковый литерал с экранированными символами.
/// </summary>
sealed class EscapeParser
    : Parser<char, string>
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="limiter">Символ-ограничитель.</param>
    /// <param name="escapeSymbol">Экранирующий символ.</param>
    public EscapeParser
        (
            char limiter,
            char escapeSymbol
        )
    {
        _limiter = limiter;
        _escapeSymbol = escapeSymbol;
    }

    #endregion

    #region Private members

    private readonly char _limiter;
    private readonly char _escapeSymbol;
    #endregion

    #region Public methods

    /// <summary>
    /// Разбор входного потока.
    /// </summary>
    public override bool TryParse
        (
            ref ParseState<char> state,
            ref PooledList<Expected<char>> expecteds,
            [MaybeNullWhen (false)] out string result
        )
    {
        result = null!;
        var builder = StringBuilderPool.Shared.Get();
        if (!state.HasCurrent)
        {
            return false;
        }

        var current = state.Current;
        if (current != _limiter)
        {
            state.SetError (Maybe.Just (_limiter), false, state.Location, null);
            return false;
        }

        var escape = false;
        state.Advance();

        while (state.HasCurrent)
        {
            current = state.Current;
            if (current == _limiter)
            {
                if (!escape)
                {
                    state.Advance();
                    result = builder.ReturnShared();

                    return true;
                }
            }
            else
            {
                if (current == _escapeSymbol)
                {
                    escape = !escape;
                }
                else
                {
                    escape = false;
                }
            }

            builder.Append (current);
            state.Advance();
        }

        builder.DismissShared();
        state.SetError (Maybe.Just (_limiter), true, state.Location, null);

        return false;
    }

    #endregion
}
