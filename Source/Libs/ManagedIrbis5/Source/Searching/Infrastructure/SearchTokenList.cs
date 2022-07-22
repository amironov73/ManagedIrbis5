// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* SearchTokenList.cs -- список токенов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;

using AM;

using Microsoft.Extensions.Logging;

#endregion

namespace ManagedIrbis.Infrastructure;

/// <summary>
/// Список токенов.
/// </summary>
public sealed class SearchTokenList
{
    #region Properties

    /// <summary>
    /// Текущий токен.
    /// </summary>
    internal SearchToken Current => IsEof
        ? throw new InvalidOperationException()
        : _tokens[_position];

    /// <summary>
    /// Достигнут ли конец списка?
    /// </summary>
    internal bool IsEof => _position >= _tokens.Length;

    /// <summary>
    /// Общее количество токенов в списке.
    /// </summary>
    public int Length => _tokens.Length;

    #endregion

    #region Constructor

    /// <summary>
    /// Constructor.
    /// </summary>
    internal SearchTokenList
        (
            IEnumerable<SearchToken> tokens
        )
    {
        _tokens = tokens.ToArray();
        _position = 0;
    }

    #endregion

    #region Private members

    private int _position;

    private readonly SearchToken[] _tokens;

    #endregion

    #region Public methods

    /// <summary>
    /// Move to next token.
    /// </summary>
    internal bool MoveNext()
    {
        _position++;

        return _position < _tokens.Length;
    }

    /// <summary>
    /// Require next token.
    /// </summary>
    internal SearchTokenList RequireNext()
    {
        if (!MoveNext())
        {
            Magna.Logger.LogError
                (
                    nameof (SearchTokenList) + "::" + nameof (RequireNext)
                    + ": no next token"
                );

            throw new SearchSyntaxException ("No next token");
        }

        return this;
    }

    /// <summary>
    /// Require next token.
    /// </summary>
    internal SearchTokenList RequireNext
        (
            SearchTokenKind tokenKind
        )
    {
        if (!MoveNext())
        {
            Magna.Logger.LogError
                (
                    nameof(SearchTokenList) + "::" + nameof(RequireNext)
                    + ": unexpected end of stream"
                );

            throw new SearchSyntaxException ("Unexpected end of stream");
        }

        if (Current.Kind != tokenKind)
        {
            Magna.Logger.LogError
                (
                    nameof(SearchTokenList) + "::" + nameof(RequireNext)
                    + ": expected={Expected}, got={Actual}",
                    tokenKind,
                    Current.Kind
                );

            throw new SearchSyntaxException();
        }

        return this;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString" />
    public override string ToString()
        => IsEof ? "(EOF)" : Current.ToString();

    #endregion
}
