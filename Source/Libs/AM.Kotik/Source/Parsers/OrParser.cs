// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassWithVirtualMembersNeverInherited.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable NonReadonlyMemberInGetHashCode
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* OrParser.cs -- парсинг альтернатив
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Text;

#endregion

#nullable enable

namespace AM.Kotik;

/// <summary>
/// Парсинг альтернатив.
/// </summary>
public sealed class OrParser<TResult>
    : Parser<TResult>
    where TResult: class
{
    #region Construciton

    /// <summary>
    /// Конструктор.
    /// </summary>
    public OrParser
        (
            params Parser<TResult>[] alternatives
        )
    {
        Sure.AssertState (alternatives.Length != 0);

        _alternatives = alternatives;
    }

    #endregion

    #region Private members

    private readonly Parser<TResult>[] _alternatives;

    #endregion

    #region Parser<TResult> members

    /// <inheritdoc cref="Parser{TResult}.TryParse"/>
    public override bool TryParse
        (
            ParseState state,
            out TResult? result
        )
    {
        result = default;

        var location = state.Location;
        foreach (var parser in _alternatives)
        {
            state.Location = location;
            if (parser.TryParse (state, out result))
            {
                return true;
            }

        }

        return false;
    }

    #endregion
}
