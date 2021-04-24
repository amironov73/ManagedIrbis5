// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
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

using AM.Windows.Forms;

#endregion

namespace AM.Windows.Forms
{
    /// <summary>
    ///
    /// </summary>
    // ReSharper disable RedundantNameQualifier
// ReSharper disable LocalizableElement
    [System.ComponentModel.DesignerCategory("Code")]
// ReSharper restore LocalizableElement
    // ReSharper restore RedundantNameQualifier
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
            _columns = new TreeGridColumnCollection(this);
            _nodes = new TreeGridNodeCollection(this, null);
            _palette = new TreeGridPalette();
            _defaultColumn = DefaultDefaultColumn;

            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.Selectable, true);
            SetStyle(ControlStyles.StandardClick, true);
            SetStyle(ControlStyles.StandardDoubleClick, true);
            SetStyle(ControlStyles.UserPaint, true);
        }

        #endregion

        #region Events

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the background color for the control.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// A <see cref="T:System.Drawing.Color"/> that represents the background color of the control. The default is the value of the <see cref="P:System.Windows.Forms.Control.DefaultBackColor"/> property.
        /// </returns>
        /// <PermissionSet>
        /// 	<IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/>
        /// </PermissionSet>
        public override Color BackColor
        {
            get { return Palette.Backrground; }
            set { Palette.Backrground.Color = value; }
        }

        /// <summary>
        /// Gets the columns.
        /// </summary>
        /// <value>The columns.</value>
        [DesignerSerializationVisibility
            (DesignerSerializationVisibility.Content)]
        public TreeGridColumnCollection Columns
        {
            get
            {
                return _columns;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility
            (DesignerSerializationVisibility.Hidden)]
        public TreeGridNode CurrentNode
        {
            get
            {
                try
                {
                    return ( FlattenedNodes == null )
                        ? null
                        : FlattenedNodes[CurrentLine];
                }
                catch
                {
                    return null;
                }
            }
            set
            {
                try
                {
                    TreeGridNode[] flatten = FlattenedNodes;
                    int index = Array.IndexOf(flatten, value);
                    if (index >= 0)
                    {
                        GotoLine(index);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }
        }

        /// <summary>
        /// Gets or sets the default column.
        /// </summary>
        /// <value>The default column.</value>
        [DefaultValue(DefaultDefaultColumn)]
        public int DefaultColumn
        {
            get
            {
                return _defaultColumn;
            }
            set
            {
                if (!DesignMode)
                {
                    if ((value < 0) || (value >= Columns.Count))
                    {
                        throw new ArgumentOutOfRangeException("value");
                    }
                }
                _defaultColumn = value;
            }
        }

        /// <summary>
        /// Gets the default type of the node.
        /// </summary>
        /// <value>The default type of the node.</value>
        public virtual Type DefaultNodeType
        {
            get { return typeof(TreeGridNode); }
        }

        public TreeGridNode[] FlattenedNodes
        {
            get { return _flattenedNodes ?? (_flattenedNodes = GetFlattenedNodes()); }
        }

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
            get { return Palette.Foreground; }
            set { Palette.Foreground.Color = value; }
        }

        /// <summary>
        /// Gets the height of the line.
        /// </summary>
        /// <value>The height of the line.</value>
        [Browsable(false)]
        public int LineHeight
        {
            get
            {
                return (FontHeight + 1) * 4 / 3;
            }
        }

        /// <summary>
        /// Gets the nodes.
        /// </summary>
        /// <value>The nodes.</value>
        [DesignerSerializationVisibility
            (DesignerSerializationVisibility.Content)]
        public TreeGridNodeCollection Nodes
        {
            get
            {
                return _nodes;
            }
        }

        [DesignerSerializationVisibility
            (DesignerSerializationVisibility.Content)]
        public TreeGridPalette Palette
        {
            get
            {
                return _palette;
            }
        }

        public TreeGridColumn[] VisibleColumns
        {
            get { return _visibleColumns ?? (_visibleColumns = GetVisibleColumns()); }
        }

        public TreeGridNode[] VisibleNodes
        {
            get { return _visibleNodes ?? (_visibleNodes = GetVisibleNodes()); }
        }

        public int VisibleNodeCount
        {
            get
            {
                return (VisibleNodes == null)
                    ? 0
                    : VisibleNodes.Length;
            }
        }

        #endregion

        #region Private members

        private readonly TreeGridColumnCollection _columns;

        internal int CurrentLine;
        private int _defaultColumn;

        private TreeGridEditor _editor;
        private TreeGridColumn _editingColumn;

        private bool _updateGuard;

        private int _leftColumnIndex;
        private readonly TreeGridNodeCollection _nodes;

        private readonly TreeGridPalette _palette;
        private int _topNodeIndex;
        private TreeGridColumn[] _visibleColumns;
        private TreeGridNode[] _visibleNodes;
        private TreeGridNode[] _flattenedNodes;

        private TreeGridColumn _sizingColumn;
        private Cursor _savedCursor;

        /// <summary>
        /// Gets the visible columns.
        /// </summary>
        /// <returns></returns>
        protected internal virtual TreeGridColumn[] GetVisibleColumns
            (
            )
        {
            for (int i = 0; i < Columns.Count; i++)
            {
                Columns[i]._index = i;
            }

            int totalFill = Columns.Sum(_ => _.FillFactor);
            if (totalFill != 0)
            {
                int totalNotFill = Columns
                    .Where(_ => _.FillFactor == 0)
                    .Sum(_ => _.Width);
                int totalFree = ClientSize.Width - totalNotFill - 2;
                foreach (TreeGridColumn column in
                    Columns.Where(_ => _.FillFactor != 0))
                {
                    column.Width = (totalFree > 0)
                                       ? totalFree * column.FillFactor / totalFill
                                       : column.FillFactor;
                }
            }

            List<TreeGridColumn> result = new List<TreeGridColumn>();
            int runningWidth = 0;
            int totalWidth = ClientSize.Width;

            for (int i = _leftColumnIndex; i < Columns.Count; i++)
            {
                TreeGridColumn currentColumn = Columns[i];
                if (runningWidth < totalWidth)
                {
                    result.Add(currentColumn);
                }
                currentColumn._left = runningWidth;
                runningWidth += currentColumn.Width;
                currentColumn._right = runningWidth;
                runningWidth++;
            }

            HorizontalScroll.Visible = (result.Count != Columns.Count);
            HorizontalScroll.Maximum = result.Count;

            return result.ToArray();
        }

        /// <summary>
        /// Gets the visible nodes.
        /// </summary>
        /// <returns></returns>
        protected internal virtual TreeGridNode[] GetVisibleNodes()
        {
            TreeGridNode[] flattenedNodes = FlattenedNodes;

            List<TreeGridNode> result = new List<TreeGridNode>();
            int runningHeight = 0;
            int totalHeight = ClientSize.Height - LineHeight;

            int top = LineHeight;
            for (int i = _topNodeIndex; i < flattenedNodes.Length; i++)
            {
                TreeGridNode currentNode = flattenedNodes[i];
                currentNode.Top = top;
                top += currentNode.Height;
                runningHeight += currentNode.Height;
                if (runningHeight >= totalHeight)
                {
                    break;
                }
                result.Add(currentNode);
            }

            return result.ToArray();
        }

        /// <summary>
        /// Gets the flattened nodes.
        /// </summary>
        /// <returns></returns>
        protected internal TreeGridNode[] GetFlattenedNodes()
        {
            List<TreeGridNode> result = new List<TreeGridNode>();
            foreach (TreeGridNode node in Nodes)
            {
                result.AddRange(node.Flatten());
            }
            for (int i = 0; i < result.Count; i++)
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

        protected internal void ResetVisibleColumns()
        {
            _visibleColumns = GetVisibleColumns();
        }

        protected internal void ResetVisibleNodes()
        {
            _visibleNodes = GetVisibleNodes();
        }
        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.Paint"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs"/> that contains the event data.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            //base.OnPaint(e);

            TreeGridColumn[] visibleColumns = VisibleColumns;

            Graphics graphics = e.Graphics;
            graphics.Clear(BackColor);
            int height = ClientSize.Height;

            foreach (TreeGridColumn column in visibleColumns)
            {
                Rectangle r = new Rectangle
                    (
                        column.Left,
                        0,
                        column.Width,
                        LineHeight
                    );

                TreeGridDrawColumnHeaderEventArgs dha
                    = new TreeGridDrawColumnHeaderEventArgs
                    {
                        Column = column,
                        Graphics = graphics,
                        Bounds = r,
                        Grid = this
                    };
                column.OnDrawHeader(dha);

                graphics.DrawLine
                    (
                        Palette.Lines,
                        column.Right,
                        0,
                        column.Right,
                        height
                    );
            }

            TreeGridNode[] visibleNodes = VisibleNodes;

            foreach (TreeGridNode node in visibleNodes)
            {
                if (visibleColumns.Length != 0)
                {
                    int left = visibleColumns.First().Left;
                    int right = visibleColumns.Last().Right;
                    int top = node.Top;
                    int nodeHeight = node.Height;

                    Rectangle r = new Rectangle
                        (
                        left,
                        top,
                        right - left + 1,
                        nodeHeight
                        );

                    TreeGridDrawNodeEventArgs args
                        = new TreeGridDrawNodeEventArgs
                              {
                                  Graphics = graphics,
                                  Grid = this,
                                  Bounds = r,
                                  Node = node,
                                  State = GetNodeState(node)
                              };
                    node.OnDrawRow(args);

                    int bottom = top - 1;

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

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
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
                GotoLine(CurrentNode.FlatIndex);
            }
        }

        protected internal void GotoLine(int line)
        {
            TreeGridNode[] flattenNodes = FlattenedNodes;
            TreeGridNode[] visibleNodes = VisibleNodes;
            int flattenCount = flattenNodes.Length;
            int visibleCount = visibleNodes.Length;
            if (line >= flattenCount)
            {
                line = flattenCount - 1;
            }
            if (line < 0)
            {
                line = 0;
            }
            if ((_topNodeIndex + visibleCount) <= line)
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
        protected override void OnScroll(ScrollEventArgs args)
        {
            base.OnScroll(args);
            //Debug.WriteLine(args.NewValue);
            switch (args.ScrollOrientation)
            {
                case ScrollOrientation.VerticalScroll:
                    GotoLine(args.NewValue);
                    break;
                case ScrollOrientation.HorizontalScroll:
                    _leftColumnIndex = args.NewValue;
                    break;
            }
            Invalidate();
        }

        protected internal TreeGridColumn FindEditableColumn ()
        {
            if (DefaultColumn > 0)
            {
                return Columns[DefaultColumn];
            }
            TreeGridColumn result = Columns
                .Where(_ => _.Editable)
                .FirstOrDefault();
            return result;
        }

        protected override bool ProcessKeyEventArgs(ref Message m)
        {
            char? letter = KeyboardUtility.ProcessKeyEventArgs(ref m);
            if (letter != null)
            {
                TreeGridColumn column = FindEditableColumn();
                if (column != null)
                {
                    BeginEdit(column.Index, letter.ToString());
                    return true;
                }
            }
            return base.ProcessKeyEventArgs(ref m);
        }

        /// <summary>
        /// Processes a dialog key.
        /// </summary>
        /// <param name="keyData">One of the <see cref="T:System.Windows.Forms.Keys"/> values that represents the key to process.</param>
        /// <returns>
        /// true if the key was processed by the control; otherwise, false.
        /// </returns>
        protected override bool ProcessDialogKey(Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Down:
                    GotoLine(CurrentLine + 1);
                    return true;
                case Keys.Up:
                    GotoLine(CurrentLine - 1);
                    return true;
                case Keys.PageDown:
                    GotoLine(CurrentLine + VisibleNodeCount);
                    return true;
                case Keys.PageUp:
                    GotoLine(CurrentLine - VisibleNodeCount);
                    return true;
                case Keys.Left:
                    ExpandCurrentNode(false);
                    return true;
                case Keys.Right:
                    ExpandCurrentNode(true);
                    return true;
                case Keys.Enter:
                    {
                        TreeGridColumn column = FindEditableColumn();
                        if (column != null)
                        {
                            string initialValue = GetInitialValue
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
            return base.ProcessDialogKey(keyData);
        }

        public virtual string GetInitialValue
            (
                TreeGridNode node,
                TreeGridColumn column
            )
        {
            string result = null;

            if ((column != null)
                && (node != null))
            {
                object data = node.Data.SafeGet(column.Index - 1);
                if (data != null)
                {
                    result = data.ToString();
                }
            }

            return result;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            switch (e.KeyData)
            {
                case Keys.Down:
                    GotoLine(CurrentLine + 1);
                    e.Handled = true;
                    break;
                case Keys.Up:
                    GotoLine(CurrentLine - 1);
                    e.Handled = true;
                    break;
                case Keys.PageDown:
                    GotoLine(CurrentLine + VisibleNodeCount);
                    e.Handled = true;
                    break;
                case Keys.PageUp:
                    GotoLine(CurrentLine - VisibleNodeCount);
                    e.Handled = true;
                    break;
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Expands all.
        /// </summary>
        /// <param name="expand">if set to <c>true</c> [expand].</param>
        public void ExpandAll(bool expand)
        {
            foreach (TreeGridNode node in Nodes)
            {
                node.ExpandAll(expand);
            }
            UpdateState();
        }

        /// <summary>
        /// Expands the current node.
        /// </summary>
        /// <param name="expand">if set to <c>true</c> [expand].</param>
        public void ExpandCurrentNode(bool expand)
        {
            TreeGridNode currentNode = CurrentNode;
            if (currentNode == null)
            {
                return;
            }
            if (expand)
            {
                currentNode.Expanded = true;
                ResetFlattenedNodes();
                TreeGridNode first = currentNode.Nodes.FirstOrDefault();
                if (first != null)
                {
                    GotoLine(first.FlatIndex);
                }
                else
                {
                    GotoLine(currentNode.FlatIndex + 1);
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
                            GotoLine(currentNode.Parent.FlatIndex);
                        }
                    }
                    ResetFlattenedNodes();
                }
                else
                {
                    if (currentNode.Parent != null)
                    {
                        currentNode.Parent.Expanded = false;
                        GotoLine(currentNode.Parent.FlatIndex);
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
            Type nodeType = DefaultNodeType;
            TreeGridNode result = (TreeGridNode)
                Activator.CreateInstance(nodeType);
            result._SetTreeGrid(this);
            return result;
        }

        public TreeGridNodeState GetNodeState(TreeGridNode node)
        {
            TreeGridNodeState result = TreeGridNodeState.Normal;

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
        public List<TreeGridNode> GetAllNodesAndSubnodes ()
        {
            List<TreeGridNode> result = new List<TreeGridNode>(Nodes);

            result.AddRange(Nodes.SelectMany(_=>_.GetAllSubnodes()));

            return result;
        }

        #endregion

        public bool EnsureColumnVisible ( int columnIndex )
        {
            if ((columnIndex < 0)
                || (columnIndex >= Columns.Count))
            {
                return false;
            }

            while (true)
            {
                if (VisibleColumns.Any(_=>_.Index == columnIndex))
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

        public virtual bool BeginEdit(int columnIndex, string initialValue)
        {
            EndEdit(false);
            TreeGridNode currentNode = CurrentNode;
            if (currentNode == null)
            {
                return false;
            }
            if (currentNode.ReadOnly)
            {
                return false;
            }
            if (!EnsureColumnVisible(columnIndex))
            {
                return false;
            }
            TreeGridColumn column = Columns[columnIndex];
            int top = currentNode.Top;
            Rectangle bounds = new Rectangle
                (
                    column.Left,
                    top,
                    column.Width,
                    currentNode.Height
                );
            _editor = currentNode.CreateEditor(column, bounds, initialValue);
            _editingColumn = column;
            if (_editor != null)
            {
                Controls.Add(_editor.Control);
                _editor.Control.Focus();
                return true;
            }
            return false;
        }


        public virtual void EndEdit(bool accept)
        {
            if (_editor != null)
            {
                if (accept)
                {
                    TreeGridNode currentNode = CurrentNode;
                    if (currentNode != null)
                    {
                        currentNode.AcceptData(_editor,_editingColumn.Index-1);
                    }
                }
                _editor.Dispose();
                //Controls.Remove(_editor); // ???
            }
            _editor = null;
            UpdateState();
        }

        public virtual TreeGridMouseEventArgs TranslateMouseClick
            (
                MouseEventArgs args
            )
        {
            TreeGridMouseEventArgs result
                = new TreeGridMouseEventArgs(args);

            TreeGridNode [] nodes = VisibleNodes;
            TreeGridNode node = nodes
                                    .Where(_ => (args.Y >= _.Top) && (args.Y < _.Bottom))
                                    .FirstOrDefault()
                                    ?? nodes.LastOrDefault();

            if (node == null)
            {
                return null;
            }

            TreeGridColumn[] columns = VisibleColumns;
            TreeGridColumn column = columns
                .Where(_ => (args.X >= _.Left) && (args.X < _.Right))
                .FirstOrDefault()
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

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);

            if (_sizingColumn != null)
            {
                return;
            }

            EndEdit(false);

            TreeGridMouseEventArgs args = TranslateMouseClick(e);
            if (args == null)
            {
                return;
            }

            GotoLine(args.Node.FlatIndex);
            TreeGridNode currentNode = CurrentNode;
            if (currentNode != null)
            {
                currentNode.OnMouseClick(args);
            }
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);

            EndEdit(false);

            TreeGridNode currentNode = CurrentNode;
            TreeGridColumn column = Columns
                .Skip(_leftColumnIndex)
                .Where(_ => (_._left < e.X) && (_._right > e.X))
                .FirstOrDefault();
            if (currentNode != null)
            {
                TreeGridMouseEventArgs mea
                    = new TreeGridMouseEventArgs(e)
                    {
                        TreeGrid = this,
                        Node = currentNode,
                        Column = column
                    };
                currentNode.OnMouseDoubleClick(mea);
            }
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            GotoLine(CurrentLine - e.Delta / 120);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            TreeGridColumn column = FindColumnToResize(e);
            if (column != null)
            {
                _sizingColumn = column;
                Capture = true;
            }
        }

        protected TreeGridColumn FindColumnToResize(MouseEventArgs e)
        {
            return VisibleColumns
                .Where(_ => _.Resizeable
                            && (Math.Abs(_._right - e.X) < 2))
                .FirstOrDefault();
        }


        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (Capture && (_sizingColumn != null))
            {
                int width = e.X - _sizingColumn._left;
                if (width < 3)
                {
                    width = 3;
                }
                int maxWidth = ClientSize.Width - _sizingColumn._left;
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
                TreeGridColumn column = Columns
                    .Skip(_leftColumnIndex)
                    .Where(_ => _.Resizeable
                                && (Math.Abs(_._right - e.X) < 2))
                    .FirstOrDefault();
                if ((column != null) && (_savedCursor == null))
                {
                    _savedCursor = Cursor;
                    Cursor = Cursors.VSplit;
                }
                else if (_savedCursor != null)
                {
                    //Cursor = Cursors.Default;
                    Cursor = _savedCursor;
                    _savedCursor = null;
                }
            }
        }

        public virtual void OnDrawHeader
            (
                TreeGridDrawColumnHeaderEventArgs args
            )
        {
            args.Column.OnDrawHeader(args);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (Capture)
            {
                Capture = false;
                _sizingColumn = null;
                UpdateState();
            }
        }

    }
}
