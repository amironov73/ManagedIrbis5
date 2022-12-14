// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* FastTreeNode.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;

#endregion

#nullable enable

namespace AM.Windows.Forms;

/// <summary>
/// General tree data model
/// </summary>
public class FastTreeNode
{
    /// <summary>
    ///
    /// </summary>
    protected readonly List<FastTreeNode> children = new();

    /// <summary>
    ///
    /// </summary>
    public object? Tag { get; set; }

    private FastTreeNode? _parent;

    /// <summary>
    ///
    /// </summary>
    public FastTreeNode? Parent
    {
        get => _parent;
        set
        {
            if (_parent == value)
            {
                return;
            }

            SetParent (value);

            if (_parent != null)
            {
                _parent.children.Add (this);
            }
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="value"></param>
    protected virtual void SetParent (FastTreeNode? value)
    {
        if (_parent != null && _parent != value)
        {
            _parent.children.Remove (this);
        }

        _parent = value;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="node"></param>
    public virtual void RemoveNode (FastTreeNode node)
    {
        children.Remove (node);
        SetParent (null);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="node"></param>
    public virtual void AddNode (FastTreeNode node)
    {
        if (node.Parent != this)
        {
            children.Add (node);
        }

        SetParent (this);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="index"></param>
    /// <param name="node"></param>
    public virtual void InsertNode (int index, FastTreeNode node)
    {
        children.Insert (index, node);
        SetParent (this);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="existsNode"></param>
    /// <param name="node"></param>
    public virtual void InsertNodeBefore (FastTreeNode existsNode, FastTreeNode node)
    {
        var i = children.IndexOf (existsNode);
        if (i < 0)
        {
            i = 0;
        }

        InsertNode (i, node);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="existsNode"></param>
    /// <param name="node"></param>
    public virtual void InsertNodeAfter (FastTreeNode existsNode, FastTreeNode node)
    {
        var i = children.IndexOf (existsNode) + 1;
        InsertNode (i, node);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="nodes"></param>
    public virtual void RemoveNode (IEnumerable<FastTreeNode> nodes)
    {
        var hash = new HashSet<FastTreeNode> (nodes);
        var j = 0;
        for (int i = 0; i < children.Count; i++)
        {
            if (hash.Contains (children[i]))
            {
                j++;
            }
            else
            {
                children[i].SetParent (null);
            }

            children[i] = children[i + j];
        }

        if (j > 0)
        {
            children.RemoveRange (children.Count - j, j);
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="nodes"></param>
    public virtual void AddNode (IEnumerable<FastTreeNode> nodes)
    {
        children.AddRange (nodes);
        foreach (var node in nodes)
        {
            node.SetParent (this);
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="index"></param>
    /// <param name="nodes"></param>
    public virtual void InsertNode (int index, IEnumerable<FastTreeNode> nodes)
    {
        children.InsertRange (index, nodes);
        foreach (var node in nodes)
        {
            node.SetParent (this);
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="existsNode"></param>
    /// <param name="nodes"></param>
    public virtual void InsertNodeBefore (FastTreeNode existsNode, IEnumerable<FastTreeNode> nodes)
    {
        var i = children.IndexOf (existsNode);
        if (i < 0)
        {
            i = 0;
        }

        InsertNode (i, nodes);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="existsNode"></param>
    /// <param name="nodes"></param>
    public virtual void InsertNodeAfter (FastTreeNode existsNode, IEnumerable<FastTreeNode> nodes)
    {
        var i = children.IndexOf (existsNode) + 1;
        InsertNode (i, nodes);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    public int IndexOf (FastTreeNode node)
    {
        return children.IndexOf (node);
    }

    /// <summary>
    ///
    /// </summary>
    public IEnumerable<FastTreeNode> Children => children;

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TType"></typeparam>
    /// <returns></returns>
    public IEnumerable<FastTreeNode> GetChildren<TType>()
    {
        return GetChildren (t => t is TType);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="tagCondition"></param>
    /// <returns></returns>
    public IEnumerable<FastTreeNode> GetChildren (Predicate<object> tagCondition)
    {
        return children.Where (c => tagCondition (c.Tag!));
    }

    /// <summary>
    ///
    /// </summary>
    public IEnumerable<FastTreeNode> AllChildren
    {
        get
        {
            yield return this;

            foreach (var c in children)
            {
                foreach (var cc in c.AllChildren)
                {
                    yield return cc;
                }
            }
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TType"></typeparam>
    /// <returns></returns>
    public IEnumerable<FastTreeNode> GetAllChildren<TType>()
    {
        return GetAllChildren (t => t is TType);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="tagCondition"></param>
    /// <returns></returns>
    public IEnumerable<FastTreeNode> GetAllChildren (Predicate<object> tagCondition)
    {
        return AllChildren.Where (c => tagCondition (c.Tag!));
    }

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TType"></typeparam>
    /// <returns></returns>
    public FastTreeNode? GetParent<TType>()
    {
        return GetParent (t => t is TType);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="tagCondition"></param>
    /// <returns></returns>
    public FastTreeNode? GetParent (Predicate<object> tagCondition)
    {
        var parent = Parent;
        while (parent != null && !tagCondition (parent))
            parent = parent._parent;
        return parent;
    }
}
