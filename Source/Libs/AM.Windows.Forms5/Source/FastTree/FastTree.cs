// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global
// ReSharper disable VirtualMemberCallInConstructor

/* FastTree.cs --
 * Ars Magna project, http://arsmagna.ru
 */

//
//  THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY
//  KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
//  IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR
//  PURPOSE.
//
//  License: GNU Lesser General Public License (LGPLv3)
//
//  Email: pavel_torgashov@ukr.net.
//
//  Copyright (C) Pavel Torgashov, 2014.

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms;

/// <summary>
///
/// </summary>
[ToolboxItem (true)]
[DefaultEvent ("NodeChildrenNeeded")]
public class FastTree
    : FastListBase
{
    #region Events

    /// <summary>
    ///
    /// </summary>
    public event EventHandler<Int32NodeEventArgs>? NodeHeightNeeded;

    /// <summary>
    ///
    /// </summary>
    public event EventHandler<StringNodeEventArgs>? NodeTextNeeded;

    /// <summary>
    ///
    /// </summary>
    public event EventHandler<BoolNodeEventArgs>? NodeCheckStateNeeded;

    /// <summary>
    ///
    /// </summary>
    public event EventHandler<ImageNodeEventArgs>? NodeIconNeeded;

    /// <summary>
    ///
    /// </summary>
    public event EventHandler<StringAlignmentNodeEventArgs>? NodeLineAlignmentNeeded;

    /// <summary>
    ///
    /// </summary>
    public event EventHandler<BoolNodeEventArgs>? NodeCheckBoxVisibleNeeded;

    /// <summary>
    ///
    /// </summary>
    public event EventHandler<ColorNodeEventArgs>? NodeBackColorNeeded;

    /// <summary>
    ///
    /// </summary>
    public event EventHandler<Int32NodeEventArgs>? NodeIndentNeeded;

    /// <summary>
    ///
    /// </summary>
    public event EventHandler<ColorNodeEventArgs>? NodeForeColorNeeded;

    /// <summary>
    ///
    /// </summary>
    public event EventHandler<BoolNodeEventArgs>? NodeVisibilityNeeded;

    /// <summary>
    ///
    /// </summary>
    public event EventHandler<BoolNodeEventArgs>? CanUnselectNodeNeeded;

    /// <summary>
    ///
    /// </summary>
    public event EventHandler<BoolNodeEventArgs>? CanSelectNodeNeeded;

    /// <summary>
    ///
    /// </summary>
    public event EventHandler<BoolNodeEventArgs>? CanUncheckNodeNeeded;

    /// <summary>
    ///
    /// </summary>
    public event EventHandler<BoolNodeEventArgs>? CanCheckNodeNeeded;

    /// <summary>
    ///
    /// </summary>
    public event EventHandler<BoolNodeEventArgs>? CanExpandNodeNeeded;

    /// <summary>
    ///
    /// </summary>
    public event EventHandler<BoolNodeEventArgs>? CanCollapseNodeNeeded;

    /// <summary>
    ///
    /// </summary>
    public event EventHandler<BoolNodeEventArgs>? CanEditNodeNeeded;

    /// <summary>
    ///
    /// </summary>
    public event EventHandler<NodeCheckedStateChangedEventArgs>? NodeCheckedStateChanged;

    /// <summary>
    ///
    /// </summary>
    public event EventHandler<NodeExpandedStateChangedEventArgs>? NodeExpandedStateChanged;

    /// <summary>
    ///
    /// </summary>
    public event EventHandler<NodeSelectedStateChangedEventArgs>? NodeSelectedStateChanged;

    /// <summary>
    ///
    /// </summary>
    public event EventHandler<NodeTextPushedEventArgs>? NodeTextPushed;

    /// <summary>
    ///
    /// </summary>
    public event EventHandler<PaintNodeContentEventArgs>? PaintNode;

    /// <summary>
    ///
    /// </summary>
    public event EventHandler<NodeChildrenNeededEventArgs>? NodeChildrenNeeded;

    /// <summary>
    /// Occurs when user start to drag node
    /// </summary>
    public event EventHandler<NodeDragEventArgs>? NodeDrag;

    /// <summary>
    /// Occurs when user drag object over node
    /// </summary>
    public event EventHandler<DragOverItemEventArgs>? DragOverNode;

    /// <summary>
    /// Occurs when user drop object on given node
    /// </summary>
    public event EventHandler<DragOverItemEventArgs>? DropOverNode;

    /// <summary>
    ///
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    protected virtual bool GetNodeVisibility (object node)
    {
        if (NodeVisibilityNeeded != null)
        {
            _boolArg.Node = node;
            _boolArg.Result = true;
            NodeVisibilityNeeded (this, _boolArg);
            return _boolArg.Result;
        }

        return true;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="itemIndex"></param>
    /// <returns></returns>
    protected override string GetItemText (int itemIndex)
    {
        return GetStringNodeProperty (itemIndex, NodeTextNeeded, nodes[itemIndex].ToString()!);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="itemIndex"></param>
    /// <returns></returns>
    protected override bool GetItemChecked (int itemIndex)
    {
        return GetBoolNodeProperty (itemIndex, NodeCheckStateNeeded, CheckedItemIndex.Contains (itemIndex));
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="itemIndex"></param>
    /// <returns></returns>
    protected override Image GetItemIcon (int itemIndex)
    {
        return GetImageNodeProperty (itemIndex, NodeIconNeeded, ImageDefaultIcon);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="itemIndex"></param>
    /// <returns></returns>
    protected override StringAlignment GetItemLineAlignment (int itemIndex)
    {
        return GetLineAlignmentNodeProperty (itemIndex, NodeLineAlignmentNeeded, ItemLineAlignmentDefault);
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="itemIndex"></param>
    /// <returns></returns>
    protected override int GetItemHeight (int itemIndex)
    {
        return GetIntNodeProperty (itemIndex, NodeHeightNeeded, ItemHeightDefault);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="itemIndex"></param>
    /// <returns></returns>
    protected override bool GetItemCheckBoxVisible (int itemIndex)
    {
        return GetBoolNodeProperty (itemIndex, NodeCheckBoxVisibleNeeded, ShowCheckBoxes);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="itemIndex"></param>
    /// <returns></returns>
    protected override Color GetItemBackColor (int itemIndex)
    {
        return GetColorNodeProperty (itemIndex, NodeBackColorNeeded, Color.Empty);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="itemIndex"></param>
    /// <returns></returns>
    protected override Color GetItemForeColor (int itemIndex)
    {
        return GetColorNodeProperty (itemIndex, NodeForeColorNeeded, ForeColor);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="itemIndex"></param>
    /// <returns></returns>
    protected override bool CanUnselectItem (int itemIndex)
    {
        return GetBoolNodeProperty (itemIndex, CanUnselectNodeNeeded, true);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="itemIndex"></param>
    /// <returns></returns>
    protected override bool CanSelectItem (int itemIndex)
    {
        return GetBoolNodeProperty (itemIndex, CanSelectNodeNeeded, AllowSelectItems);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="itemIndex"></param>
    /// <returns></returns>
    protected override bool CanUncheckItem (int itemIndex)
    {
        return GetBoolNodeProperty (itemIndex, CanUncheckNodeNeeded, true);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="itemIndex"></param>
    /// <returns></returns>
    protected override bool CanCheckItem (int itemIndex)
    {
        return GetBoolNodeProperty (itemIndex, CanCheckNodeNeeded, true);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="itemIndex"></param>
    /// <returns></returns>
    protected override bool CanCollapseItem (int itemIndex)
    {
        return GetBoolNodeProperty (itemIndex, CanCollapseNodeNeeded, true);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="itemIndex"></param>
    /// <returns></returns>
    protected override bool CanEditItem (int itemIndex)
    {
        return GetBoolNodeProperty (itemIndex, CanEditNodeNeeded, true);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="itemIndex"></param>
    protected override void OnItemChecked (int itemIndex)
    {
        if (NodeCheckedStateChanged != null)
        {
            NodeCheckedStateChanged (this,
                new NodeCheckedStateChangedEventArgs { Node = nodes[itemIndex], Checked = true });
        }

        base.OnItemChecked (itemIndex);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="itemIndex"></param>
    protected override void OnItemUnchecked (int itemIndex)
    {
        if (NodeCheckedStateChanged != null)
        {
            NodeCheckedStateChanged (this,
                new NodeCheckedStateChangedEventArgs { Node = nodes[itemIndex], Checked = false });
        }

        base.OnItemUnchecked (itemIndex);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="itemIndex"></param>
    /// <param name="text"></param>
    protected override void OnItemTextPushed (int itemIndex, string text)
    {
        if (NodeTextPushed != null)
        {
            NodeTextPushed (this, new NodeTextPushedEventArgs { Node = nodes[itemIndex], Text = text });
        }

        base.OnItemTextPushed (itemIndex, text);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="itemIndex"></param>
    protected override void OnItemExpanded (int itemIndex)
    {
        OnNodeExpanded (nodes[itemIndex]);
        base.OnItemExpanded (itemIndex);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="node"></param>
    protected virtual void OnNodeExpanded (object node)
    {
        if (NodeExpandedStateChanged != null)
        {
            NodeExpandedStateChanged (this, new NodeExpandedStateChangedEventArgs { Node = node, Expanded = true });
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="itemIndex"></param>
    protected override void OnItemCollapsed (int itemIndex)
    {
        if (NodeExpandedStateChanged != null)
        {
            NodeExpandedStateChanged (this,
                new NodeExpandedStateChangedEventArgs { Node = nodes[itemIndex], Expanded = false });
        }

        base.OnItemCollapsed (itemIndex);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="itemIndex"></param>
    protected override void OnItemSelected (int itemIndex)
    {
        if (NodeSelectedStateChanged != null)
        {
            NodeSelectedStateChanged (this,
                new NodeSelectedStateChangedEventArgs { Node = nodes[itemIndex], Selected = true });
        }

        base.OnItemSelected (itemIndex);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="itemIndex"></param>
    protected override void OnItemUnselected (int itemIndex)
    {
        if (NodeSelectedStateChanged != null)
        {
            NodeSelectedStateChanged (this,
                new NodeSelectedStateChangedEventArgs { Node = nodes[itemIndex], Selected = false });
        }

        base.OnItemUnselected (itemIndex);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="itemIndex"></param>
    protected override void OnItemDrag (HashSet<int> itemIndex)
    {
        var nodeSet = new HashSet<object> (itemIndex.Select (i => this.nodes[i]));

        if (NodeDrag != null)
        {
            NodeDrag (this, new NodeDragEventArgs { Nodes = nodeSet });
        }
        else
        {
            DoDragDrop (nodeSet, DragDropEffects.Copy);
        }

        base.OnItemDrag (itemIndex);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="gr"></param>
    /// <param name="info"></param>
    protected override void DrawItem (Graphics gr, VisibleItemInfo info)
    {
        if (PaintNode != null)
        {
            PaintNode (this,
                new PaintNodeContentEventArgs { Graphics = gr, Info = info, Node = nodes[info.ItemIndex] });
        }
        else
        {
            base.DrawItem (gr, info);
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="eventArgs"></param>
    protected override void OnDragOverItem (DragOverItemEventArgs eventArgs)
    {
        base.OnDragOverItem (eventArgs);

        eventArgs.Tag = nodes[eventArgs.ItemIndex];

        DragOverNode?.Invoke (this, eventArgs);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="eventArgs"></param>
    protected override void OnDropOverItem (DragOverItemEventArgs eventArgs)
    {
        eventArgs.Tag = nodes[eventArgs.ItemIndex];

        DropOverNode?.Invoke (this, eventArgs);

        base.OnDropOverItem (eventArgs);
    }

    #endregion

    #region Properties

    /// <summary>
    ///
    /// </summary>
    [DefaultValue (true)]
    public bool AutoCollapse { get; set; }

    /// <summary>
    ///
    /// </summary>
    [DefaultValue (false)]
    public bool ShowRootNode { get; set; }

    /// <summary>
    ///
    /// </summary>
    [Browsable (false)]
    public IEnumerable<object> ExpandedNodes => expandedNodes;

    /// <summary>
    ///
    /// </summary>
    [Browsable (false)]
    public IEnumerable<object> SelectedNodes
    {
        get { return SelectedItemIndexes.OrderBy (i => i).Select (i => nodes[i]); }
    }

    /// <summary>
    ///
    /// </summary>
    [Browsable (false)]
    public object? SelectedNode => SelectedNodes.FirstOrDefault();

    /// <summary>
    ///
    /// </summary>
    [Browsable (false)]
    public IEnumerable<object> CheckedNodes
    {
        get { return CheckedItemIndex.OrderBy (i => i).Select (i => nodes[i]); }
    }

    /// <summary>
    ///
    /// </summary>
    [DefaultValue (true)]
    public bool UncheckChildWhenCollapsed { get; set; }

    /// <summary>
    /// List of all visible nodes
    /// </summary>
    [Browsable (false)]
    public IEnumerable<object> Nodes => nodes;

    /// <summary>
    ///
    /// </summary>
    protected List<object> nodes = new ();

    /// <summary>
    ///
    /// </summary>
    protected List<int> levels = new ();

    /// <summary>
    ///
    /// </summary>
    protected HashSet<object> expandedNodes = new ();

    /// <summary>
    ///
    /// </summary>
    protected BitArray hasChildren;

    /// <summary>
    ///
    /// </summary>
    [Browsable (false)]
    [DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
    public override int ItemCount
    {
        get => base.ItemCount;
        set => base.ItemCount = value;
    }

    /// <summary>
    ///
    /// </summary>
    [DefaultValue (16)]
    public override int ItemIndentDefault
    {
        get => base.ItemIndentDefault;
        set => base.ItemIndentDefault = value;
    }

    #endregion

    #region Construction

    /// <summary>
    /// Конуструктор.
    /// </summary>
    public FastTree()
    {
        hasChildren = null!;

        AutoCollapse = true;
        ShowRootNode = false;
        ShowExpandBoxes = true;
        UncheckChildWhenCollapsed = true;
        ItemIndentDefault = 16;

        if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
        {
            Build (new object[] { "Node 1", "Node 2", "Node 3" });
            SelectedItemIndexes.Add (0);
        }
    }

    #endregion

    #region Build

    /// <summary>
    ///
    /// </summary>
    protected object? root;

    /// <summary>
    ///
    /// </summary>
    /// <param name="rootNode"></param>
    public void Build (object? rootNode)
    {
        root = rootNode;

        //create set of selected and checked nodes
        var selected = new HashSet<object>();
        var check = new HashSet<object>();

        foreach (var i in SelectedItemIndexes)
        {
            selected.Add (nodes[i]);
        }

        foreach (var i in CheckedItemIndex)
        {
            check.Add (nodes[i]);
        }

        //
        nodes.Clear();
        levels.Clear();
        SelectedItemIndexes.Clear();
        CheckedItemIndex.Clear();

        //build list of expanded nodes
        if (ShowRootNode)
        {
            AddNode (rootNode, 0);
        }
        else
        {
            AddNodeChildren (rootNode, 0);
        }

        //restore indexes of selected and checked nodes
        var newExpanded = new HashSet<object>();
        hasChildren = new BitArray (nodes.Count);

        for (var i = 0; i < nodes.Count; i++)
        {
            var node = nodes[i];
            if (selected.Contains (node))
            {
                SelectedItemIndexes.Add (i);
            }

            if (check.Contains (node))
            {
                CheckedItemIndex.Add (i);
            }

            if (expandedNodes.Contains (node))
            {
                newExpanded.Add (node);
            }

            hasChildren[i] = GetNodeChildren (nodes[i]).Cast<object>().Any();
        }

        expandedNodes = newExpanded;
        ItemCount = nodes.Count;
        base.Build();
    }

    /// <summary>
    ///
    /// </summary>
    public void Rebuild()
    {
        if (root != null)
        {
            Build (root);
        }
    }

    private void AddNode (object? node, int level)
    {
        if (node == null || !GetNodeVisibility (node))
        {
            return;
        }

        //
        nodes.Add (node);
        levels.Add (level);

        //
        if (expandedNodes.Contains (node))
        {
            AddNodeChildren (node, level + 1);
        }
    }

    private void AddNodeChildren (object? node, int level)
    {
        foreach (var child in GetNodeChildren (node))
        {
            AddNode (child, level);
        }
    }

    protected virtual IEnumerable GetNodeChildren (object? node)
    {
        if (NodeChildrenNeeded != null)
        {
            var arg = new NodeChildrenNeededEventArgs() { Node = node };
            NodeChildrenNeeded (this, arg);
            if (arg.Children != null)
            {
                foreach (var child in arg.Children)
                {
                    yield return child;
                }
            }
        }
        else if (node is IEnumerable enumerable)
        {
            if (enumerable is not string)
            {
                foreach (var child in enumerable)
                {
                    yield return child;
                }
            }
        }
    }

    #endregion Build

    #region Additional methods

    /// <summary>
    ///
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public virtual object? GetNodeByIndex (int index)
    {
        if (index < 0 || index >= nodes.Count)
        {
            return null;
        }

        return nodes[index];
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public virtual int GetItemLevel (int index)
    {
        return levels[index];
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="itemIndex"></param>
    /// <returns></returns>
    public override bool ExpandItem (int itemIndex)
    {
        if (itemIndex < 0 || itemIndex >= nodes.Count)
        {
            return false;
        }

        var list = GetNodeChildren (nodes[itemIndex]).Cast<object>().ToList();
        if (list.Count > 0)
        {
            if (CanExpandItem (itemIndex))
            {
                expandedNodes.Add (nodes[itemIndex]);
                Build (root);
                if (itemIndex < nodes.Count)
                {
                    OnItemExpanded (itemIndex);
                }

                return true;
            }
        }

        return false;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="sequence"></param>
    public virtual void ExpandNodesUnsafe (IEnumerable<object> sequence)
    {
        foreach (var node in sequence)
        {
            expandedNodes.Add (node);
        }

        BuildNeeded();
        Invalidate();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="itemIndex"></param>
    /// <returns></returns>
    public override bool CollapseItem (int itemIndex)
    {
        return CollapseItem (itemIndex, true);
    }

    protected virtual bool CollapseItem (int itemIndex, bool build)
    {
        if (itemIndex < 0 || itemIndex >= nodes.Count)
        {
            return false;
        }

        if (CanCollapseItem (itemIndex))
        {
            //range of collapsing nodes
            var i = itemIndex + 1;
            var level = levels[itemIndex];
            while (i < nodes.Count && levels[i] > level)
                i++;

            var from = itemIndex + 1;
            var to = i - 1;
            if (to < from)
            {
                return true;
            }

            //check selection, checked
            foreach (var j in SelectedItemIndexes)
            {
                if (j >= from && j <= to)
                {
                    if (!CanUnselectItem (j))
                    {
                        return false;
                    }
                }
            }

            foreach (var j in CheckedItemIndex)
            {
                if (j >= from && j <= to)
                {
                    if (!CanUncheckItem (j))
                    {
                        return false;
                    }
                }
            }

            for (var j = from; j <= to; j++)
                if (expandedNodes.Contains (nodes[j]))
                {
                    if (!CanCollapseItem (j))
                    {
                        return false;
                    }
                }

            //unselect, uncheck
            for (var j = from; j <= to; j++)
            {
                UnselectItem (j);
                if (UncheckChildWhenCollapsed)
                {
                    UncheckItem (j);
                }

                if (expandedNodes.Contains (nodes[j]))
                {
                    expandedNodes.Remove (nodes[j]);
                    OnItemCollapsed (j);
                }
            }

            //remove
            expandedNodes.Remove (nodes[itemIndex]);

            //
            OnItemCollapsed (itemIndex);
            if (build)
            {
                Build (root);
            }

            return true;
        }

        return false;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    public virtual bool UnselectNode (object node)
    {
        return base.UnselectItem (nodes.IndexOf (node));
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="node"></param>
    /// <param name="unselectOtherItems"></param>
    /// <returns></returns>
    public virtual bool SelectNode (object node, bool unselectOtherItems = true)
    {
        return base.SelectItem (nodes.IndexOf (node), unselectOtherItems);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    public virtual bool UncheckNode (object node)
    {
        return base.UncheckItem (nodes.IndexOf (node));
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    public virtual bool CheckNode (object node)
    {
        return base.CheckItem (nodes.IndexOf (node));
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    public virtual bool IsNodeSelected (object node)
    {
        return SelectedItemIndexes.Contains (nodes.IndexOf (node));
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    public virtual bool IsNodeChecked (object node)
    {
        return GetItemChecked (nodes.IndexOf (node));
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    public virtual bool IsNodeExpanded (object node)
    {
        return expandedNodes.Contains (node);
    }

    private bool ExpandNodeAndChildren (object? node, int maxExpandLevelCount)
    {
        var list = GetNodeChildren (node).Cast<object>().ToList();
        if (list.Count > 0)
        {
            if (CanExpandNode (node))
            {
                expandedNodes.Add (node!);
                OnNodeExpanded (node!);

                if (maxExpandLevelCount > 1)
                {
                    foreach (var child in list)
                    {
                        ExpandNodeAndChildren (child, maxExpandLevelCount - 1);
                    }
                }

                return true;
            }
        }

        return false;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="node"></param>
    /// <param name="expandChildren"></param>
    /// <returns></returns>
    public bool ExpandNode (object node, bool expandChildren = false)
    {
        if (expandChildren)
        {
            if (ExpandNodeAndChildren (node, int.MaxValue))
            {
                Build (root);
                return true;
            }

            return false;
        }
        else
        {
            return ExpandItem (nodes.IndexOf (node));
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="node"></param>
    /// <param name="maxExpandLevelCount"></param>
    /// <returns></returns>
    public bool ExpandNode (object node, int maxExpandLevelCount)
    {
        if (maxExpandLevelCount > 1)
        {
            if (ExpandNodeAndChildren (node, maxExpandLevelCount))
            {
                Build (root);
                return true;
            }

            return false;
        }
        else
        {
            return ExpandItem (nodes.IndexOf (node));
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    public bool CollapseNode (object node)
    {
        return CollapseItem (nodes.IndexOf (node));
    }

    /// <summary>
    ///
    /// </summary>
    public void ExpandAll()
    {
        if (ShowRootNode)
        {
            ExpandNodeAndChildren (root, int.MaxValue);
        }
        else
        {
            foreach (var child in GetNodeChildren (root))
            {
                ExpandNodeAndChildren (child, int.MaxValue);
            }
        }

        Build (root);
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public bool CollapseAll()
    {
        var res = true;

        for (var i = nodes.Count - 1; i >= 0; i--)
            if (expandedNodes.Contains (nodes[i]))
            {
                res &= CollapseItem (i, false);
            }

        Build (root);

        return res;
    }

    /// <summary>
    /// Returns all expanded children of the node
    /// </summary>
    public virtual IEnumerable GetNodeExpandedChildren (object node, bool onlyFirstLevel = false)
    {
        var itemIndex = nodes.IndexOf (node);
        if (itemIndex < 0)
        {
            yield break;
        }

        foreach (var i in GetItemExpandedChildren (itemIndex, onlyFirstLevel))
        {
            yield return nodes[i];
        }
    }

    /// <summary>
    /// Returns all expanded children of the item
    /// </summary>
    public virtual IEnumerable<int> GetItemExpandedChildren (int itemIndex, bool onlyFirstLevel = false)
    {
        var i = itemIndex + 1;
        var level = levels[itemIndex];
        while (i < nodes.Count && levels[i] > level)
        {
            if ((!onlyFirstLevel) || (levels[i] == level + 1))
            {
                yield return i;
            }

            i++;
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    public int GetItemIndexOfNode (object node)
    {
        return nodes.IndexOf (node);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="node"></param>
    /// <param name="tryToCenter"></param>
    /// <returns></returns>
    public virtual bool ScrollToNode (object node, bool tryToCenter = false)
    {
        var itemIndex = GetItemIndexOfNode (node);
        if (itemIndex < 0 || itemIndex >= ItemCount)
        {
            return false;
        }

        var y = GetItemY (itemIndex);
        var height = GetItemHeight (itemIndex);
        if (tryToCenter)
        {
            y -= ClientSize.Height / 2 - 10;
            height += ClientSize.Height - 10;
        }

        ScrollToRectangle (new Rectangle (0, y, ClientRectangle.Width, height));
        return true;
    }

    #endregion

    #region Overrided methods

    /// <summary>
    ///
    /// </summary>
    /// <param name="itemIndex"></param>
    /// <returns></returns>
    public override int GetItemIndent (int itemIndex)
    {
        return GetIntNodeProperty (itemIndex, NodeIndentNeeded, levels[itemIndex] * ItemIndentDefault);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="itemIndex"></param>
    /// <returns></returns>
    protected override bool GetItemExpanded (int itemIndex)
    {
        return expandedNodes.Contains (nodes[itemIndex]);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="itemIndex"></param>
    /// <returns></returns>
    protected override bool CanExpandItem (int itemIndex)
    {
        return GetBoolNodeProperty (itemIndex, CanExpandNodeNeeded, hasChildren[itemIndex]);
    }

    /// <summary>
    /// This method is used only for programmatically expanding.
    /// For GUI expanding - use CanExpandItem
    /// </summary>
    protected virtual bool CanExpandNode (object? node)
    {
        if (CanExpandNodeNeeded != null)
        {
            _boolArg.Node = node;
            _boolArg.Result = true;
            CanExpandNodeNeeded (this, _boolArg);
            return _boolArg.Result;
        }

        return true;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="itemIndex"></param>
    /// <returns></returns>
    public override bool CheckItem (int itemIndex)
    {
        if (GetItemChecked (itemIndex))
        {
            return true;
        }

        Invalidate();

        if (CanCheckItem (itemIndex))
        {
            if (NodeCheckStateNeeded ==
                null) //add to CheckedItemIndex only if handler of NodeCheckStateNeeded is not assigned
            {
                CheckedItemIndex.Add (itemIndex);
            }

            OnItemChecked (itemIndex);
            return true;
        }

        return false;
    }

    /// <summary>
    ///
    /// </summary>
    protected override bool IsItemHeightFixed => NodeHeightNeeded == null;

    /// <summary>
    ///
    /// </summary>
    /// <param name="graphics"></param>
    /// <param name="eventArgs"></param>
    protected override void DrawDragOverInsertEffect
        (
            Graphics graphics,
            DragOverItemEventArgs eventArgs
        )
    {
        var c1 = Color.FromArgb (255, SelectionColor);
        var c2 = Color.Transparent;
        var c3 = BackColor;

        if (!visibleItemInfos.ContainsKey (eventArgs.ItemIndex))
        {
            return;
        }

        graphics.ResetClip();
        var info = visibleItemInfos[eventArgs.ItemIndex];
        var rect = new Rectangle (info.X_ExpandBox + 1, info.Y, 10000, info.Height);
        if (eventArgs.ItemIndex <= 0)
        {
            rect.Offset (0, 2);
        }

        switch (eventArgs.InsertEffect)
        {
            case InsertEffect.Replace:
                using (var brush = new SolidBrush (c1))
                {
                    graphics.FillRectangle (brush, rect);
                }

                break;

            case InsertEffect.InsertBefore:
                rect.Offset (0, -rect.Height / 2 - ItemInterval - 1);
                DrawDragDropMarker (graphics, rect, c1, c2, c3);
                break;

            case InsertEffect.InsertAfter:
                rect.Offset (0, rect.Height / 2);
                DrawDragDropMarker (graphics, rect, c1, c2, c3);
                break;

            case InsertEffect.AddAsChild:
                if (eventArgs.ItemIndex >= 0 && eventArgs.ItemIndex < ItemCount)
                {
                    var dx = GetItemIndent (eventArgs.ItemIndex) + ItemIndentDefault;
                    var r = new Rectangle (dx, rect.Y + rect.Height / 2, rect.Width, rect.Height);
                    DrawDragDropMarker (graphics, r, c1, c2, c3);
                    using (var pen = new Pen (c1))
                    {
                        graphics.DrawLines (pen, new PointF[]
                            {
                                new Point (r.Left, r.Top + r.Height / 2),
                                new Point (rect.Left + 8, r.Top + r.Height / 2),
                                new Point (rect.Left + 8, r.Top + r.Height / 2 - 5)
                            });
                    }
                }

                break;
        }
    }

    private static void DrawDragDropMarker (Graphics gr, Rectangle rect, Color c1, Color c2, Color c3)
    {
        var h = rect.Height;
        using (var brush = new LinearGradientBrush (rect, Color.Empty, Color.Empty, LinearGradientMode.Vertical))
        {
            brush.InterpolationColors = new ColorBlend()
            {
                Positions = new[] { 0, 0.2f, 0.8f, 1.0f },
                Colors = new[] { c2, c3, c3, c2 }
            };
            gr.FillRectangle (brush, new RectangleF (0, rect.Top, rect.Width, rect.Height));
        }

        rect = new Rectangle (rect.Left, rect.Top + h / 2 - 2, 50, 4);
        using (var brush = new SolidBrush (c3))
        {
            using (var pen = new Pen (c1))
            {
                gr.FillRectangle (brush, rect);
                gr.DrawRectangle (pen, rect);
            }
        }
    }

    #endregion

    #region Event Helpers

    private readonly Int32NodeEventArgs _int32Arg = new ();
    private readonly BoolNodeEventArgs _boolArg = new ();
    private readonly StringNodeEventArgs _stringArg = new ();
    private readonly ImageNodeEventArgs _imageArg = new ();
    private readonly StringAlignmentNodeEventArgs _alignArg = new ();
    private readonly ColorNodeEventArgs _colorArg = new ();

    private int GetIntNodeProperty
        (
            int itemIndex,
            EventHandler<Int32NodeEventArgs>? handler,
            int defaultValue
        )
    {
        if (handler != null)
        {
            _int32Arg.Node = nodes[itemIndex];
            _int32Arg.Result = defaultValue;
            handler (this, _int32Arg);
            return _int32Arg.Result;
        }

        return defaultValue;
    }

    private string GetStringNodeProperty
        (
            int itemIndex,
            EventHandler<StringNodeEventArgs>? handler,
            string defaultValue
        )
    {
        if (handler != null)
        {
            _stringArg.Node = nodes[itemIndex];
            _stringArg.Result = defaultValue;
            handler (this, _stringArg);
            return _stringArg.Result;
        }

        return defaultValue;
    }

    private bool GetBoolNodeProperty
        (
            int itemIndex,
            EventHandler<BoolNodeEventArgs>? handler,
            bool defaultValue
        )
    {
        if (handler != null)
        {
            _boolArg.Node = nodes[itemIndex];
            _boolArg.Result = defaultValue;
            handler (this, _boolArg);
            return _boolArg.Result;
        }

        return defaultValue;
    }

    private Image GetImageNodeProperty
        (
            int itemIndex,
            EventHandler<ImageNodeEventArgs>? handler,
            Image defaultValue
        )
    {
        if (handler != null)
        {
            _imageArg.Node = nodes[itemIndex];
            _imageArg.Result = defaultValue;
            handler (this, _imageArg);
            return _imageArg.Result;
        }

        return defaultValue;
    }

    private StringAlignment GetLineAlignmentNodeProperty
        (
            int itemIndex,
            EventHandler<StringAlignmentNodeEventArgs>? handler,
            StringAlignment defaultValue
        )
    {
        if (handler != null)
        {
            _alignArg.Node = nodes[itemIndex];
            _alignArg.Result = defaultValue;
            handler (this, _alignArg);
            return _alignArg.Result;
        }

        return defaultValue;
    }

    private Color GetColorNodeProperty
        (
            int itemIndex,
            EventHandler<ColorNodeEventArgs>? handler,
            Color defaultValue
        )
    {
        if (handler != null)
        {
            _colorArg.Node = nodes[itemIndex];
            _colorArg.Result = defaultValue;
            handler (this, _colorArg);
            return _colorArg.Result;
        }

        return defaultValue;
    }

    #endregion Helpers
}
