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

/* OldDawg.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

#nullable enable

namespace AM.Dawg;

/// <summary>
///
/// </summary>
/// <typeparam name="TPayload"></typeparam>
internal sealed class OldDawg<TPayload>
    : IDawg<TPayload>
{
    #region Private members

    internal readonly Node<TPayload> root;

    #endregion

    #region Public methods

    public TPayload this [IEnumerable<char> word]
    {
        get
        {
            var node = FindNode (word);

            return node == null ? default : node.Payload;
        }
    }

    private Node<TPayload>? FindNode
        (
            IEnumerable<char> word
        )
    {
        var node = root;
        foreach (var c in word)
        {
            node = node.GetChild (c);

            if (node == null)
            {
                return null;
            }
        }

        return node;
    }

    public int GetLongestCommonPrefixLength (IEnumerable<char> word)
    {
        var node = root;
        var len = 0;

        foreach (var c in word)
        {
            node = node.GetChild (c);

            if (node == null)
            {
                break;
            }

            ++len;
        }

        return len;
    }

    /// <summary>
    /// Returns all elements with key matching given <paramref name="prefix"/>.
    /// </summary>
    public IEnumerable<KeyValuePair<string, TPayload>> MatchPrefix (IEnumerable<char> prefix)
    {
        var prefixStr = prefix.AsString();

        var node = FindNode (prefixStr);

        if (node == null)
        {
            return Enumerable.Empty<KeyValuePair<string, TPayload>>();
        }

        var sb = new StringBuilder();

        sb.Append (prefixStr);

        return new PrefixMatcher<TPayload> (sb).MatchPrefix (node);
    }

    IEnumerable<KeyValuePair<string, TPayload>> IDawg<TPayload>.GetPrefixes (IEnumerable<char> key)
    {
        throw new NotImplementedException();
    }

    internal OldDawg (Node<TPayload> root)
    {
        this.root = root;
    }

    public int GetNodeCount()
    {
        return root.GetRecursiveChildNodeCount();
    }

    public KeyValuePair<string, TPayload> GetRandomItem (Random random)
    {
        throw new NotImplementedException();
    }

    #endregion
}

/// <summary>
///
/// </summary>
/// <typeparam name="TPayload"></typeparam>
internal sealed class NodeByPayloadComparer<TPayload>
    : IComparer<Node<TPayload>>
{
    #region IComparer<T> members

    /// <inheritdoc cref="IComparer{T}.Compare"/>
    public int Compare
        (
            Node<TPayload>? x,
            Node<TPayload>? y
        )
    {
        return -x!.HasPayload.CompareTo (y!.HasPayload);
    }

    #endregion
}
