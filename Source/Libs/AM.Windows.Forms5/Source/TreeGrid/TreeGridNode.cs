// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassWithVirtualMembersNeverInherited.Global
// ReSharper disable CommentTypo
// ReSharper disable EventNeverSubscribedTo.Global
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global

/* TreeGridNode.cs -- нода иерархического грида
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
    /// Нода иерархического грида.
    /// </summary>
    // ReSharper disable RedundantNameQualifier
    [System.ComponentModel.DesignerCategory("Code")]
    // ReSharper restore RedundantNameQualifier
    [DesignTimeVisible(false)]
    public class TreeGridNode
        : Component
    {
        #region Construction

        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        public TreeGridNode()
        {
            Enabled = true;

            Nodes = new TreeGridNodeCollection(null, this);
            Data = new TreeGridDataCollection(this);

            _backgroundColor = Color.White;
            _foregroundColor = Color.Black;
        }

        /// <summary>
        /// Конструктор с заголовком.
        /// </summary>
        /// <param name="title">Заголовок ноды.</param>
        public TreeGridNode
            (
                string title
            )
            : this()
        {
            _title = title;
        }

        /// <summary>
        /// Конструктор с заголовком и данными.
        /// </summary>
        /// <param name="title">Заголовок ноды.</param>
        /// <param name="data">Данные, ассоциированные с нодой.</param>
        public TreeGridNode
            (
                string title,
                params object?[] data
            )
            : this(title)
        {
            Data.AddRange(data);
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
        private bool _enabled;
        private bool _expanded;
        private Font? _font;
        private Color _foregroundColor;
        private Icon? _icon;
        private int _height;

        private bool _readOnly;

        private string? _title;

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

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"></param>
        protected internal virtual void OnDrawRow
            (
                TreeGridDrawNodeEventArgs args
            )
        {
            var grid = args.Grid;
            if (grid != null)
            {
                foreach (var column in grid.VisibleColumns)
                {
                    var bounds = args.Bounds;
                    bounds.X = column.Left;
                    bounds.Width = column.Width;

                    var tgdcea = new TreeGridDrawCellEventArgs
                        {
                            Graphics = args.Graphics,
                            Grid = grid,
                            Node = args.Node,
                            Column = column,
                            Bounds = bounds,
                            State = args.State
                        };

                    column.OnDrawCell(tgdcea);
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"></param>
        protected internal virtual void OnDrawCell
            (
                TreeGridDrawCellEventArgs args
            )
        {
            args.DrawBackground();
            args.DrawText();
            args.DrawSelection();
        }

        internal void _SetTreeGrid
            (
                TreeGrid? value
            )
        {
            Grid = value;
            Nodes._grid = value;
            foreach (var child in Nodes)
            {
                child._SetTreeGrid(value);
            }
            _UpdateGrid();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"></param>
        protected internal virtual void OnKeyDown
            (
                KeyEventArgs args
            )
        {
            // TODO: нужно ли тут что-нибудь делать?
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"></param>
        protected internal virtual void OnMouseClick
            (
                TreeGridMouseEventArgs args
            )
        {
            args.Column?.OnMouseClick(args);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"></param>
        protected internal virtual void OnMouseDoubleClick
            (
                TreeGridMouseEventArgs args
            )
        {
            args.Column?.OnMouseDoubleClick(args);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="column"></param>
        /// <param name="bounds"></param>
        /// <param name="initialValue"></param>
        /// <returns></returns>
        protected internal virtual TreeGridEditor? CreateEditor
            (
                TreeGridColumn column,
                Rectangle bounds,
                string? initialValue
            )
        {
            if (ReadOnly || !Enabled)
            {
                return null;
            }

            var result = (TreeGridEditor?) Activator.CreateInstance(EditorType);

            if (result != null)
            {
                var control = result.Control;
                if (control != null)
                {
                    control.Bounds = bounds;
                    result.SetValue(initialValue ?? (string?) Data.SafeGet(0));
                    if (!string.IsNullOrEmpty(initialValue))
                    {
                        result.SelectText(initialValue.Length, 0);
                    }

                    control.PreviewKeyDown
                        += _editor_PreviewKeyDown;
                }
            }

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="editor"></param>
        /// <param name="index"></param>
        protected internal virtual void AcceptData
            (
                TreeGridEditor? editor,
                int index
            )
        {
            if (editor != null)
            {
                Data.SafeSet(index, editor.GetValue());
                _UpdateGrid();
            }
        }

        private void _editor_PreviewKeyDown
            (
                object? sender,
                PreviewKeyDownEventArgs e
            )
        {
            switch (e.KeyData)
            {
                case Keys.Enter:
                    Grid?.EndEdit(true);
                    Grid?.GotoLine(Grid.CurrentLine + 1);
                    e.IsInputKey = false;
                    break;

                case Keys.Escape:
                    Grid?.EndEdit(false);
                    e.IsInputKey = false;
                    break;

                case Keys.Down:
                    Grid?.EndEdit(true);
                    Grid?.GotoLine(Grid.CurrentLine + 1);
                    e.IsInputKey = false;
                    break;

                case Keys.Up:
                    Grid?.EndEdit(true);
                    Grid?.GotoLine(Grid.CurrentLine - 1);
                    e.IsInputKey = false;
                    break;
            }
        }

        internal void _UpdateGrid()
        {
            if (Grid != null)
            {
                Grid.Invalidate();
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
            get => _backgroundColor;
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
            get => _foregroundColor;
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
            get => _font ?? Grid?.Font ?? throw new ApplicationException();
            set => _font = value;
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
            get => _checked;
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
            get => _enabled;
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
        public Icon? Icon
        {
            get => _icon;
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
            get => _expanded;
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
            get => _readOnly;
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
                var result = _height;
                if (result <= 0)
                {
                    result = Grid?.LineHeight ?? (Font.Height + 4);
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
        public virtual string? Title
        {
            get => _title;
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
        public int Top { get; internal set; }

        /// <summary>
        /// Gets the bottom.
        /// </summary>
        /// <value>The bottom.</value>
        public int Bottom => (Top + Height);

        /// <summary>
        /// Gets or sets the type of the editor.
        /// </summary>
        /// <value>The type of the editor.</value>
        public virtual Type EditorType
        {
            get => _editorType;
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
        [DesignerSerializationVisibility (DesignerSerializationVisibility.Content)]
        public TreeGridDataCollection Data { get; private set; }

        /// <summary>
        /// Gets or sets the tree grid.
        /// </summary>
        /// <value>The tree grid.</value>
        [Browsable(false)]
        [DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
        public TreeGrid? Grid { get; internal set; }

        /// <summary>
        /// Gets or sets the parent.
        /// </summary>
        /// <value>The parent.</value>
        [Browsable(false)]
        [DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
        public TreeGridNode? Parent { get; internal set; }

        /// <summary>
        /// Gets a value indicating whether this instance has children.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has children; otherwise, <c>false</c>.
        /// </value>
        [Browsable(false)]
        [DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
        public bool HasChildren => Nodes.FirstOrDefault() != null;

        /// <summary>
        /// Gets the level.
        /// </summary>
        /// <value>The level.</value>
        [Browsable(false)]
        [DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
        public int Level
        {
            get
            {
                var result = 0;
                for (var node = this; node.Parent != null;
                    node = node.Parent)
                    result++;
                return result;
            }
        }

        /// <summary>
        /// Gets the children.
        /// </summary>
        /// <value>The children.</value>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public TreeGridNodeCollection Nodes { get; }

        /// <summary>
        /// Gets the bounds.
        /// </summary>
        /// <value>The bounds.</value>
        public Rectangle Bounds
        {
            get
            {
                var result = new Rectangle
                    (
                        0,
                        Top,
                        Grid?.ClientSize.Width ?? 0,
                        Height
                    );

                return result;
            }
        }

        /// <summary>
        /// Gets the index of the flat.
        /// </summary>
        /// <value>The index of the flat.</value>
        public int FlatIndex { get; internal set; }

        /// <summary>
        /// Gets or sets the user data.
        /// </summary>
        /// <value>The user data.</value>
        [DefaultValue(null)]
        public object? UserData { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Flattens this instance.
        /// </summary>
        /// <returns></returns>
        public List<TreeGridNode> Flatten()
        {
            var result = new List<TreeGridNode> { this };
            if (Expanded)
            {
                foreach (var child in Nodes)
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
        public void ExpandAll
            (
                bool expand
            )
        {
            Expanded = expand;
            foreach (var node in Nodes)
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
            var result = (TreeGridNode) MemberwiseClone();

            result.Data = new TreeGridDataCollection(result);
            if (withData)
            {
                for (var i = 0; i < Data.Count; i++)
                {
                    result.Data.SafeSet(i, Data.SafeGet(i));
                }
            }

            for ( var i = 0; i < Nodes.Count; i++ )
            {
                var node = Nodes[i];
                var clone = node.Clone(withData);
                result.Nodes.Add(clone);
            }
            result._SetTreeGrid(Grid);
            return result;
        }

        /// <summary>
        /// Убеждаемся, что данная нода видимая.
        /// </summary>
        public void Select ()
        {
            Grid?.GotoLine(FlatIndex);
        }

        /// <summary>
        /// Выброр первой редактируемой ноды.
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
                    foreach (var child in Nodes)
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
        /// Список всех подчиненных нод.
        /// </summary>
        public List<TreeGridNode> GetAllSubnodes ()
        {
            var result = new List<TreeGridNode>(Nodes);
            result.AddRange(Nodes.SelectMany(_=>_.GetAllSubnodes()));

            return result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString() => Title ?? string.Empty;

        #endregion

    } // class TreeGridNode

} // namespace AM.Windows.Forms
