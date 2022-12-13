// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable NonReadonlyMemberInGetHashCode
// ReSharper disable UnusedMember.Global

/* HierarchyPosition.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using AM;

#endregion

#nullable enable

namespace TreeCollections;

/// <summary>
/// Describes the unique position of a tree node as sequence of ordinal integers based on pre-order traversal.
/// Can be compared (equal, greater than, less than) with other HierarchyPosition objects
/// </summary>
public partial class HierarchyPosition
    : IComparable<HierarchyPosition>,
    IEquatable<HierarchyPosition>,
    IEnumerable<int>
{
    /// <summary>
    /// Конструктор.
    /// </summary>
    public HierarchyPosition
        (
            IReadOnlyList<int> components
        )
    {
        Sure.NotNull (components);

        Components = components;
    }

    /// <summary>
    ///
    /// </summary>
    public HierarchyPosition
        (
            params int[] components
        )
    {
        Sure.NotNull (components);

        Components = components;
    }

    /// <summary>
    /// Compares this position with another position
    /// 0 signifies equivalent positions;
    /// greater than 0 signifies other position is deeper;
    /// less than 0 signifies other position is shallower
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public int CompareTo
        (
            HierarchyPosition? other
        )
    {
        if (other is null)
        {
            return 1;
        }

        int? determinant = null;

        var i = 0;
        while (!determinant.HasValue)
        {
            determinant = GetDeterminant (other, ref i);
        }

        return determinant.Value;
    }

    /// <summary>
    ///
    /// </summary>
    public IReadOnlyList<int> Components { get; }

    /// <summary>
    ///
    /// </summary>
    public int Level => Components.Count - 1;

    /// <summary>
    ///
    /// </summary>
    public int ChildOrderIndex => Components.LastOrDefault();

    private int Length => Components.Count;

    private int? GetDeterminant (HierarchyPosition other, ref int i)
    {
        if (i == Length)
        {
            return other.Length > i
                ? -1
                : 0;
        }

        if (i == other.Length)
        {
            return 1;
        }

        var comparison = Components[i].CompareTo (other.Components[i]);
        if (comparison != 0)
        {
            return comparison;
        }

        i++;
        return null;
    }

    /// <inheritdoc cref="ToString"/>
    public string ToString (string separator) => Components.SerializeToString (separator);

    /// <summary>
    ///
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals (HierarchyPosition? other)
    {
        return other is not null && Components.SequenceEqual (other.Components);
    }

    /// <inheritdoc cref="object.Equals(object?)"/>
    public override bool Equals
        (
            object? obj
        )
    {
        var position = obj as HierarchyPosition;

        return position is null || Equals (position);
    }

    /// <inheritdoc cref="object.GetHashCode"/>
    public override int GetHashCode() => Components.GetHashCode();

    /// <inheritdoc cref="IEnumerable{T}.GetEnumerator"/>
    public IEnumerator<int> GetEnumerator()
    {
        return Components.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
