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

/* DawgBuilder.cs -- строит DAWG
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.IO;

#endregion

#nullable enable

namespace AM.Dawg;

/// <summary>
/// Строит DAWG.
/// </summary>
public class DawgBuilder<TPayload>
{
    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public DawgBuilder()
    {
        _root = new Node<TPayload>();
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    internal DawgBuilder
        (
            Node<TPayload> root
        )
    {
        Sure.NotNull (root);

        _root = root;
    }

    #endregion

    #region Private members

    internal readonly Node<TPayload> _root;

    private readonly List<Node<TPayload>> lastPath = new ();
    private string _lastKey = string.Empty;

    private static void DoInsert
        (
            Node<TPayload> node,
            IEnumerable<char> key,
            TPayload value
        )
    {
        foreach (var c in key)
        {
            node = node.GetOrAddEdge (c);
        }

        node.Payload = value;
    }

    private void InsertLastPath
        (
            string strKey,
            TPayload value
        )
    {
        var i = 0;

        while (i < strKey.Length && i < _lastKey.Length)
        {
            if (strKey[i] != _lastKey[i])
            {
                break;
            }

            ++i;
        }

        lastPath.RemoveRange (i, lastPath.Count - i);

        _lastKey = strKey;

        var node = i == 0 ? _root : lastPath[i - 1];

        while (i < strKey.Length)
        {
            node = node.GetOrAddEdge (strKey[i]);
            lastPath.Add (node);
            ++i;
        }

        node.Payload = value;
    }

    internal static DawgBuilder<TPayload> Merge
        (
            Dictionary<char, Node<TPayload>> children
        )
    {
        var root = new Node<TPayload> (children);
        return new DawgBuilder<TPayload> (root);
    }

    #endregion

    #region Public methods

    /// <summary>
    ///
    /// </summary>
    public DawgContainer<TPayload> BuildDawg()
    {
        return BuildDawg (EqualityComparer<TPayload>.Default);
    }

    /// <summary>
    ///
    /// </summary>
    public DawgContainer<TPayload> BuildDawg
        (
            IEqualityComparer<TPayload> payloadComparer
        )
    {
        Sure.NotNull (payloadComparer);

        new LevelBuilder<TPayload> (payloadComparer).MergeEnds (_root);

        return new DawgContainer<TPayload> (new OldDawg<TPayload> (_root));
    }

    /// <summary>
    ///
    /// </summary>
    public DawgContainer<TPayload> BuildYaleDawg()
    {
        var dawg = BuildDawg();

        var writer = BuiltinTypeIO.GetWriter<TPayload>();

        var memoryStream = new MemoryStream();

#pragma warning disable 612,618
        dawg.SaveAsYaleDawg (memoryStream, writer);
#pragma warning restore 612,618

        memoryStream.Position = 0;

        var rehydrated = DawgContainer<TPayload>.Load (memoryStream);

        return rehydrated;
    }

    /// <summary>
    /// Inserts a new key/value pair or updates the value for an existing key.
    /// </summary>
    public void Insert
        (
            IEnumerable<char> key,
            TPayload value
        )
    {
        Sure.NotNull (key);

        if (key is string strKey)
        {
            InsertLastPath (strKey, value);
        }
        else
        {
            DoInsert (_root, key, value);
        }
    }

    /// <summary>
    ///
    /// </summary>
    public bool TryGetValue
        (
            IEnumerable<char> key,
            out TPayload value
        )
    {
        Sure.NotNull (key);

        value = default!;

        var node = _root;
        foreach (var c in key)
        {
            node = node.GetChild (c);
            if (node == null)
            {
                return false;
            }
        }

        value = node.Payload;

        return node.HasPayload;
    }

    #endregion
}
