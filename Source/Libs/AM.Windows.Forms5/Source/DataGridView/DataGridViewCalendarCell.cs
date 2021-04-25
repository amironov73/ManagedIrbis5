// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* DataGridViewCalendarCell.cs -- DateTimePicker hosted in DataGridViewCell
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms
{
    /// <summary>
    /// <see cref="DateTimePicker"/> hosted in
    /// <see cref="DataGridViewCell"/>.
    /// </summary>
    /// <remarks>Stolen from MSDN article
    /// "How to: Host Controls in Windows Forms DataGridView Cells"
    /// </remarks>
    public class DataGridViewCalendarCell
        : DataGridViewTextBoxCell
    {
        #region Properties

        /// <summary>
        /// Gets or sets the format of the date.
        /// </summary>
        /// <value>Format of the date.</value>
        public string Format
        {
            get => Style.Format;
            set => Style.Format = value;
        }

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="DataGridViewCalendarCell"/> class.
        /// </summary>
        public DataGridViewCalendarCell()
        {
            // Use the short date format.
            Style.Format = "d";
        }

        #endregion

        #region DataGridViewCell members

        /// <summary>
        /// Initializes the control used to edit the cell.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the
        /// cell's location.</param>
        /// <param name="initialFormattedValue">An
        /// <see cref="T:System.Object"/> that represents the value
        /// displayed by the cell when editing is started.</param>
        /// <param name="dataGridViewCellStyle">A
        /// <see cref="T:System.Windows.Forms.DataGridViewCellStyle"/>
        /// that represents the style of the cell.</param>
        /// <exception cref="T:System.InvalidOperationException">
        /// There is no associated
        /// <see cref="T:System.Windows.Forms.DataGridView"/>
        /// or if one is present, it does not have an associated editing control.
        /// </exception>
        public override void InitializeEditingControl
            (
                int rowIndex,
                object initialFormattedValue,
                DataGridViewCellStyle dataGridViewCellStyle
            )
        {
            // Set the value of the editing control to the current cell value.
            base.InitializeEditingControl
                (
                    rowIndex,
                    initialFormattedValue,
                    dataGridViewCellStyle
                );

            if (DataGridView?.EditingControl is DataGridViewCalendarEditingControl control)
            {
                control.Value = (DateTime)Value;
            }
        }

        /// <summary>
        /// Gets the type of the cell's hosted editing control.
        /// </summary>
        /// <value></value>
        /// <returns>A <see cref="T:System.Type"/>
        /// representing the <see cref="DataGridViewCalendarEditingControl"/>
        /// type.</returns>
        public override Type EditType => typeof (DataGridViewCalendarEditingControl);

        /// <summary>
        /// Gets or sets the data type of the values in the cell.
        /// </summary>
        /// <value></value>
        /// <returns>A <see cref="T:System.Type"/>
        /// representing the data type of the value in the cell.</returns>
        public override Type ValueType => typeof (DateTime);

        /// <summary>
        /// Gets the default value for a cell in the row for new records.
        /// </summary>
        /// <value></value>
        /// <returns>An <see cref="T:System.Object"/> representing
        /// the default value.</returns>
        public override object DefaultNewRowValue => DateTime.Now;

        /// <summary>
        /// Creates an exact copy of this cell.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Object"/> that represents the cloned
        /// <see cref="DataGridViewTextBoxCell"></see>.
        /// </returns>
        public override object Clone()
        {
            var result = base.Clone();
            if (result is DataGridViewCalendarCell clone)
            {
                clone.Format = Format;
            }

            return result;
        }

        #endregion
    }
}
