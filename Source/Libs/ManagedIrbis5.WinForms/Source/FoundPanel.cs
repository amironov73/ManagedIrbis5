// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* FoundPanel.cs -- список найденных документов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Windows.Forms;
using AM;

#endregion

#nullable enable

namespace ManagedIrbis.WinForms
{
    /// <summary>
    /// Список найденных документов.
    /// </summary>
    public partial class FoundPanel
        : UserControl
    {
        #region Events

        /// <summary>
        /// Событие, возникающее, когда элемент выбран.
        /// </summary>
        public event EventHandler? ItemSelected;

        #endregion

        #region Properties

        /// <summary>
        /// Адаптер, подтягивающий записи с сервера.
        /// </summary>
        public RecordAdapter? Adapter
        {
            get => (RecordAdapter?) _grid.Adapter;
            set => _grid.Adapter = value;
        }

        /// <summary>
        /// Текущий MFN.
        /// </summary>
        public int CurrentMfn
        {
            get
            {
                var cell = _grid.CurrentRow?.Cells[0];
                if (cell is null)
                {
                    return 0;
                }

                if (cell.Value is int mfn)
                {
                    return mfn;
                }

                return 0;
            }
        }

        /// <summary>
        /// Текущие строки.
        /// </summary>
        public FoundLine?[]? Records => (FoundLine?[]?) _grid.Lines;

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public FoundPanel()
        {
            InitializeComponent();

            _SetupEvents();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public FoundPanel
            (
                RecordAdapter adapter
            )
        {
            InitializeComponent();

            Adapter = adapter;
            _SetupEvents();
        }

        #endregion

        #region Private members

        private void _SetupEvents()
        {
            //_grid.KeyDown += _grid_KeyDown;
            //_grid.KeyPress += _grid_KeyPress;
            _grid.CellClick += _grid_CellClick;
            //_grid.DoubleClick += _grid_DoubleClick;
            //_grid.MouseWheel += _grid_MouseWheel;
            //_keyBox.KeyDown += _keyBox_KeyDown;
            //_keyBox.DelayedTextChanged += _keyBox_TextChanged;
            //_keyBox.EnterPressed += _keyBox_TextChanged;
            //_keyBox.MouseWheel += _grid_MouseWheel;
            //_scrollControl.Scroll += _scrollControl_Scroll;
            //_scrollControl.MouseWheel += _grid_MouseWheel;
        }

        private void _adapter_CurrentItemChanged
            (
                object? sender,
                EventArgs e
            )
        {
            _RaiseItemSelected();
        }

        private void _grid_CellClick
            (
                object? sender,
                DataGridViewCellEventArgs e
            )
        {
            _RaiseItemSelected();
        }

        // private void _grid_MouseWheel
        //     (
        //         object? sender,
        //         MouseEventArgs e
        //     )
        // {
        //     if (ReferenceEquals(Adapter, null))
        //     {
        //         return;
        //     }
        //
        //     int delta = e.Delta;
        //
        //     if (delta > 0)
        //     {
        //         Adapter.MovePrevious();
        //     }
        //     else if (delta < 0)
        //     {
        //         Adapter.MoveNext();
        //     }
        // }

        private int _VisibleRowCount()
        {
            return _grid.DisplayedRowCount(true);
        }

        // private void _grid_KeyDown
        //     (
        //         object? sender,
        //         KeyEventArgs e
        //     )
        // {
        //     if (ReferenceEquals(Adapter, null))
        //     {
        //         return;
        //     }
        //
        //     switch (e.KeyData)
        //     {
        //         case Keys.Down:
        //             Adapter.MoveNext();
        //             e.Handled = true;
        //             break;
        //
        //         case Keys.Up:
        //             Adapter.MovePrevious();
        //             e.Handled = true;
        //             break;
        //
        //         case Keys.PageDown:
        //             Adapter.MoveNext(_VisibleRowCount());
        //             e.Handled = true;
        //             break;
        //
        //         case Keys.PageUp:
        //             Adapter.MoveNext(_VisibleRowCount());
        //             e.Handled = true;
        //             break;
        //
        //         case Keys.Enter:
        //             _RaiseItemSelected();
        //             break;
        //     }
        // }

        // private void _keyBox_KeyDown
        //     (
        //         object? sender,
        //         KeyEventArgs e
        //     )
        // {
        //     if (ReferenceEquals(Adapter, null))
        //     {
        //         return;
        //     }
        //
        //     switch (e.KeyData)
        //     {
        //         case Keys.Down:
        //             Adapter.MoveNext();
        //             e.Handled = true;
        //             break;
        //
        //         case Keys.Up:
        //             Adapter.MovePrevious();
        //             e.Handled = true;
        //             break;
        //
        //         case Keys.PageDown:
        //             Adapter.MoveNext(_VisibleRowCount());
        //             e.Handled = true;
        //             break;
        //
        //         case Keys.PageUp:
        //             Adapter.MoveNext(_VisibleRowCount());
        //             e.Handled = true;
        //             break;
        //
        //         case Keys.Enter:
        //             _RaiseItemSelected();
        //             break;
        //     }
        // }

        private void _RaiseItemSelected()
        {
            ItemSelected.Raise(this);
        }

        private void _grid_DoubleClick
            (
                object? sender,
                EventArgs e
            )
        {
            _RaiseItemSelected();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Очистка.
        /// </summary>
        public void Clear()
        {
            _grid.Adapter?.Clear();
            _grid.Clear();
            _grid.Invalidate();
        }

        /// <summary>
        /// Заполнение.
        /// </summary>
        public void Fill()
        {
            var adapter = Adapter;
            if (adapter is null)
            {
                Clear();
                return;
            }

            _grid.InitialFill();

        } // method Fill

        /// <summary>
        /// Заполнение найденными записями.
        /// </summary>
        public void Fill
            (
                int[] found
            )
        {
            if (Adapter is not null)
            {
                Adapter.Fill(found);
                _grid.Invalidate();
            }
            else
            {
                Clear();
            }

        } // method Fill

        /// <summary>
        /// Пересоздание грида (паллиативное решение).
        /// </summary>
        public void RecreateGrid()
        {
            SuspendLayout();

            if (_mfnColumn is not null)
            {
                if (_grid is not null)
                {
                    _grid.Columns.Remove(_mfnColumn);
                }

                _mfnColumn.Dispose();
                _mfnColumn = null;
            }

            if (_selectionColumn is not null)
            {
                if (_grid is not null)
                {
                    _grid.Columns.Remove(_selectionColumn);
                }

                _selectionColumn.Dispose();
                _selectionColumn = null;
            }

            if (_iconColumn is not null)
            {
                if (_grid is not null)
                {
                    _grid.Columns.Remove(_iconColumn);
                }

                _iconColumn.Dispose();
                _iconColumn = null;
            }

            if (_descriptionColumn is not null)
            {
                if (_grid is not null)
                {
                    _grid.Columns.Remove(_descriptionColumn);
                }

                _descriptionColumn.Dispose();
                _descriptionColumn = null;
            }

            if (_grid is not null)
            {
                Controls.Remove(_grid);
                _grid.Dispose();
            }

            _grid = new AM.Windows.Forms.VirtualGrid();
            _mfnColumn = new DataGridViewTextBoxColumn();
            _selectionColumn = new DataGridViewCheckBoxColumn();
            _iconColumn = new DataGridViewImageColumn();
            _descriptionColumn = new DataGridViewTextBoxColumn();

            _grid.AllowUserToAddRows = false;
            _grid.AllowUserToDeleteRows = false;
            _grid.AllowUserToResizeRows = false;
            _grid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
            _grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            _grid.Columns.AddRange(new DataGridViewColumn[] {
                _mfnColumn,
                _selectionColumn,
                _iconColumn,
                _descriptionColumn});
            _grid.Dock = DockStyle.Fill;
            _grid.Location = new System.Drawing.Point(0, 23);
            _grid.Margin = new Padding(2);
            _grid.Name = "_grid";
            _grid.ReadOnly = true;
            _grid.RowHeadersVisible = false;
            _grid.RowTemplate.Height = 24;
            _grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            _grid.Size = new System.Drawing.Size(438, 164);
            _grid.TabIndex = 1;

            _mfnColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            _mfnColumn.HeaderText = "MFN";
            _mfnColumn.MinimumWidth = 70;
            _mfnColumn.Name = "_mfnColumn";
            _mfnColumn.ReadOnly = true;
            _mfnColumn.Width = 70;

            _selectionColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            _selectionColumn.HeaderText = "";
            _selectionColumn.MinimumWidth = 30;
            _selectionColumn.Name = "_selectionColumn";
            _selectionColumn.ReadOnly = true;
            _selectionColumn.Resizable = DataGridViewTriState.False;
            _selectionColumn.SortMode = DataGridViewColumnSortMode.Automatic;
            _selectionColumn.Width = 30;

            _iconColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            _iconColumn.HeaderText = "";
            _iconColumn.MinimumWidth = 30;
            _iconColumn.Name = "_iconColumn";
            _iconColumn.ReadOnly = true;
            _iconColumn.Width = 30;

            _descriptionColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            _descriptionColumn.HeaderText = "Description";
            _descriptionColumn.Name = "_descriptionColumn";
            _descriptionColumn.ReadOnly = true;
            _descriptionColumn.Resizable = DataGridViewTriState.True;
            _descriptionColumn.SortMode = DataGridViewColumnSortMode.NotSortable;

            Controls.Remove(_topPanel);
            Controls.Add(_grid);
            Controls.Add(_topPanel);

            ResumeLayout(false);

            _grid.Adapter = Adapter;
        }

        #endregion

    } // class FoundPanel

} // namespace ManagedIrbis.WinForms
