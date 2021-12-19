// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo

/* PickList.cs -- отбор элементов с помощью двух списков: доступные и отобранные
   Ars Magna project, http://library.istu.edu/am
 */

#region Using directives

using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms
{
    /// <summary>
    /// Отбор элементов с помощью двух списков: доступные и отобранные.
    /// </summary>
    public class PickList
        : UserControl
    {
        #region Properties

        /// <summary>
        /// Список доступных элементов.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ListBox AvailableList => _available.ThrowIfNull();

        ///<summary>
        /// Собственно доступные элементы.
        ///</summary>
        [Editor("System.Windows.Forms.Design.ListControlStringCollectionEditor, System.Design, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ListBox.ObjectCollection AvailableItems => _available.ThrowIfNull().Items;

        /// <summary>
        /// Список отобранных элементов.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ListBox SelectedList => _selected.ThrowIfNull();

        ///<summary>
        /// Собственно отобранные элементы.
        ///</summary>
        [Editor("System.Windows.Forms.Design.ListControlStringCollectionEditor, System.Design, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ListBox.ObjectCollection SelectedItems => _selected.ThrowIfNull().Items;

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PickList()
        {
            InitializeComponent();

            _rightOne!.Click += _rightOne_Click;
            _rightAll!.Click += _rightAll_Click;
            _leftOne!.Click += _leftOne_Click;
            _leftAll!.Click += _leftAll_Click;
            _available!.MouseDown += _available_MouseDown;
            _available!.DragEnter += _available_DragEnter;
            _available!.DragDrop += _available_DragDrop;
            _selected!.MouseDown += _selected_MouseDown;
            _selected!.DragEnter += _selected_DragEnter;
            _selected!.DragDrop += _selected_DragDrop;
        }

        #endregion

        #region Private members

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private readonly IContainer? components = null;

        private ListBox? _available;
        private ListBox? _selected;
        private Panel? _centerPanel;
        private Button? _rightOne;
        private Button? _rightAll;
        private Button? _leftOne;
        private Button? _leftAll;

        private void _rightAll_Click
            (
                object? sender,
                EventArgs e
            )
        {
            SelectedItems.AddRange(AvailableItems);
            AvailableItems.Clear();
        }

        private void _leftAll_Click
            (
                object? sender,
                EventArgs e
            )
        {
            AvailableItems.AddRange(SelectedItems);
            SelectedItems.Clear();
        }

        private void _rightOne_Click
            (
                object? sender,
                EventArgs e
            )
        {
            var selected = new ArrayList(_available!.SelectedItems);
            foreach (var obj in selected)
            {
                SelectedItems.Add(obj);
                AvailableItems.Remove(obj);
            }
        }

        private void _leftOne_Click
            (
                object? sender,
                EventArgs e
            )
        {
            var selected = new ArrayList(_selected!.SelectedItems);
            foreach (var obj in selected)
            {
                AvailableItems.Add(obj);
                SelectedItems.Remove(obj);
            }
        }

        private void _available_MouseDown
            (
                object? sender,
                MouseEventArgs e
            )
        {
            DoDragDrop(_available!, DragDropEffects.Move);
        }

        private void _selected_MouseDown
            (
                object? sender,
                MouseEventArgs e
            )
        {
            DoDragDrop(_selected!, DragDropEffects.Move);
        }

        private void _available_DragEnter
            (
                object? sender,
                DragEventArgs e
            )
        {
            var obj = e.Data!.GetData(typeof(ListBox));
            if (obj == _selected)
            {
                e.Effect = DragDropEffects.Move;
            }
        }

        private void _available_DragDrop
            (
                object? sender,
                DragEventArgs e
            )
        {
            _leftOne_Click(null, EventArgs.Empty);
        }

        private void _selected_DragEnter
            (
                object? sender,
                DragEventArgs e
            )
        {
            var obj = e.Data!.GetData(typeof(ListBox));
            if (obj == _available)
            {
                e.Effect = DragDropEffects.Move;
            }
        }

        private void _selected_DragDrop
            (
                object? sender,
                DragEventArgs e
            )
        {
            _rightOne_Click(null, EventArgs.Empty);
        }

        private void _available_DoubleClick
            (
                object? sender,
                EventArgs e
            )
        {
            var selectedItem = _available!.SelectedItem;
            if (selectedItem is not null)
            {
                SelectedItems.Add(selectedItem);
                AvailableItems.Remove(selectedItem);
            }
        }

        private void _selected_DoubleClick
            (
                object? sender,
                EventArgs e
            )
        {
            var selectedItem = _selected!.SelectedItem;
            if (selectedItem is not null)
            {
                AvailableItems.Add(selectedItem);
                SelectedItems.Remove(selectedItem);
            }
        }

        private void _available_MouseDoubleClick
            (
                object? sender,
                MouseEventArgs e
            )
        {
            var selectedItem = _available!.SelectedItem;
            if (selectedItem is not null)
            {
                SelectedItems.Add(selectedItem);
                AvailableItems.Remove(selectedItem);
            }
        }

        private void _selected_MouseDoubleClick
            (
                object? sender,
                MouseEventArgs e
            )
        {
            var selectedItem = _selected!.SelectedItem;
            if (selectedItem is not null)
            {
                AvailableItems.Add(selectedItem);
                SelectedItems.Remove(selectedItem);
            }
        }

        #endregion

        #region UserControl members

        /// <inheritdoc />
        protected override void OnResize
            (
                EventArgs e
            )
        {
            base.OnResize(e);
            var width = (Width - _centerPanel!.Width) / 2;
            _available!.Width = width;
            _selected!.Width = width;
        }

        /// <inheritdoc />
        protected override void Dispose
            (
                bool disposing
            )
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this._available = new System.Windows.Forms.ListBox();
            this._selected = new System.Windows.Forms.ListBox();
            this._centerPanel = new System.Windows.Forms.Panel();
            this._leftAll = new System.Windows.Forms.Button();
            this._leftOne = new System.Windows.Forms.Button();
            this._rightAll = new System.Windows.Forms.Button();
            this._rightOne = new System.Windows.Forms.Button();
            this._centerPanel.SuspendLayout();
            this.SuspendLayout();
            //
            // _available
            //
            this._available.AllowDrop = true;
            this._available.Dock = System.Windows.Forms.DockStyle.Left;
            this._available.FormattingEnabled = true;
            this._available.IntegralHeight = false;
            this._available.ItemHeight = 16;
            this._available.Location = new System.Drawing.Point(0, 0);
            this._available.Name = "_available";
            this._available.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this._available.Size = new System.Drawing.Size(106, 183);
            this._available.TabIndex = 0;
            this._available.DoubleClick += new System.EventHandler(this._available_DoubleClick);
            this._available.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this._available_MouseDoubleClick);
            //
            // _selected
            //
            this._selected.AllowDrop = true;
            this._selected.Dock = System.Windows.Forms.DockStyle.Right;
            this._selected.FormattingEnabled = true;
            this._selected.IntegralHeight = false;
            this._selected.ItemHeight = 16;
            this._selected.Location = new System.Drawing.Point(165, 0);
            this._selected.Name = "_selected";
            this._selected.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this._selected.Size = new System.Drawing.Size(106, 183);
            this._selected.TabIndex = 2;
            this._selected.DoubleClick += new System.EventHandler(this._selected_DoubleClick);
            this._selected.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this._selected_MouseDoubleClick);
            //
            // _centerPanel
            //
            this._centerPanel.Controls.Add(this._leftAll);
            this._centerPanel.Controls.Add(this._leftOne);
            this._centerPanel.Controls.Add(this._rightAll);
            this._centerPanel.Controls.Add(this._rightOne);
            this._centerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._centerPanel.Location = new System.Drawing.Point(106, 0);
            this._centerPanel.Name = "_centerPanel";
            this._centerPanel.Size = new System.Drawing.Size(59, 183);
            this._centerPanel.TabIndex = 3;
            //
            // _leftAll
            //
            this._leftAll.Location = new System.Drawing.Point(15, 117);
            this._leftAll.Name = "_leftAll";
            this._leftAll.Size = new System.Drawing.Size(30, 25);
            this._leftAll.TabIndex = 3;
            this._leftAll.Text = "<<";
            //
            // _leftOne
            //
            this._leftOne.Location = new System.Drawing.Point(15, 95);
            this._leftOne.Name = "_leftOne";
            this._leftOne.Size = new System.Drawing.Size(30, 25);
            this._leftOne.TabIndex = 2;
            this._leftOne.Text = "<";
            //
            // _rightAll
            //
            this._rightAll.Location = new System.Drawing.Point(15, 57);
            this._rightAll.Name = "_rightAll";
            this._rightAll.Size = new System.Drawing.Size(30, 25);
            this._rightAll.TabIndex = 1;
            this._rightAll.Text = ">>";
            //
            // _rightOne
            //
            this._rightOne.Location = new System.Drawing.Point(15, 34);
            this._rightOne.Name = "_rightOne";
            this._rightOne.Size = new System.Drawing.Size(30, 25);
            this._rightOne.TabIndex = 0;
            this._rightOne.Text = ">";
            //
            // PickList
            //
            this.Controls.Add(this._centerPanel);
            this.Controls.Add(this._selected);
            this.Controls.Add(this._available);
            this.Name = "PickList";
            this.Size = new System.Drawing.Size(271, 183);
            this._centerPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

    } // class PickList

} // namespace AM.Windows.Forms
