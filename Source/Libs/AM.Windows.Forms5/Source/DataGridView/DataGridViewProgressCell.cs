// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* DataGridViewProgressCell.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Drawing;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms;

/// <summary>
///
/// </summary>
public class DataGridViewProgressCell
    : DataGridViewCell
{
    #region Properties

    /// <summary>
    /// Gets or sets the maximum.
    /// </summary>
    /// <value>The maximum.</value>
    public int Maximum { get; set; } = 100;

    /// <summary>
    /// Gets or sets the minimum.
    /// </summary>
    /// <value>The minimum.</value>
    public int Minimum { get; set; }

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

        using (Brush backBrush = new SolidBrush (backColor))
        {
            graphics.FillRectangle (backBrush, cellBounds);
        }

        if ((paintParts & DataGridViewPaintParts.Border)
            != DataGridViewPaintParts.None)
        {
            PaintBorder (graphics,
                clipBounds,
                cellBounds,
                cellStyle,
                advancedBorderStyle);
        }

        var r = cellBounds;
        r.Inflate (-2, -2);
        ProgressBarRenderer.DrawHorizontalBar (graphics, r);
        var percent = (((int)Value) - Minimum)
                      / ((float)(Maximum - Minimum));
        r.Width = (int)(r.Width * percent);
        r.Inflate (-2, -2);
        r.Width++;
        ProgressBarRenderer.DrawHorizontalChunks (graphics, r);
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

    /// <summary>
    /// Gets or sets the data type of the values in the cell.
    /// </summary>
    public override Type ValueType
    {
        get => base.ValueType ?? typeof (int);
        set => base.ValueType = value;
    }

    /// <inheritdoc cref="DataGridViewCell.DefaultNewRowValue"/>
    public override object DefaultNewRowValue => Minimum;

    /// <inheritdoc cref="DataGridViewCell.Clone"/>
    public override object Clone()
    {
        var result = base.Clone();
        if (result is DataGridViewProgressCell clone)
        {
            clone.Minimum = Minimum;
            clone.Maximum = Maximum;
        }

        return result;
    }

    #endregion
}
