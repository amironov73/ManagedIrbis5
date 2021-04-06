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

namespace AM.Windows.Forms
{
    /// <summary>
    ///
    /// </summary>
    public class DataGridViewProgressCell
        : DataGridViewCell
    {
        #region Properties

        private int _maximum = 100;

        /// <summary>
        /// Gets or sets the maximum.
        /// </summary>
        /// <value>The maximum.</value>
        public int Maximum
        {
            get
            {
                return _maximum;
            }
            set
            {
                _maximum = value;
            }
        }

        private int _minimum = 0;

        /// <summary>
        /// Gets or sets the minimum.
        /// </summary>
        /// <value>The minimum.</value>
        public int Minimum
        {
            get
            {
                return _minimum;
            }
            set
            {
                _minimum = value;
            }
        }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region DataGridViewCell members

        /// <summary>
        /// Paints the current <see cref="T:System.Windows.Forms.DataGridViewCell"></see>.
        /// </summary>
        /// <param name="graphics">The <see cref="T:System.Drawing.Graphics"></see> used to paint the <see cref="T:System.Windows.Forms.DataGridViewCell"></see>.</param>
        /// <param name="clipBounds">A <see cref="T:System.Drawing.Rectangle"></see> that represents the area of the <see cref="T:System.Windows.Forms.DataGridView"></see> that needs to be repainted.</param>
        /// <param name="cellBounds">A <see cref="T:System.Drawing.Rectangle"></see> that contains the bounds of the <see cref="T:System.Windows.Forms.DataGridViewCell"></see> that is being painted.</param>
        /// <param name="rowIndex">The row index of the cell that is being painted.</param>
        /// <param name="cellState">A bitwise combination of <see cref="T:System.Windows.Forms.DataGridViewElementStates"></see> values that specifies the state of the cell.</param>
        /// <param name="value">The data of the <see cref="T:System.Windows.Forms.DataGridViewCell"></see> that is being painted.</param>
        /// <param name="formattedValue">The formatted data of the <see cref="T:System.Windows.Forms.DataGridViewCell"></see> that is being painted.</param>
        /// <param name="errorText">An error message that is associated with the cell.</param>
        /// <param name="cellStyle">A <see cref="T:System.Windows.Forms.DataGridViewCellStyle"></see> that contains formatting and style information about the cell.</param>
        /// <param name="advancedBorderStyle">A <see cref="T:System.Windows.Forms.DataGridViewAdvancedBorderStyle"></see> that contains border styles for the cell that is being painted.</param>
        /// <param name="paintParts">A bitwise combination of the <see cref="T:System.Windows.Forms.DataGridViewPaintParts"></see> values that specifies which parts of the cell need to be painted.</param>
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
            using (Brush backBrush = new SolidBrush(backColor))
            {
                graphics.FillRectangle(backBrush, cellBounds);
            }
            if ((paintParts & DataGridViewPaintParts.Border)
                 != DataGridViewPaintParts.None)
            {
                PaintBorder(graphics,
                              clipBounds,
                              cellBounds,
                              cellStyle,
                              advancedBorderStyle);
            }
            var r = cellBounds;
            r.Inflate(-2, -2);
            ProgressBarRenderer.DrawHorizontalBar(graphics, r);
            var percent = (((int)Value) - Minimum)
                          / ((float)(Maximum - Minimum));
            r.Width = (int)(r.Width * percent);
            r.Inflate(-2, -2);
            r.Width++;
            ProgressBarRenderer.DrawHorizontalChunks(graphics, r);
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
            get
            {
                return base.ValueType ?? typeof(int);
            }
            set
            {
                base.ValueType = value;
            }
        }

        /// <summary>
        /// Gets the default value for a cell in the row for new records.
        /// </summary>
        public override object DefaultNewRowValue
        {
            get
            {
                return Minimum;
            }
        }

        /// <summary>
        /// Creates an exact copy of this cell.
        /// </summary>
        public override object Clone()
        {
            var result = base.Clone();
            var clone
                = result as DataGridViewProgressCell;
            if (clone != null)
            {
                clone.Minimum = Minimum;
                clone.Maximum = Maximum;
            }
            return result;
        }

        #endregion
    }
}
