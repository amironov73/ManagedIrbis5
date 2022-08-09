// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassWithVirtualMembersNeverInherited.Global
// ReSharper disable CommentTypo
// ReSharper disable EventNeverSubscribedTo.Global
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMemberInSuper.Global
// ReSharper disable UnusedParameter.Local
// ReSharper disable VirtualMemberCallInConstructor

/* SequenceEqualityComparer.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Linq;

#endregion

#nullable enable

namespace AM.Dawg;

/// <summary>
///
/// </summary>
public class SequenceEqualityComparer<T>
    : IEqualityComparer<IList<T>>
{
    private readonly IEqualityComparer<T> elementComparer;

    public SequenceEqualityComparer (IEqualityComparer<T> elementComparer = null)
    {
        this.elementComparer = elementComparer ?? EqualityComparer<T>.Default;
    }

    public bool Equals (IList<T> x, IList<T> y)
    {
        return ReferenceEquals (x, y) || (x != null && y != null && x.SequenceEqual (y, elementComparer));
    }

    public int GetHashCode (IList<T> obj)
    {
        if (obj == null)
        {
            return 0;
        }

        // Will not throw an OverflowException
        unchecked
        {
            return obj.Where (e => e != null).Select (elementComparer.GetHashCode).Aggregate (17, (a, b) => 23 * a + b);
        }
    }
}
