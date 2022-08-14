// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* PagingDataGridView.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms;

/// <summary>
///
/// </summary>
[System.ComponentModel.DesignerCategory ("Code")]
public class PagingDataGridView
    : DataGridView
{
    #region Events

    /// <summary>
    /// Raised on paging.
    /// </summary>
    public event EventHandler<PagingDataGridViewEventArgs>? Paging;

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public PagingDataGridView()
    {
        ReadOnly = true;
        AutoGenerateColumns = false;
        RowHeadersVisible = false;
        SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        AllowUserToAddRows = false;
        AllowUserToResizeRows = false;
        AllowUserToDeleteRows = false;
        EnableHeadersVisualStyles = false;
        ColumnHeadersDefaultCellStyle.SelectionBackColor = ColumnHeadersDefaultCellStyle.BackColor;
        ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        ScrollBars = ScrollBars.None;

        var mainStyle = new DataGridViewCellStyle();
        DefaultCellStyle = mainStyle;

        var alternateStyle = new DataGridViewCellStyle (mainStyle)
        {
            BackColor = Color.LightGray
        };
        AlternatingRowsDefaultCellStyle = alternateStyle;
    }

    #endregion

    #region Private members

    private void _HandleScrolling (bool down)
    {
        var row = CurrentRow;
        if (row == null)
        {
            return;
        }

        var rowIndex = row.Index;
        var rowCount = RowCount;

        if (down)
        {
            if (rowIndex >= rowCount - 1)
            {
                PerformPaging (true, false);
            }
        }
        else
        {
            if (rowIndex == 0)
            {
                PerformPaging (false, false);
            }
        }
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Fill the grid.
    /// </summary>
    public void FillGrid
        (
            IEnumerable<object> objects
        )
    {
        DataSource = objects.ToArray();
    }

    /// <summary>
    /// Perform paging.
    /// </summary>
    public void PerformPaging
        (
            bool scrollDown,
            bool initialCall
        )
    {
        var eventArgs = new PagingDataGridViewEventArgs
        {
            InitialCall = initialCall,
            ScrollDown = scrollDown,
            CurrentRow = CurrentRow
        };

        Paging.Raise (this, eventArgs);

        if (!eventArgs.Success)
        {
            return;
        }

        if (!scrollDown && RowCount > 0)
        {
            CurrentCell = Rows[RowCount - 1].Cells[0];
        }
    }

    #endregion

    #region DataGridView members

    /// <inheritdoc cref="DataGridView.OnKeyDown" />
    protected override void OnKeyDown
        (
            KeyEventArgs e
        )
    {
        base.OnKeyDown (e);

        var down = false;

        switch (e.KeyCode)
        {
            case Keys.Down:
            case Keys.PageDown:
                down = true;
                break;

            case Keys.Up:
            case Keys.PageUp:
                break;

            default:
                return;
        }

        e.Handled = true;

        _HandleScrolling (down);
    }

    /// <inheritdoc cref="DataGridView.OnMouseWheel" />
    protected override void OnMouseWheel
        (
            MouseEventArgs e
        )
    {
        base.OnMouseWheel (e);

        _HandleScrolling (e.Delta < 0);
    }

    #endregion
}
