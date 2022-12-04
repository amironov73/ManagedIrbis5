// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable NonReadonlyMemberInGetHashCode
// ReSharper disable UnusedMember.Global

/* OperatorOverloads.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace TreeCollections;

public partial class HierarchyPosition
{
    /// <summary>
    ///
    /// </summary>
    public static bool operator ==
        (
            HierarchyPosition? hp1,
            HierarchyPosition? hp2
        )
    {
        if (ReferenceEquals (hp1, hp2))
        {
            return true;
        }

        if (hp1 is null || hp2 is null)
        {
            return false;
        }

        return hp1.Equals (hp2);
    }

    /// <summary>
    ///
    /// </summary>
    public static bool operator !=
        (
            HierarchyPosition? hp1,
            HierarchyPosition? hp2
        )
    {
        return !(hp1 == hp2);
    }

    /// <summary>
    ///
    /// </summary>
    public static bool operator <
        (
            HierarchyPosition hp1,
            HierarchyPosition hp2
        )
    {
        if (!CanCompare (hp1, hp2))
        {
            return false;
        }

        return hp1.CompareTo (hp2) < 0;
    }

    /// <summary>
    ///
    /// </summary>
    public static bool operator >
        (
            HierarchyPosition hp1,
            HierarchyPosition hp2
        )
    {
        if (!CanCompare (hp1, hp2))
        {
            return false;
        }

        return hp1.CompareTo (hp2) > 0;
    }

    /// <summary>
    ///
    /// </summary>
    private static bool CanCompare
        (
            HierarchyPosition? hp1,
            HierarchyPosition? hp2
        )
    {
        if (ReferenceEquals (hp1, hp2))
        {
            return false;
        }

        return hp1 is not null && hp2 is not null;
    }
}
