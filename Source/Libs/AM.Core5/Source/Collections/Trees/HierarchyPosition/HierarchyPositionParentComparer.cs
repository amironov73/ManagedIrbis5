// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable NonReadonlyMemberInGetHashCode
// ReSharper disable UnusedMember.Global

/* HierarchyPositionParentComparer.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Linq;

#endregion

#nullable enable

namespace TreeCollections;

internal sealed class HierarchyPositionParentComparer
    : IEqualityComparer<HierarchyPosition>
{
    #region Properties

    public static readonly HierarchyPositionParentComparer Default = new ();

    #endregion

    #region IEqualityComparer members

    /// <summary>
    ///
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public bool Equals
        (
            HierarchyPosition? x,
            HierarchyPosition? y
        )
    {
        if (x is null || y is null)
        {
            return false;
        }

        return x.Take (x.Level).SequenceEqual (y.Take (y.Level));
    }

    /// <inheritdoc cref="IEqualityComparer{T}.GetHashCode(T)"/>
    public int GetHashCode
        (
            HierarchyPosition obj
        )
    {
        return obj.GetHashCode();
    }

    #endregion
}
