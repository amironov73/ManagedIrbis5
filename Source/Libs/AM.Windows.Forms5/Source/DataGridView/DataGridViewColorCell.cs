// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

/* DataGridViewColorCell.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

#endregion

#nullable enable

namespace AM.Windows.Forms;

/// <summary>
///
/// </summary>
public class DataGridViewColorCell
    : DataGridViewCell,
    IDataGridViewEditingCell
{
    #region Public methods

    /// <summary>
    /// Shows the editor.
    /// </summary>
    public void ShowEditor()
    {
        using var dialog = new ColorDialog { Color = (Color)Value };
        if (dialog.ShowDialog (DataGridView?.FindForm()) == DialogResult.OK)
        {
            EditingCellValueChanged = true;
            DataGridView?.NotifyCurrentCellDirty (true);
            SetValue (RowIndex, dialog.Color);
        }
    }

    #endregion

    #region DataGridViewCell members

    /// <inheritdoc cref="DataGridViewCell.Paint"/>
    protected override void Paint
        (
            Graphics graphics,
            Rectangle clipBounds,
            Rectangle cellBounds,
            int rowIndex,
            DataGridViewElementStates cellState,
            object value,
            object formattedValue,
            string errorText,
            DataGridViewCellStyle cellStyle,
            DataGridViewAdvancedBorderStyle advancedBorderStyle,
            DataGridViewPaintParts paintParts
        )
    {
        var backColor = cellStyle.BackColor;
        if ((cellState & DataGridViewElementStates.Selected)
            != DataGridViewElementStates.None)
        {
            backColor = cellStyle.SelectionBackColor;
        }

        using (var brush = new SolidBrush (backColor))
        {
            graphics.FillRectangle (brush, cellBounds);
        }

        if ((paintParts & DataGridViewPaintParts.Border)
            != DataGridViewPaintParts.None)
        {
            PaintBorder
                (
                    graphics,
                    clipBounds,
                    cellBounds,
                    cellStyle,
                    advancedBorderStyle
                );
        }

        var colorRectangle = cellBounds;
        colorRectangle.Inflate (-2, -2);
        if ((cellState & DataGridViewElementStates.ReadOnly)
            == DataGridViewElementStates.None)
        {
            var buttonRectangle = new Rectangle
                (
                    colorRectangle.Left + colorRectangle.Width - colorRectangle.Height,
                    colorRectangle.Top,
                    colorRectangle.Height,
                    colorRectangle.Height
                );
            colorRectangle.Width = buttonRectangle.Left - 2 - colorRectangle.Left;
            var state = ComboBoxState.Normal;
            if ((cellState & DataGridViewElementStates.Selected)
                != DataGridViewElementStates.None)
            {
                state = ComboBoxState.Hot;
            }

            ComboBoxRenderer.DrawDropDownButton
                (
                    graphics,
                    buttonRectangle,
                    state
                );
        }

        if (Value is Color)
        {
            var color = (Color)Value;
            using var brush = new SolidBrush (color);
            graphics.FillRectangle (brush, colorRectangle);
        }

        base.Paint
            (
                graphics,
                clipBounds,
                cellBounds,
                rowIndex,
                cellState,
                value,
                formattedValue,
                errorText,
                cellStyle,
                advancedBorderStyle,
                paintParts
            );
    }

    /// <inheritdoc cref="DataGridViewCell.DefaultNewRowValue"/>
    public override object DefaultNewRowValue => Color.Black;

    /// <inheritdoc cref="DataGridViewCell.ValueType"/>
    public override Type ValueType
    {
        get => base.ValueType ?? typeof (Color);
        set => base.ValueType = value;
    }

    #region IDataGridViewEditingCell members

    ///<summary>
    /// Retrieves the formatted value of the cell.
    ///</summary>
    ///
    ///<returns>
    ///An <see cref="T:System.Object"/> that represents the formatted version of the cell contents.
    ///</returns>
    ///
    ///<param name="context">A bitwise combination of <see cref="T:System.Windows.Forms.DataGridViewDataErrorContexts"/> values that specifies the context in which the data is needed.</param>
    public object GetEditingCellFormattedValue
        (
            DataGridViewDataErrorContexts context
        )
    {
        return Value;
    }

    ///<summary>
    ///Prepares the currently selected cell for editing
    ///</summary>
    ///
    ///<param name="selectAll">true to select the cell contents; otherwise, false.</param>
    public void PrepareEditingCellForEdit (bool selectAll)
    {
        // throw new NotImplementedException ();
    }

    ///<summary>
    ///Gets or sets the formatted value of the cell.
    ///</summary>
    ///
    ///<returns>
    ///An <see cref="T:System.Object"/> that contains the cell's value.
    ///</returns>
    ///
    public object EditingCellFormattedValue
    {
        get => Value;
        set => Value = value;
    }

    ///<summary>
    /// Gets or sets a value indicating whether the value of the cell has changed.
    ///</summary>
    ///
    ///<returns>
    /// true if the value of the cell has changed; otherwise, false.
    ///</returns>
    ///
    public bool EditingCellValueChanged { get; set; }

    #endregion

    /// <inheritdoc cref="DataGridViewCell.OnClick"/>
    protected override void OnClick
        (
            DataGridViewCellEventArgs e
        )
    {
        base.OnClick (e);
        if (!ReadOnly)
        {
            ShowEditor();
        }
    }

    #endregion
}
