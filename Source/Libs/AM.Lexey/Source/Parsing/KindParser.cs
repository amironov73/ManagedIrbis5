// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* KindParser.cs -- парсер, принимающий только токен определенного вида
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives


using AM.Lexey.Tokenizing;

using JetBrains.Annotations;

#endregion

namespace AM.Lexey.Parsing;

/// <summary>
/// Парсер, принимающий только токен определенного вида.
/// </summary>
[PublicAPI]
public sealed class KindParser
    : Parser<Token>
{
    #region Construciton

    /// <summary>
    /// Конструктор.
    /// </summary>
    public KindParser
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

    #region Parser<TResult> members

    /// <inheritdoc cref="Parser{TResult}.TryParse"/>
    public override bool TryParse
        (
            ParseState state,
            out Token result
        )
    {
        result = default!;
        DebugHook (state);
        if (!state.HasCurrent)
        {
            return DebugSuccess (state, false);
        }

        var current = state.Current;
        if (current.Kind == _expected)
        {
            state.Advance();
            result = current;
            return DebugSuccess (state, true);
        }

        return DebugSuccess (state, false);
    }

    #endregion
}
