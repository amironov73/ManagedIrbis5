// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* DataGridViewRatingCell.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Drawing;
using System.Windows.Forms;

using RC = AM.Windows.Forms.Properties.Resources;

#endregion

#nullable enable

namespace AM.Windows.Forms;

/// <summary>
///
/// </summary>
public class DataGridViewRatingCell
    : DataGridViewCell
{
    #region Properties

    /// <summary>
    /// Gets or sets the maximum.
    /// </summary>
    public int Maximum { get; set; } = 5;

    #endregion

    #region Public methods

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

        if (value is float)
        {
            var current = (float)value;
            var curInt = (int)current;
            var curFloat = current - curInt;
            for (var i = 0; i < Maximum; i++)
            {
                Image image = RC.Star0;
                if (curInt > i)
                {
                    image = RC.Star2;
                }
                else if (curInt == i)
                {
                    if (curFloat >= 0.49f)
                    {
                        image = RC.Star1;
                    }
                }

                graphics.DrawImageUnscaled
                    (
                        image,
                        cellBounds.Left + 2 + image.Width * i,
                        cellBounds.Top + (cellBounds.Height - image.Height) / 2
                    );
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
    }

    /// <inheritdoc cref="DataGridViewCell.FormattedValueType"/>
    public override Type FormattedValueType => typeof (float);

    /// <inheritdoc cref="DataGridViewCell.ValueType"/>
    public override Type ValueType
    {
        get => typeof (float);
        set => base.ValueType = value;
    }

    /// <inheritdoc cref="DataGridViewCell.DefaultNewRowValue"/>
    public override object DefaultNewRowValue => 0f;

    #endregion
}
