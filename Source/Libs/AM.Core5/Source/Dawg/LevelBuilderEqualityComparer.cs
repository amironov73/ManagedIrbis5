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

/* LevelBuilderEqualityComparer.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

#endregion

#nullable enable

namespace AM.Dawg;

/// <summary>
///
/// </summary>
internal sealed class LevelBuilderEqualityComparer<TPayload>
    : IEqualityComparer<Node<TPayload>>
{
    private readonly IEqualityComparer<TPayload> _payloadComparer;

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public LevelBuilderEqualityComparer
        (
            IEqualityComparer<TPayload> payloadComparer
        )
    {
        Sure.NotNull (payloadComparer);

        _payloadComparer = payloadComparer;
    }

    #endregion

    /// <inheritdoc cref="IEqualityComparer{T}.Equals(T?,T?)"/>
    public bool Equals
        (
            Node<TPayload>? x,
            Node<TPayload>? y
        )
    {
        // ReSharper disable PossibleNullReferenceException
        var equals = AreEqual (x!, y!);

        // ReSharper restore PossibleNullReferenceException

        return equals;
    }

    private bool AreEqual (Node<TPayload> xNode, Node<TPayload> yNode)
    {
        var equals = _payloadComparer.Equals (xNode.Payload, yNode.Payload)
                     && SequenceEqual (xNode.SortedChildren, yNode.SortedChildren);
        return equals;
    }

    private bool SequenceEqual
        (
        IEnumerable<KeyValuePair<char, Node<TPayload>>> x,
        IEnumerable<KeyValuePair<char, Node<TPayload>>> y
        )
    {
        // Do not bother disposing of these enumerators.

        // ReSharper disable GenericEnumeratorNotDisposed
        var xe = x.GetEnumerator();
        var ye = y.GetEnumerator();

        // ReSharper restore GenericEnumeratorNotDisposed

        while (xe.MoveNext())
        {
            if (!ye.MoveNext())
            {
                return false;
            }

            var xcurrent = xe.Current;
            var ycurrent = ye.Current;

            if (xcurrent.Key != ycurrent.Key)
            {
                return false;
            }

            // Child nodes have already been merged
            // so we can use reference equality here.
            if (xcurrent.Value != ycurrent.Value)
            {
                return false;
            }
        }

        return !ye.MoveNext();
    }

    private int ComputeHashCode
        (
            Node<TPayload> node
        )
    {
        var hashCode = _payloadComparer.GetHashCode (node.Payload!);

        foreach (var pair in node.Children)
        {
            var c = pair.Key;
            var childNode = pair.Value;

            // Child nodes have already been merged
            // so we can use reference equality here.
            hashCode ^= c ^ childNode.GetHashCode();
        }

        return hashCode;
    }

    public int GetHashCode (Node<TPayload> node)
    {
        return ComputeHashCode (node);
    }
}
