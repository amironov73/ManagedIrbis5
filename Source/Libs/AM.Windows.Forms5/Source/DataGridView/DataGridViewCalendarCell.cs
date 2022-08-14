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

namespace AM.Windows.Forms;

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

    /// <inheritdoc cref="DataGridViewTextBoxCell.InitializeEditingControl"/>
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

    ///<inheritdoc cref="DataGridViewCell.EditType"/>
    public override Type EditType => typeof (DataGridViewCalendarEditingControl);

    /// <inheritdoc cref="DataGridViewTextBoxCell.ValueType"/>
    public override Type ValueType => typeof (DateTime);

    /// <inheritdoc cref="DataGridViewCell.DefaultNewRowValue"/>
    public override object DefaultNewRowValue => DateTime.Now;

    /// <inheritdoc cref="DataGridViewTextBoxCell.Clone"/>
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
