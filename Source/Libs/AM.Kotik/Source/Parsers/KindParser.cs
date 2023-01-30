// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* KindParser.cs -- токенайзер, возвращающий токен определенного вида
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics.CodeAnalysis;

#endregion

#nullable enable

namespace AM.Kotik.Parsers;

/// <summary>
/// Токенайзер, возвращающий токен определенного вида.
/// </summary>
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
            [MaybeNullWhen (false)] out Token result
        )
    {
        result = default;
        if (!state.HasCurrent)
        {
            return false;
        }

        var current = state.Current;
        if (current.Kind == _expected)
        {
            state.Advance();
            result = current;
            return true;
        }

        return false;
    }

    #endregion
}
