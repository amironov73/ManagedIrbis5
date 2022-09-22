// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable RedundantNameQualifier
// ReSharper disable UnusedMember.Global

/* TreeGrid.cs
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#endregion

namespace AM.Windows.Forms;

/// <summary>
///
/// </summary>

[System.ComponentModel.DesignerCategory ("Code")]
public class TreeGrid
    : SimpleScrollableControl
{
    #region Constants

    /// <summary>
    ///
    /// </summary>
    public const int DefaultDefaultColumn = -1;

    #endregion

    #region Construction

    /// <summary>
    /// Initializes a new instance of the <see cref="TreeGrid"/>
    /// class.
    /// </summary>
    public TreeGrid()
    {
        Columns = new TreeGridColumnCollection (this);
        Nodes = new TreeGridNodeCollection (this, null);
        Palette = new TreeGridPalette();
        _defaultColumn = DefaultDefaultColumn;

        SetStyle (ControlStyles.AllPaintingInWmPaint, true);
        SetStyle (ControlStyles.DoubleBuffer, true);
        SetStyle (ControlStyles.OptimizedDoubleBuffer, true);
        SetStyle (ControlStyles.ResizeRedraw, true);
        SetStyle (ControlStyles.Selectable, true);
        SetStyle (ControlStyles.StandardClick, true);
        SetStyle (ControlStyles.StandardDoubleClick, true);
        SetStyle (ControlStyles.UserPaint, true);
    }

    #endregion

    #region Events

    #endregion

    #region Properties

    /// <inheritdoc cref="Control.BackColor"/>
    public override Color BackColor
    {
        get => Palette.Background;
        set => Palette.Background.Color = value;
    }

    /// <summary>
    /// Gets the columns.
    /// </summary>
    /// <value>The columns.</value>
    [DesignerSerializationVisibility (DesignerSerializationVisibility.Content)]
    public TreeGridColumnCollection Columns { get; }

    /// <summary>
    ///
    /// </summary>
    [Browsable (false)]
    [DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
    public TreeGridNode? CurrentNode
    {
        get => FlattenedNodes.SafeAt (CurrentLine);
        set
        {
            try
            {
                var flatten = FlattenedNodes;
                var index = Array.IndexOf (flatten, value);
                if (index >= 0)
                {
                    GotoLine (index);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine (ex);
            }
        }
    }

    /// <summary>
    /// Gets or sets the default column.
    /// </summary>
    /// <value>The default column.</value>
    [DefaultValue (DefaultDefaultColumn)]
    public int DefaultColumn
    {
        get => _defaultColumn;
        set
        {
            if (!DesignMode)
            {
                if (value < 0 || value >= Columns.Count)
                {
                    throw new ArgumentOutOfRangeException (nameof (value));
                }
            }

            _defaultColumn = value;
        }
    } // property DefaultColumn

    /// <summary>
    /// Gets the default type of the node.
    /// </summary>
    /// <value>The default type of the node.</value>
    public virtual Type DefaultNodeType => typeof (TreeGridNode);

    /// <summary>
    ///
    /// </summary>
    public TreeGridNode[] FlattenedNodes => _flattenedNodes ??= GetFlattenedNodes();

    /// <summary>
    /// Gets or sets the foreground color of the control.
    /// </summary>
    /// <value></value>
    /// <returns>
    /// The foreground <see cref="T:System.Drawing.Color"/> of the control. The default is the value of the <see cref="P:System.Windows.Forms.Control.DefaultForeColor"/> property.
    /// </returns>
    /// <PermissionSet>
    /// 	<IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/>
    /// </PermissionSet>
    public override Color ForeColor
    {
        get => Palette.Foreground;
        set => Palette.Foreground.Color = value;
    }

    /// <summary>
    /// Gets the height of the line.
    /// </summary>
    /// <value>The height of the line.</value>
    [Browsable (false)]
    public int LineHeight => (FontHeight + 1) * 4 / 3;

    /// <summary>
    /// Gets the nodes.
    /// </summary>
    /// <value>The nodes.</value>
    [DesignerSerializationVisibility (DesignerSerializationVisibility.Content)]
    public TreeGridNodeCollection Nodes { get; }

    /// <summary>
    ///
    /// </summary>
    [DesignerSerializationVisibility (DesignerSerializationVisibility.Content)]
    public TreeGridPalette Palette { get; }

    /// <summary>
    ///
    /// </summary>
    public TreeGridColumn[] VisibleColumns => _visibleColumns ??= GetVisibleColumns();

    /// <summary>
    ///
    /// </summary>
    public TreeGridNode[] VisibleNodes => _visibleNodes ??= GetVisibleNodes();

    /// <summary>
    ///
    /// </summary>
    public int VisibleNodeCount => VisibleNodes.Length;

    #endregion

    #region Private members

    internal int CurrentLine;
    private int _defaultColumn;

    private TreeGridEditor? _editor;
    private TreeGridColumn? _editingColumn;

    private bool _updateGuard;

    private int _leftColumnIndex;

    private int _topNodeIndex;
    private TreeGridColumn[]? _visibleColumns;
    private TreeGridNode[]? _visibleNodes;
    private TreeGridNode[]? _flattenedNodes;

    private TreeGridColumn? _sizingColumn;
    private Cursor? _savedCursor;

    /// <summary>
    /// Gets the visible columns.
    /// </summary>
    /// <returns></returns>
    protected internal virtual TreeGridColumn[] GetVisibleColumns
        (
        )
    {
        for (var i = 0; i < Columns.Count; i++)
        {
            Columns[i]._index = i;
        }

        var totalFill = Columns.Sum (_ => _.FillFactor);
        if (totalFill != 0)
        {
            var totalNotFill = Columns
                .Where (_ => _.FillFactor == 0)
                .Sum (_ => _.Width);
            var totalFree = ClientSize.Width - totalNotFill - 2;
            foreach (var column in
                     Columns.Where (_ => _.FillFactor != 0))
            {
                column.Width = totalFree > 0
                    ? totalFree * column.FillFactor / totalFill
                    : column.FillFactor;
            }
        }

        var result = new List<TreeGridColumn>();
        var runningWidth = 0;
        var totalWidth = ClientSize.Width;

        for (var i = _leftColumnIndex; i < Columns.Count; i++)
        {
            var currentColumn = Columns[i];
            if (runningWidth < totalWidth)
            {
                result.Add (currentColumn);
            }

            currentColumn._left = runningWidth;
            runningWidth += currentColumn.Width;
            currentColumn._right = runningWidth;
            runningWidth++;
        }

        HorizontalScroll.Visible = result.Count != Columns.Count;
        HorizontalScroll.Maximum = result.Count;

        return result.ToArray();
    }

    /// <summary>
    /// Gets the visible nodes.
    /// </summary>
    /// <returns></returns>
    protected internal virtual TreeGridNode[] GetVisibleNodes()
    {
        var flattenedNodes = FlattenedNodes;

        var result = new List<TreeGridNode>();
        var runningHeight = 0;
        var totalHeight = ClientSize.Height - LineHeight;

        var top = LineHeight;
        for (var i = _topNodeIndex; i < flattenedNodes.Length; i++)
        {
            var currentNode = flattenedNodes[i];
            currentNode.Top = top;
            top += currentNode.Height;
            runningHeight += currentNode.Height;
            if (runningHeight >= totalHeight)
            {
                break;
            }

            result.Add (currentNode);
        }

        return result.ToArray();
    }

    /// <summary>
    /// Gets the flattened nodes.
    /// </summary>
    /// <returns></returns>
    protected internal TreeGridNode[] GetFlattenedNodes()
    {
        var result = new List<TreeGridNode>();
        foreach (var node in Nodes)
        {
            result.AddRange (node.Flatten());
        }

        for (var i = 0; i < result.Count; i++)
        {
            result[i].FlatIndex = i;
        }

        return result.ToArray();
    }

    /// <summary>
    /// Resets the flattened nodes.
    /// </summary>
    protected internal void ResetFlattenedNodes()
    {
        _flattenedNodes = GetFlattenedNodes();
    }

    /// <summary>
    ///
    /// </summary>
    protected internal void ResetVisibleColumns()
    {
        _visibleColumns = GetVisibleColumns();
    }

    /// <summary>
    ///
    /// </summary>
    protected internal void ResetVisibleNodes()
    {
        _visibleNodes = GetVisibleNodes();
    }

    /// <summary>
    /// Raises the <see cref="E:System.Windows.Forms.Control.Paint"/> event.
    /// </summary>
    /// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs"/> that contains the event data.</param>
    protected override void OnPaint (PaintEventArgs e)
    {
        //base.OnPaint(e);

        var visibleColumns = VisibleColumns;

        var graphics = e.Graphics;
        graphics.Clear (BackColor);
        var height = ClientSize.Height;

        foreach (var column in visibleColumns)
        {
            var r = new Rectangle
                (
                    column.Left,
                    0,
                    column.Width,
                    LineHeight
                );

            var dha
                = new TreeGridDrawColumnHeaderEventArgs
                {
                    Column = column,
                    Graphics = graphics,
                    Bounds = r,
                    Grid = this
                };
            column.OnDrawHeader (dha);

            graphics.DrawLine
                (
                    Palette.Lines,
                    column.Right,
                    0,
                    column.Right,
                    height
                );
        }

        var visibleNodes = VisibleNodes;

        foreach (var node in visibleNodes)
        {
            if (visibleColumns.Length != 0)
            {
                var left = visibleColumns.First().Left;
                var right = visibleColumns.Last().Right;
                var top = node.Top;
                var nodeHeight = node.Height;

                var r = new Rectangle
                    (
                        left,
                        top,
                        right - left + 1,
                        nodeHeight
                    );

                var args
                    = new TreeGridDrawNodeEventArgs
                    {
                        Graphics = graphics,
                        Grid = this,
                        Bounds = r,
                        Node = node,
                        State = GetNodeState (node)
                    };
                node.OnDrawRow (args);

                var bottom = top - 1;

                graphics.DrawLine
                    (
                        Palette.Lines,
                        left,
                        bottom,
                        right,
                        bottom
                    );
            }
        }
    }

    /// <summary>
    /// Updates the state.
    /// </summary>
    public void UpdateState()
    {
        if (!_updateGuard)
        {
            try
            {
                _updateGuard = true;
                RecalculateSize();
                Invalidate();
            }
            finally
            {
                _updateGuard = false;
            }
        }
    }

    /// <inheritdoc cref="Control.OnResize"/>
    protected override void OnResize (EventArgs e)
    {
        base.OnResize (e);
        RecalculateSize();
    }

    private void RecalculateSize()
    {
        ResetFlattenedNodes();
        ResetVisibleNodes();
        ResetVisibleColumns();
        VerticalScroll.Maximum = VisibleNodes.Length;
        HorizontalScroll.Maximum = Columns.Count;
        if (CurrentNode != null)
        {
            GotoLine (CurrentNode.FlatIndex);
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="line"></param>
    protected internal void GotoLine (int line)
    {
        var flattenNodes = FlattenedNodes;
        var visibleNodes = VisibleNodes;
        var flattenCount = flattenNodes.Length;
        var visibleCount = visibleNodes.Length;
        if (line >= flattenCount)
        {
            line = flattenCount - 1;
        }

        if (line < 0)
        {
            line = 0;
        }

        if (_topNodeIndex + visibleCount <= line)
        {
            _topNodeIndex = line - visibleCount + 1;
        }

        if (_topNodeIndex > line)
        {
            _topNodeIndex = line;
        }

        if (_topNodeIndex < 0)
        {
            _topNodeIndex = 0;
        }

        CurrentLine = line;
        VerticalScroll.Position = line;
        UpdateState();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="args">The <see cref="System.Windows.Forms.ScrollEventArgs"/> instance containing the event data.</param>
    protected override void OnScroll (ScrollEventArgs args)
    {
        base.OnScroll (args);

        //Debug.WriteLine(args.NewValue);
        switch (args.ScrollOrientation)
        {
            case ScrollOrientation.VerticalScroll:
                GotoLine (args.NewValue);
                break;
            case ScrollOrientation.HorizontalScroll:
                _leftColumnIndex = args.NewValue;
                break;
        }

        Invalidate();
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    protected internal TreeGridColumn? FindEditableColumn()
    {
        if (DefaultColumn > 0)
        {
            return Columns[DefaultColumn];
        }

        return Columns.FirstOrDefault (item => item.Editable);
    }

    /// <summary>
    ///
    /// </summary>
    protected override bool ProcessKeyEventArgs (ref Message m)
    {
        var letter = KeyboardUtility.ProcessKeyEventArgs (ref m);
        if (letter != null)
        {
            var column = FindEditableColumn();
            if (column != null)
            {
                BeginEdit (column.Index, letter.ToString());
                return true;
            }
        }

        return base.ProcessKeyEventArgs (ref m);
    }

    /// <inheritdoc cref="Control.ProcessDialogKey"/>
    protected override bool ProcessDialogKey (Keys keyData)
    {
        switch (keyData)
        {
            case Keys.Down:
                GotoLine (CurrentLine + 1);
                return true;

            case Keys.Up:
                GotoLine (CurrentLine - 1);
                return true;

            case Keys.PageDown:
                GotoLine (CurrentLine + VisibleNodeCount);
                return true;

            case Keys.PageUp:
                GotoLine (CurrentLine - VisibleNodeCount);
                return true;

            case Keys.Left:
                ExpandCurrentNode (false);
                return true;

            case Keys.Right:
                ExpandCurrentNode (true);
                return true;

            case Keys.Enter:
            {
                var column = FindEditableColumn();
                if (column != null)
                {
                    var initialValue = GetInitialValue
                        (
                            CurrentNode,
                            column
                        );

                    BeginEdit
                        (
                            column.Index,
                            initialValue
                        );
                }
            }
                return true;
        }

        return base.ProcessDialogKey (keyData);
    }

    /// <summary>
    ///
    /// </summary>
    public virtual string? GetInitialValue
        (
            TreeGridNode? node,
            TreeGridColumn? column
        )
    {
        string? result = null;

        if (column is not null && node is not null)
        {
            var data = node.Data.SafeGet (column.Index - 1);
            if (data != null)
            {
                result = data.ToString();
            }
        }

        return result;
    }

    /// <inheritdoc cref="Control.OnKeyDown"/>
    protected override void OnKeyDown
        (
            KeyEventArgs eventArgs
        )
    {
        base.OnKeyDown (eventArgs);
        switch (eventArgs.KeyData)
        {
            case Keys.Down:
                GotoLine (CurrentLine + 1);
                eventArgs.Handled = true;
                break;

            case Keys.Up:
                GotoLine (CurrentLine - 1);
                eventArgs.Handled = true;
                break;

            case Keys.PageDown:
                GotoLine (CurrentLine + VisibleNodeCount);
                eventArgs.Handled = true;
                break;

            case Keys.PageUp:
                GotoLine (CurrentLine - VisibleNodeCount);
                eventArgs.Handled = true;
                break;
        }
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Expands all.
    /// </summary>
    /// <param name="expand">if set to <c>true</c> [expand].</param>
    public void ExpandAll (bool expand)
    {
        foreach (var node in Nodes)
        {
            node.ExpandAll (expand);
        }

        UpdateState();
    }

    /// <summary>
    /// Expands the current node.
    /// </summary>
    /// <param name="expand">if set to <c>true</c> [expand].</param>
    public void ExpandCurrentNode (bool expand)
    {
        var currentNode = CurrentNode;
        if (currentNode == null)
        {
            return;
        }

        if (expand)
        {
            currentNode.Expanded = true;
            ResetFlattenedNodes();
            var first = currentNode.Nodes.FirstOrDefault();
            if (first != null)
            {
                GotoLine (first.FlatIndex);
            }
            else
            {
                GotoLine (currentNode.FlatIndex + 1);
            }
        }
        else
        {
            if (currentNode.HasChildren)
            {
                if (currentNode.Expanded)
                {
                    currentNode.Expanded = false;
                }
                else
                {
                    if (currentNode.Parent != null)
                    {
                        currentNode.Parent.Expanded = false;
                        GotoLine (currentNode.Parent.FlatIndex);
                    }
                }

                ResetFlattenedNodes();
            }
            else
            {
                if (currentNode.Parent != null)
                {
                    currentNode.Parent.Expanded = false;
                    GotoLine (currentNode.Parent.FlatIndex);
                }
            }
        }

        ResetVisibleNodes();
    }

    /// <summary>
    /// Creates the node.
    /// </summary>
    /// <returns></returns>
    public virtual TreeGridNode CreateNode()
    {
        var nodeType = DefaultNodeType;
        var result = (TreeGridNode)Activator.CreateInstance (nodeType).ThrowIfNull();
        result._SetTreeGrid (this);

        return result;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    public TreeGridNodeState GetNodeState (TreeGridNode node)
    {
        var result = TreeGridNodeState.Normal;

        if (node.FlatIndex == CurrentLine)
        {
            result |= TreeGridNodeState.Selected;
        }

        if (!node.Enabled)
        {
            result |= TreeGridNodeState.Disabled;
        }

        if (node.ReadOnly)
        {
            result |= TreeGridNodeState.ReadOnly;
        }

        return result;
    }

    /// <summary>
    /// Gets all nodes and subnodes.
    /// </summary>
    /// <returns></returns>
    public List<TreeGridNode> GetAllNodesAndSubnodes()
    {
        var result = new List<TreeGridNode> (Nodes);

        result.AddRange (Nodes.SelectMany (_ => _.GetAllSubNodes()));

        return result;
    }

    #endregion

    /// <summary>
    ///
    /// </summary>
    /// <param name="columnIndex"></param>
    /// <returns></returns>
    public bool EnsureColumnVisible (int columnIndex)
    {
        if (columnIndex < 0
            || columnIndex >= Columns.Count)
        {
            return false;
        }

        while (true)
        {
            if (VisibleColumns.Any (_ => _.Index == columnIndex))
            {
                break;
            }

            if (VisibleColumns.First().Index < columnIndex)
            {
                _leftColumnIndex--;
            }

            if (VisibleColumns.Last().Index > columnIndex)
            {
                _leftColumnIndex++;
            }

            UpdateState();
        }

        return true;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="columnIndex"></param>
    /// <param name="initialValue"></param>
    /// <returns></returns>
    public virtual bool BeginEdit
        (
            int columnIndex,
            string? initialValue
        )
    {
        EndEdit (false);
        var currentNode = CurrentNode;
        if (currentNode == null)
        {
            return false;
        }

        if (currentNode.ReadOnly)
        {
            return false;
        }

        if (!EnsureColumnVisible (columnIndex))
        {
            return false;
        }

        var column = Columns[columnIndex];
        var top = currentNode.Top;
        var bounds = new Rectangle
            (
                column.Left,
                top,
                column.Width,
                currentNode.Height
            );
        _editor = currentNode.CreateEditor (column, bounds, initialValue);
        _editingColumn = column;
        if (_editor is not null)
        {
            if (_editor.Control is { } editorControl)
            {
                Controls.Add (editorControl);
                editorControl.Focus();
            }

            return true;
        }

        return false;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="accept"></param>
    public virtual void EndEdit (bool accept)
    {
        if (_editor is not null)
        {
            if (accept)
            {
                if (CurrentNode is { } currentNode)
                {
                    if (_editingColumn is not null)
                    {
                        currentNode.AcceptData (_editor, _editingColumn.Index - 1);
                    }
                }
            }

            _editor.Dispose();

            //Controls.Remove(_editor); // ???
        }

        _editor = null;
        UpdateState();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    public virtual TreeGridMouseEventArgs? TranslateMouseClick
        (
            MouseEventArgs args
        )
    {
        var result = new TreeGridMouseEventArgs (args);

        var nodes = VisibleNodes;
        var node = nodes.FirstOrDefault (item => args.Y >= item.Top && args.Y < item.Bottom)
                   ?? nodes.LastOrDefault();

        if (node == null)
        {
            return null;
        }

        var columns = VisibleColumns;
        var column = columns.FirstOrDefault (item => args.X >= item.Left && args.X < item.Right)
                     ?? columns.LastOrDefault();

        if (column == null)
        {
            return null;
        }

        result.TreeGrid = this;
        result.Node = node;
        result.Column = column;

        return result;
    }

    /// <inheritdoc cref="Control.OnMouseClick"/>
    protected override void OnMouseClick (MouseEventArgs e)
    {
        base.OnMouseClick (e);

        if (_sizingColumn != null)
        {
            return;
        }

        EndEdit (false);

        var args = TranslateMouseClick (e);
        if (args == null)
        {
            return;
        }

        if (args.Node is { } node)
        {
            GotoLine (node.FlatIndex);
        }

        if (CurrentNode is { } currentNode)
        {
            currentNode.OnMouseClick (args);
        }
    }

    /// <inheritdoc cref="Control.OnMouseDoubleClick"/>
    protected override void OnMouseDoubleClick (MouseEventArgs e)
    {
        base.OnMouseDoubleClick (e);

        EndEdit (false);

        var currentNode = CurrentNode;
        var column = Columns
            .Skip (_leftColumnIndex)
            .FirstOrDefault (item => item._left < e.X && item._right > e.X);
        if (currentNode is not null)
        {
            var treeGridMouseEventArgs = new TreeGridMouseEventArgs (e)
            {
                TreeGrid = this,
                Node = currentNode,
                Column = column
            };
            currentNode.OnMouseDoubleClick (treeGridMouseEventArgs);
        }
    } // method OnMouseDoubleClick

    /// <inheritdoc cref="System.Windows.Forms.Control.OnMouseWheel"/>
    protected override void OnMouseWheel (MouseEventArgs e)
    {
        GotoLine (CurrentLine - e.Delta / 120);
    }

    /// <inheritdoc cref="Control.OnMouseDown"/>
    protected override void OnMouseDown (MouseEventArgs e)
    {
        base.OnMouseDown (e);
        var column = FindColumnToResize (e);
        if (column != null)
        {
            _sizingColumn = column;
            Capture = true;
        }
    }

    /// <summary>
    ///
    /// </summary>
    protected TreeGridColumn? FindColumnToResize (MouseEventArgs e)
    {
        return VisibleColumns
            .FirstOrDefault (item => item.Resizeable && Math.Abs (item._right - e.X) < 2);
    }

    /// <inheritdoc cref="Control.OnMouseMove"/>
    protected override void OnMouseMove (MouseEventArgs e)
    {
        if (Capture && _sizingColumn != null)
        {
            var width = e.X - _sizingColumn._left;
            if (width < 3)
            {
                width = 3;
            }

            var maxWidth = ClientSize.Width - _sizingColumn._left;
            if (width >= maxWidth)
            {
                width = maxWidth;
            }

            _sizingColumn.FillFactor = 0;
            _sizingColumn._width = width;
            _sizingColumn._right = _sizingColumn._left + width;

            //_Update();
            Invalidate();
            Application.DoEvents();
        }
        else
        {
            //_CalculateColumnWidths();
            var column = Columns
                .Skip (_leftColumnIndex)
                .FirstOrDefault (item => item.Resizeable
                                         && Math.Abs (item._right - e.X) < 2);
            if (column is not null && _savedCursor is null)
            {
                _savedCursor = Cursor;
                Cursor = Cursors.VSplit;
            }
            else if (_savedCursor is not null)
            {
                //Cursor = Cursors.Default;
                Cursor = _savedCursor;
                _savedCursor = null;
            }
        }
    }

    /// <summary>
    ///
    /// </summary>
    public virtual void OnDrawHeader
        (
            TreeGridDrawColumnHeaderEventArgs args
        )
    {
        args.Column?.OnDrawHeader (args);
    }

    /// <inheritdoc cref="Control.OnMouseUp"/>
    protected override void OnMouseUp (MouseEventArgs e)
    {
        if (Capture)
        {
            Capture = false;
            _sizingColumn = null;
            UpdateState();
        }
    }
}
