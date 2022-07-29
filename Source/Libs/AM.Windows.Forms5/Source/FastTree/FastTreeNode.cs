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
    protected readonly List<FastTreeNode> children = new();

    public object Tag { get; set; }

    private FastTreeNode parent;

    public FastTreeNode Parent
    {
        get { return parent; }
        set
        {
            if (parent == value)
            {
                return;
            }

            SetParent (value);

            if (parent != null)
            {
                parent.children.Add (this);
            }
        }
    }

    protected virtual void SetParent (FastTreeNode value)
    {
        if (parent != null && parent != value)
        {
            parent.children.Remove (this);
        }

        parent = value;
    }

    public virtual void RemoveNode (FastTreeNode node)
    {
        children.Remove (node);
        SetParent (null);
    }

    public virtual void AddNode (FastTreeNode node)
    {
        if (node.Parent != this)
        {
            children.Add (node);
        }

        SetParent (this);
    }

    public virtual void InsertNode (int index, FastTreeNode node)
    {
        children.Insert (index, node);
        SetParent (this);
    }

    public virtual void InsertNodeBefore (FastTreeNode existsNode, FastTreeNode node)
    {
        var i = children.IndexOf (existsNode);
        if (i < 0)
        {
            i = 0;
        }

        InsertNode (i, node);
    }

    public virtual void InsertNodeAfter (FastTreeNode existsNode, FastTreeNode node)
    {
        var i = children.IndexOf (existsNode) + 1;
        InsertNode (i, node);
    }

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

    public virtual void AddNode (IEnumerable<FastTreeNode> nodes)
    {
        children.AddRange (nodes);
        foreach (var node in nodes)
            node.SetParent (this);
    }

    public virtual void InsertNode (int index, IEnumerable<FastTreeNode> nodes)
    {
        children.InsertRange (index, nodes);
        foreach (var node in nodes)
            node.SetParent (this);
    }

    public virtual void InsertNodeBefore (FastTreeNode existsNode, IEnumerable<FastTreeNode> nodes)
    {
        var i = children.IndexOf (existsNode);
        if (i < 0)
        {
            i = 0;
        }

        InsertNode (i, nodes);
    }

    public virtual void InsertNodeAfter (FastTreeNode existsNode, IEnumerable<FastTreeNode> nodes)
    {
        var i = children.IndexOf (existsNode) + 1;
        InsertNode (i, nodes);
    }

    public int IndexOf (FastTreeNode node)
    {
        return children.IndexOf (node);
    }

    public IEnumerable<FastTreeNode> Children
    {
        get { return children; }
    }

    public IEnumerable<FastTreeNode> GetChilds<TagType>()
    {
        return GetChilds (t => t is TagType);
    }

    public IEnumerable<FastTreeNode> GetChilds (Predicate<object> tagCondition)
    {
        return children.Where (c => tagCondition (c.Tag));
    }

    public IEnumerable<FastTreeNode> AllChilds
    {
        get
        {
            yield return this;

            foreach (var c in children)
            foreach (var cc in c.AllChilds)
                yield return cc;
        }
    }

    public IEnumerable<FastTreeNode> GetAllChilds<TagType>()
    {
        return GetAllChilds (t => t is TagType);
    }

    public IEnumerable<FastTreeNode> GetAllChilds (Predicate<object> tagCondition)
    {
        return AllChilds.Where (c => tagCondition (c.Tag));
    }

    public FastTreeNode GetParent<TagType>()
    {
        return GetParent (t => t is TagType);
    }

    public FastTreeNode GetParent (Predicate<object> tagCondition)
    {
        var parent = Parent;
        while (parent != null && !tagCondition (parent))
            parent = parent.parent;
        return parent;
    }
}
