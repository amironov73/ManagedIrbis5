// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* TreeGridNode.cs
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#endregion

#nullable enable

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
    [DesignTimeVisible(false)]
    public class TreeGridNode
        : Component
    {
        #region Event

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="TreeGridNode"/> class.
        /// </summary>
        public TreeGridNode()
        {
            Enabled = true;

            _nodes = new TreeGridNodeCollection(null, this);
            _data = new TreeGridDataCollection(this);

            _backgroundColor = Color.Empty;
            _foregroundColor = Color.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeGridNode"/> class.
        /// </summary>
        /// <param name="title">The title.</param>
        public TreeGridNode(string title)
            : this()
        {
            _title = title;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeGridNode"/> class.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="data">The data.</param>
        public TreeGridNode
            (
                string title,
                params object[] data
            )
            : this(title)
        {
            _data.AddRange(data);
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when [before edit].
        /// </summary>
        public event EventHandler? BeforeEdit;

        /// <summary>
        /// Occurs when [after edit].
        /// </summary>
        public event EventHandler? AfterEdit;

        #endregion

        #region Private members

        private Color _backgroundColor;
        private bool _checked;
        private TreeGridDataCollection _data;
        private bool _enabled;
        private bool _expanded;
        private Font _font;
        private Color _foregroundColor;
        private Icon _icon;
        private int _height;
        internal int _top;

        private readonly TreeGridNodeCollection _nodes;
        internal TreeGridNode _parent;
        private bool _readOnly;

        private string _title;
        internal TreeGrid _treeGrid;

        private Type _editorType = typeof(TreeGridTextBox);

        /// <summary>
        /// Called when [before edit].
        /// </summary>
        protected virtual void OnBeforeEdit () =>
            BeforeEdit?.Invoke(this, EventArgs.Empty);

        /// <summary>
        /// Called when [after edit].
        /// </summary>
        protected virtual void OnAfterEdit () =>
            AfterEdit?.Invoke(this, EventArgs.Empty);

        protected internal virtual void OnDrawRow
            (
                TreeGridDrawNodeEventArgs args
            )
        {
            if (args != null)
            {
                TreeGrid grid = args.TreeGrid;
                if (grid != null)
                {
                    foreach (TreeGridColumn column in grid.VisibleColumns)
                    {
                        Rectangle bounds = args.Bounds;
                        bounds.X = column.Left;
                        bounds.Width = column.Width;

                        TreeGridDrawCellEventArgs tgdc
                            = new TreeGridDrawCellEventArgs
                            {
                                Graphics = args.Graphics,
                                TreeGrid = grid,
                                Node = args.Node,
                                Column = column,
                                Bounds = bounds,
                                State = args.State
                            };

                        column.OnDrawCell(tgdc);
                    }
                }
            }
        }

        protected internal virtual void OnDrawCell
            (
                TreeGridDrawCellEventArgs args
            )
        {
            if (args != null)
            {
                args.DrawBackground();
                args.DrawText();
                args.DrawSelection();
            }
        }

        internal void _SetTreeGrid(TreeGrid value)
        {
            _treeGrid = value;
            _nodes._grid = value;
            foreach (TreeGridNode child in Nodes)
            {
                child._SetTreeGrid(value);
            }
            _UpdateGrid();
        }

        protected internal virtual void OnKeyDown
            (
                KeyEventArgs args
            )
        {

        }

        protected internal virtual void OnMouseClick
            (
                TreeGridMouseEventArgs args
            )
        {
            args.Column.OnMouseClick(args);
        }

        protected internal virtual void OnMouseDoubleClick
            (
                TreeGridMouseEventArgs args
            )
        {
            args.Column.OnMouseDoubleClick(args);
        }

        protected internal virtual TreeGridEditor? CreateEditor
            (
                TreeGridColumn column,
                Rectangle bounds,
                string initialValue
            )
        {
            if (ReadOnly || !Enabled)
            {
                return null;
            }

            TreeGridEditor result = (TreeGridEditor) Activator
                    .CreateInstance(EditorType);

            //if (result != null)
            //{
            result.Control.Bounds = bounds;
            result.SetValue(initialValue
                            ?? (string) Data.SafeGet(0));
            if (!string.IsNullOrEmpty(initialValue))
            {
                result.SelectText(initialValue.Length,0);
            }
            result.Control.PreviewKeyDown
                += _editor_PreviewKeyDown;
            //}

            return result;
        }

        protected internal virtual void AcceptData
            (
                TreeGridEditor editor,
                int index
            )
        {
            if (editor != null)
            {
                Data.SafeSet(index, editor.GetValue());
                _UpdateGrid();
            }
        }

        void _editor_PreviewKeyDown
            (
                object sender,
                PreviewKeyDownEventArgs e
            )
        {
            switch (e.KeyData)
            {
                case Keys.Enter:
                    TreeGrid.EndEdit(true);
                    TreeGrid.GotoLine(TreeGrid.CurrentLine + 1);
                    e.IsInputKey = false;
                    break;

                case Keys.Escape:
                    TreeGrid.EndEdit(false);
                    e.IsInputKey = false;
                    break;

                case Keys.Down:
                    TreeGrid.EndEdit(true);
                    TreeGrid.GotoLine(TreeGrid.CurrentLine + 1);
                    e.IsInputKey = false;
                    break;

                case Keys.Up:
                    TreeGrid.EndEdit(true);
                    TreeGrid.GotoLine(TreeGrid.CurrentLine - 1);
                    e.IsInputKey = false;
                    break;
            }
        }

        internal void _UpdateGrid()
        {
            if (TreeGrid != null)
            {
                TreeGrid.Invalidate();
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the color of the background.
        /// </summary>
        /// <value>The color of the background.</value>
        [DefaultValue(typeof(Color),"Empty")]
        public virtual Color BackgroundColor
        {
            get
            {
                return _backgroundColor;
            }
            set
            {
                _backgroundColor = value;
                _UpdateGrid();
            }
        }

        /// <summary>
        /// Gets or sets the color of the foreground.
        /// </summary>
        /// <value>The color of the foreground.</value>
        [DefaultValue(typeof(Color),"Empty")]
        public virtual Color ForegroundColor
        {
            get
            {
                return _foregroundColor;
            }
            set
            {
                _foregroundColor = value;
                _UpdateGrid();
            }
        }

        /// <summary>
        /// Gets or sets the font.
        /// </summary>
        /// <value>The font.</value>
        [DefaultValue(null)]
        public Font Font
        {
            get { return _font ?? TreeGrid.Font; }
            set { _font = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="TreeGridNode"/> is checkable.
        /// </summary>
        /// <value><c>true</c> if checkable; otherwise, <c>false</c>.</value>
        [DefaultValue(false)]
        public bool Checkable
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this
        /// <see cref="TreeGridNode"/> is checked.
        /// </summary>
        /// <value><c>true</c> if checked; otherwise,
        /// <c>false</c>.</value>
        [DefaultValue(false)]
        public bool Checked
        {
            get
            {
                return _checked;
            }
            set
            {
                _checked = value;
                _UpdateGrid();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="TreeGridNode"/> is enabled.
        /// </summary>
        /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
        [DefaultValue(false)]
        public bool Enabled
        {
            get
            {
                return _enabled;
            }
            set
            {
                _enabled = value;
                _UpdateGrid();
            }
        }

        /// <summary>
        /// Gets or sets the icon.
        /// </summary>
        /// <value>The icon.</value>
        public Icon Icon
        {
            get
            {
                return _icon;
            }
            set
            {
                _icon = value;
                _UpdateGrid();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this
        /// <see cref="TreeGridNode"/> is expanded.
        /// </summary>
        /// <value><c>true</c> if expanded; otherwise,
        /// <c>false</c>.</value>
        [DefaultValue(false)]
        public bool Expanded
        {
            get
            {
                return _expanded;
            }
            set
            {
                _expanded = value;
                _UpdateGrid();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [read only].
        /// </summary>
        /// <value><c>true</c> if [read only]; otherwise, <c>false</c>.</value>
        [DefaultValue(false)]
        public virtual bool ReadOnly
        {
            get
            {
                return _readOnly;
            }
            set
            {
                _readOnly = value;
                _UpdateGrid();
            }
        }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value>The height.</value>
        public virtual int Height
        {
            get
            {
                int result = _height;
                if (result <= 0)
                {
                    result = (TreeGrid != null)
                        ? TreeGrid.LineHeight
                        : (Font.Height + 4);
                }
                return result;
            }
            set
            {
                _height = value;
                _UpdateGrid();
            }
        }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public virtual string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
                _UpdateGrid();
            }
        }

        /// <summary>
        /// Gets the top.
        /// </summary>
        /// <value>The top.</value>
        public int Top
        {
            get { return _top; }
        }

        /// <summary>
        /// Gets the bottom.
        /// </summary>
        /// <value>The bottom.</value>
        public int Bottom
        {
            get { return (Top + Height); }
        }

        /// <summary>
        /// Gets or sets the type of the editor.
        /// </summary>
        /// <value>The type of the editor.</value>
        public virtual Type EditorType
        {
            get
            {
                return _editorType;
            }
            set
            {
                if (!value.IsSubclassOf(typeof(TreeGridEditor)))
                {
                    throw new ArgumentException("value");
                }
                _editorType = value;
            }
        }

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <value>The data.</value>
        [DesignerSerializationVisibility
            (DesignerSerializationVisibility.Content)]
        public TreeGridDataCollection Data
        {
            get
            {
                return _data;
            }
        }

        /// <summary>
        /// Gets or sets the tree grid.
        /// </summary>
        /// <value>The tree grid.</value>
        [Browsable(false)]
        [DesignerSerializationVisibility
            (DesignerSerializationVisibility.Hidden)]
        public TreeGrid TreeGrid
        {
            get
            {
                return _treeGrid;
            }
        }

        /// <summary>
        /// Gets or sets the parent.
        /// </summary>
        /// <value>The parent.</value>
        [Browsable(false)]
        [DesignerSerializationVisibility
            (DesignerSerializationVisibility.Hidden)]
        public TreeGridNode Parent
        {
            get
            {
                return _parent;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has children.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has children; otherwise, <c>false</c>.
        /// </value>
        [Browsable(false)]
        [DesignerSerializationVisibility
            (DesignerSerializationVisibility.Hidden)]
        public bool HasChildren
        {
            get
            {
                return Nodes.FirstOrDefault() != null;
            }
        }

        /// <summary>
        /// Gets the level.
        /// </summary>
        /// <value>The level.</value>
        [Browsable(false)]
        [DesignerSerializationVisibility
            (DesignerSerializationVisibility.Hidden)]
        public int Level
        {
            get
            {
                int result = 0;
                for (TreeGridNode node = this; node.Parent != null;
                    node = node.Parent)
                    result++;
                return result;
            }
        }

        /// <summary>
        /// Gets the children.
        /// </summary>
        /// <value>The children.</value>
        [DesignerSerializationVisibility
            (DesignerSerializationVisibility.Content)]
        public TreeGridNodeCollection Nodes
        {
            get
            {
                return _nodes;
            }
        }

        /// <summary>
        /// Gets the bounds.
        /// </summary>
        /// <value>The bounds.</value>
        public Rectangle Bounds
        {
            get
            {
                Rectangle result = new Rectangle
                    (
                        0,
                        Top,
                        TreeGrid.ClientSize.Width,
                        Height
                    );

                return result;
            }
        }

        /// <summary>
        /// Gets the index of the flat.
        /// </summary>
        /// <value>The index of the flat.</value>
        public int FlatIndex { get { return _flatIndex; } }

        /// <summary>
        /// Gets or sets the user data.
        /// </summary>
        /// <value>The user data.</value>
        [DefaultValue(null)]
        public object UserData { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Flattens this instance.
        /// </summary>
        /// <returns></returns>
        public List<TreeGridNode> Flatten()
        {
            List<TreeGridNode> result = new List<TreeGridNode> { this };
            if (Expanded)
            {
                foreach (TreeGridNode child in Nodes)
                {
                    result.AddRange(child.Flatten());
                }
            }
            return result;
        }

        /// <summary>
        /// Expands all.
        /// </summary>
        /// <param name="expand">if set to <c>true</c> [expand].</param>
        public void ExpandAll(bool expand)
        {
            Expanded = expand;
            foreach (TreeGridNode node in Nodes)
            {
                node.ExpandAll(expand);
            }
        }

        /// <summary>
        /// Clones the specified with data.
        /// </summary>
        /// <param name="withData">if set to <c>true</c> [with data].</param>
        /// <returns></returns>
        public TreeGridNode Clone ( bool withData )
        {
            TreeGridNode result = (TreeGridNode) MemberwiseClone();

            result._data = new TreeGridDataCollection(result);
            if (withData)
            {
                for (int i = 0; i < Data.Count; i++)
                {
                    result.Data.SafeSet(i,Data.SafeGet(i));
                }
            }

            for ( int i = 0; i < Nodes.Count; i++ )
                {
                    TreeGridNode node = Nodes[i];
                    TreeGridNode clone = node.Clone(withData);
                    result.Nodes.Add(clone);
                }
            result._SetTreeGrid(TreeGrid);
            return result;
        }

        /// <summary>
        /// Ensures the visible.
        /// </summary>
        public void Select ()
        {
            TreeGrid.GotoLine(_flatIndex);
        }

        /// <summary>
        /// Selects the first editable.
        /// </summary>
        public void SelectFirstEditable ()
        {
            if (!HasChildren)
            {
                Select();
            }
            else
            {
                if (!ReadOnly)
                {
                    Select();
                }
                else
                {
                    foreach (TreeGridNode child in Nodes)
                    {
                        if (!child.ReadOnly)
                        {
                            child.Select();
                            return;
                        }
                    }
                    Select();
                }
            }
        }

        /// <summary>
        /// Gets all subnodes.
        /// </summary>
        /// <returns></returns>
        public List<TreeGridNode> GetAllSubnodes ()
        {
            List<TreeGridNode> result
                = new List<TreeGridNode>(Nodes);

            result.AddRange(Nodes.SelectMany(_=>_.GetAllSubnodes()));

            return result;
        }

        #endregion

        #region Object members

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return Title;
        }

        internal int _flatIndex;

        #endregion
    }
}
