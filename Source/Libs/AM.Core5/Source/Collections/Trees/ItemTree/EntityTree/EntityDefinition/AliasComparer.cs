// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable NonReadonlyMemberInGetHashCode
// ReSharper disable UnusedMember.Global

/* AliasComparer.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

#endregion

#nullable enable

namespace TreeCollections;

internal sealed class AliasComparer<TItem, TName>
    : IEqualityComparer<TItem>
{
    #region Construction

    public AliasComparer
        (
            Func<TItem, TName> getName,
            IEqualityComparer<TName> nameComparer
        )
    {
        _getName = getName;
        _nameComparer = nameComparer;
    }

    #endregion

    #region Private members

    private readonly Func<TItem, TName> _getName;

    private readonly IEqualityComparer<TName> _nameComparer;

    #endregion

    #region IEqualityComparer members

    /// <inheritdoc cref="IEqualityComparer{T}.Equals(T?,T?)"/>
    public bool Equals
        (
            TItem? x,
            TItem? y
        )
    {
        if (x is null || y is null)
        {
            return false;
        }

        return _nameComparer.Equals (_getName (x), _getName (y));
    }

    /// <inheritdoc cref="IEqualityComparer{T}.GetHashCode(T)"/>
    public int GetHashCode
        (
            TItem obj
        )
    {
        var name = _getName (obj);
        return name is null ? 0 : _nameComparer.GetHashCode (name);
    }

    #endregion
}
