// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* OneOfParser.cs -- парсинг альтернатив
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

#endregion

#nullable enable

namespace AM.Kotik;

/// <summary>
/// Парсинг альтернатив, результат выдает первая из сработавших.
/// </summary>
public sealed class OneOfParser<TResult>
    : Parser<TResult>
    where TResult: class
{
    #region Construciton

    /// <summary>
    /// Конструктор.
    /// </summary>
    public OneOfParser
        (
            params Parser<TResult>[] alternatives
        )
    {
        Sure.NotNull (alternatives);
        Sure.AssertState (alternatives.Length != 0);

        _alternatives = alternatives;
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public OneOfParser
        (
            IList<Parser<TResult>> alternatives
        )
    {
        Sure.NotNull (alternatives);
        Sure.AssertState (alternatives.Count != 0);

        _alternatives = alternatives;
    }

    #endregion

    #region Private members

    private readonly IList<Parser<TResult>> _alternatives;

    #endregion

    #region Parser<TResult> members

    /// <inheritdoc cref="Parser{TResult}.TryParse"/>
    public override bool TryParse
        (
            ParseState state,
            [MaybeNullWhen (false)] out TResult result
        )
    {
        using var _ = state.Enter (this);
        result = default;
        DebugHook (state);

        var location = state.Location;
        foreach (var parser in _alternatives)
        {
            state.Location = location;
            if (parser.TryParse (state, out result!))
            {
                return DebugSuccess (state, true);
            }

        }

        return DebugSuccess (state, false);
    }

    #endregion
}
