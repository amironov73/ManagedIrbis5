// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* DataGridViewCalendarColumn.cs -- column of DataGridViewCalendarCell's
   Ars Magna project, http://library.istu.edu/am */

#region Using directives

using System;
using System.Diagnostics;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms;

/// <summary>
/// Column of <see cref="DataGridViewCalendarCell"/>'s.
/// </summary>
/// <remarks>Stolen from MSDN article
/// "How to: Host Controls in Windows Forms DataGridView Cells"
/// </remarks>
public class DataGridViewCalendarColumn
    : DataGridViewColumn
{
    #region Properties

    /// <summary>
    /// Gets or sets the format of date.
    /// </summary>
    /// <value>Format of date.</value>
    public string Format
    {
        [DebuggerStepThrough] get => ((DataGridViewCalendarCell)CellTemplate).Format;

        [DebuggerStepThrough] set => ((DataGridViewCalendarCell)CellTemplate).Format = value;
    }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию
    /// </summary>
    public DataGridViewCalendarColumn()
        : base (new DataGridViewCalendarCell())
    {
        // пустое тело конструктора
    }

    #endregion

    #region DataGridViewColumn members

    /// <inheritdoc cref="DataGridViewColumn.CellTemplate"/>
    public override DataGridViewCell CellTemplate
    {
        get => base.CellTemplate;
        set
        {
            // Ensure that the cell used for the template is a CalendarCell.
            if (!(value is DataGridViewCalendarCell))
            {
                throw new InvalidCastException ("Must be a CalendarCell");
            }

            base.CellTemplate = value;
        }
    }

    /// <inheritdoc cref="DataGridViewColumn.Clone"/>
    public override object Clone()
    {
        var result = base.Clone();
        if (result is DataGridViewCalendarColumn clone)
        {
            clone.Format = Format;
        }

        return result;
    }

    #endregion
}
