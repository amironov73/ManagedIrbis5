// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable NonReadonlyMemberInGetHashCode
// ReSharper disable UnusedMember.Global

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;

#endregion

#nullable enable

namespace HtmlAgilityPack;

/// <summary>
/// Represents a combined list and collection of HTML nodes.
/// </summary>
public class HtmlNodeCollection : IList<HtmlNode>
{
    #region Fields

    private readonly List<HtmlNode> _items = new List<HtmlNode>();

    #endregion

    #region Constructors

    /// <summary>
    /// Initialize the HtmlNodeCollection with the base parent node
    /// </summary>
    /// <param name="parentnode">The base node of the collection</param>
    public HtmlNodeCollection
        (
            HtmlNode? parentnode
        )
    {
        ParentNode = parentnode; // may be null
    }

    #endregion

    #region Properties

    /// <summary>Gets the parent node associated to the collection.</summary>
    internal HtmlNode? ParentNode { get; }

    /// <summary>
    /// Gets a given node from the list.
    /// </summary>
    public int this [HtmlNode node]
    {
        get
        {
            var index = GetNodeIndex (node);
            if (index == -1)
            {
                throw new ArgumentOutOfRangeException ("node",
                    "Node \"" + node.CloneNode (false).OuterHtml +
                    "\" was not found in the collection");
            }

            return index;
        }
    }

    /// <summary>
    /// Get node with tag name
    /// </summary>
    /// <param name="nodeName"></param>
    /// <returns></returns>
    public HtmlNode this [string nodeName]
    {
        get
        {
            for (var i = 0; i < _items.Count; i++)
            {
                if (string.Equals (_items[i].Name, nodeName, StringComparison.OrdinalIgnoreCase))
                {
                    return _items[i];
                }
            }

            return null;
        }
    }

    #endregion

    #region IList<HtmlNode> Members

    /// <summary>
    /// Gets the number of elements actually contained in the list.
    /// </summary>
    public int Count => _items.Count;

    /// <summary>
    /// Is collection read only
    /// </summary>
    public bool IsReadOnly => false;

    /// <summary>
    /// Gets the node at the specified index.
    /// </summary>
    public HtmlNode this [int index]
    {
        get => _items[index];
        set => _items[index] = value;
    }

    /// <summary>
    /// Add node to the collection
    /// </summary>
    /// <param name="node"></param>
    public void Add (HtmlNode node)
    {
        Add (node, true);
    }

    /// <summary>
    /// Add node to the collection
    /// </summary>
    /// <param name="node"></param>
    /// <param name="setParent"></param>
    public void Add (HtmlNode node, bool setParent)
    {
        _items.Add (node);

        if (setParent)
        {
            node.ParentNode = ParentNode;
        }
    }

    /// <summary>
    /// Clears out the collection of HtmlNodes. Removes each nodes reference to parentnode, nextnode and prevnode
    /// </summary>
    public void Clear()
    {
        foreach (var node in _items)
        {
            node.ParentNode = null;
            node.NextSibling = null;
            node.PreviousSibling = null;
        }

        _items.Clear();
    }

    /// <summary>
    /// Gets existence of node in collection
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool Contains (HtmlNode item)
    {
        return _items.Contains (item);
    }

    /// <summary>
    /// Copy collection to array
    /// </summary>
    /// <param name="array"></param>
    /// <param name="arrayIndex"></param>
    public void CopyTo (HtmlNode[] array, int arrayIndex)
    {
        _items.CopyTo (array, arrayIndex);
    }

    /// <summary>
    /// Get Enumerator
    /// </summary>
    /// <returns></returns>
    IEnumerator<HtmlNode> IEnumerable<HtmlNode>.GetEnumerator()
    {
        return _items.GetEnumerator();
    }

    /// <summary>
    /// Get Explicit Enumerator
    /// </summary>
    /// <returns></returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return _items.GetEnumerator();
    }

    /// <summary>
    /// Get index of node
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public int IndexOf (HtmlNode item)
    {
        return _items.IndexOf (item);
    }

    /// <summary>
    /// Insert node at index
    /// </summary>
    /// <param name="index"></param>
    /// <param name="node"></param>
    public void Insert (int index, HtmlNode node)
    {
        HtmlNode? next = null;
        HtmlNode? prev = null;

        if (index > 0)
        {
            prev = _items[index - 1];
        }

        if (index < _items.Count)
        {
            next = _items[index];
        }

        _items.Insert (index, node);

        if (prev != null)
        {
            if (node == prev)
            {
                throw new InvalidProgramException ("Unexpected error.");
            }

            prev._nextNode = node;
        }

        if (next != null)
        {
            next._prevNode = node;
        }

        node._prevNode = prev;
        if (next == node)
        {
            throw new InvalidProgramException ("Unexpected error.");
        }

        node._nextNode = next;
        node.SetParent (ParentNode);
    }

    /// <summary>
    /// Remove node
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool Remove (HtmlNode item)
    {
        var i = _items.IndexOf (item);
        RemoveAt (i);
        return true;
    }

    /// <summary>
    /// Remove <see cref="HtmlNode"/> at index
    /// </summary>
    /// <param name="index"></param>
    public void RemoveAt (int index)
    {
        HtmlNode next = null;
        HtmlNode prev = null;
        var oldnode = _items[index];

        // KEEP a reference since it will be set to null
        var parentNode = ParentNode ?? oldnode._parentNode;

        if (index > 0)
        {
            prev = _items[index - 1];
        }

        if (index < (_items.Count - 1))
        {
            next = _items[index + 1];
        }

        _items.RemoveAt (index);

        if (prev != null)
        {
            if (next == prev)
            {
                throw new InvalidProgramException ("Unexpected error.");
            }

            prev._nextNode = next;
        }

        if (next != null)
        {
            next._prevNode = prev;
        }

        oldnode._prevNode = null;
        oldnode._nextNode = null;
        oldnode._parentNode = null;

        if (parentNode != null)
        {
            parentNode.SetChanged();
        }
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Get first instance of node in supplied collection
    /// </summary>
    /// <param name="items"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static HtmlNode? FindFirst (HtmlNodeCollection items, string name)
    {
        foreach (var node in items)
        {
            if (node.Name.Equals (name, StringComparison.OrdinalIgnoreCase))
            {
                return node;
            }

            if (!node.HasChildNodes)
            {
                continue;
            }

            var returnNode = FindFirst (node.ChildNodes, name);
            if (returnNode != null)
            {
                return returnNode;
            }
        }

        return null;
    }

    /// <summary>
    /// Add node to the end of the collection
    /// </summary>
    /// <param name="node"></param>
    public void Append (HtmlNode node)
    {
        HtmlNode? last = null;
        if (_items.Count > 0)
        {
            last = _items[^1];
        }

        _items.Add (node);
        node._prevNode = last;
        node._nextNode = null;
        node.SetParent (ParentNode);
        if (last == null)
        {
            return;
        }

        if (last == node)
        {
            throw new InvalidProgramException ("Unexpected error.");
        }

        last._nextNode = node;
    }

    /// <summary>
    /// Get first instance of node with name
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public HtmlNode FindFirst (string name)
    {
        return FindFirst (this, name);
    }

    /// <summary>
    /// Get index of node
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    public int GetNodeIndex (HtmlNode node)
    {
        // TODO: should we rewrite this? what would be the key of a node?
        for (var i = 0; i < _items.Count; i++)
        {
            if (node == _items[i])
            {
                return i;
            }
        }

        return -1;
    }

    /// <summary>
    /// Add node to the beginning of the collection
    /// </summary>
    /// <param name="node"></param>
    public void Prepend (HtmlNode node)
    {
        HtmlNode first = null;
        if (_items.Count > 0)
        {
            first = _items[0];
        }

        _items.Insert (0, node);

        if (node == first)
        {
            throw new InvalidProgramException ("Unexpected error.");
        }

        node._nextNode = first;
        node._prevNode = null;
        node.SetParent (ParentNode);

        if (first != null)
        {
            first._prevNode = node;
        }
    }

    /// <summary>
    /// Remove node at index
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public bool Remove (int index)
    {
        RemoveAt (index);
        return true;
    }

    /// <summary>
    /// Replace node at index
    /// </summary>
    /// <param name="index"></param>
    /// <param name="node"></param>
    public void Replace (int index, HtmlNode node)
    {
        HtmlNode next = null;
        HtmlNode prev = null;
        var oldnode = _items[index];

        if (index > 0)
        {
            prev = _items[index - 1];
        }

        if (index < (_items.Count - 1))
        {
            next = _items[index + 1];
        }

        _items[index] = node;

        if (prev != null)
        {
            if (node == prev)
            {
                throw new InvalidProgramException ("Unexpected error.");
            }

            prev._nextNode = node;
        }

        if (next != null)
        {
            next._prevNode = node;
        }

        node._prevNode = prev;

        if (next == node)
        {
            throw new InvalidProgramException ("Unexpected error.");
        }

        node._nextNode = next;
        node.SetParent (ParentNode);

        oldnode._prevNode = null;
        oldnode._nextNode = null;
        oldnode._parentNode = null;
    }

    #endregion

    #region LINQ Methods

    /// <summary>
    /// Get all node descended from this collection
    /// </summary>
    /// <returns></returns>
    public IEnumerable<HtmlNode> Descendants()
    {
        foreach (var item in _items)
        {
            foreach (var n in item.Descendants())
            {
                yield return n;
            }
        }
    }

    /// <summary>
    /// Get all node descended from this collection with matching name
    /// </summary>
    /// <returns></returns>
    public IEnumerable<HtmlNode> Descendants (string name)
    {
        foreach (var item in _items)
        {
            foreach (var n in item.Descendants (name))
            {
                yield return n;
            }
        }
    }

    /// <summary>
    /// Gets all first generation elements in collection
    /// </summary>
    /// <returns></returns>
    public IEnumerable<HtmlNode> Elements()
    {
        foreach (var item in _items)
        {
            foreach (var n in item.ChildNodes)
            {
                yield return n;
            }
        }
    }

    /// <summary>
    /// Gets all first generation elements matching name
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public IEnumerable<HtmlNode> Elements (string name)
    {
        foreach (var item in _items)
        {
            foreach (var n in item.Elements (name))
            {
                yield return n;
            }
        }
    }

    /// <summary>
    /// All first generation nodes in collection
    /// </summary>
    /// <returns></returns>
    public IEnumerable<HtmlNode> Nodes()
    {
        foreach (var item in _items)
        {
            foreach (var n in item.ChildNodes)
            {
                yield return n;
            }
        }
    }

    #endregion
}
