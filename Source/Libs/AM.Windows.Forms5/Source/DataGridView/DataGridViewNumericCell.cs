// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* DataGridViewNumericCell.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms;

/// <summary>
///
/// </summary>
public class DataGridViewNumericCell
    : DataGridViewTextBoxCell
{
    #region Properties

    /// <summary>
    /// Gets or sets number of the decimal points.
    /// </summary>
    [DefaultValue (0)]
    public int DecimalPoints { get; set; }

    /// <summary>
    /// Gets or sets the default value.
    /// </summary>
    public decimal DefaultValue { get; set; }

    /// <summary>
    /// Gets or sets the maximum.
    /// </summary>
    public decimal Maximum { get; set; } = decimal.MaxValue;

    //[DefaultValue ( decimal.MinValue )]
    /// <summary>
    /// Gets or sets the minimum.
    /// </summary>
    /// <value>The minimum.</value>
    public decimal Minimum { get; set; } = decimal.MinValue;

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
        base.InitializeEditingControl
            (
                rowIndex,
                initialFormattedValue,
                dataGridViewCellStyle
            );

        if (DataGridView?.EditingControl is DataGridViewNumericEditingControl control)
        {
            control.BorderStyle = BorderStyle.None;

            // control.AutoSize = false;
            // control.Size = Size;
            control.Minimum = Minimum;
            control.Maximum = Maximum;
            control.Value = (decimal)Value;
        }
    }

    /// <inheritdoc cref="DataGridViewCell.EditType"/>
    public override Type EditType => typeof (DataGridViewNumericEditingControl);

    /// <inheritdoc cref="DataGridViewTextBoxCell.ValueType"/>
    public override Type ValueType => base.ValueType ?? typeof (decimal);

    ///<inheritdoc cref="DataGridViewCell.DefaultNewRowValue"/>
    public override object DefaultNewRowValue => DefaultValue;

    ///<inheritdoc cref="DataGridViewCell.GetFormattedValue(object,int,ref System.Windows.Forms.DataGridViewCellStyle,System.ComponentModel.TypeConverter,System.ComponentModel.TypeConverter,System.Windows.Forms.DataGridViewDataErrorContexts)"/>
    protected override object? GetFormattedValue
        (
            object value,
            int rowIndex,
            ref DataGridViewCellStyle cellStyle,
            TypeConverter valueTypeConverter,
            TypeConverter formattedValueTypeConverter,
            DataGridViewDataErrorContexts context
        )
    {
        if (value is decimal)
        {
            return value.ToString();
        }

        return base.GetFormattedValue
            (
                value,
                rowIndex,
                ref cellStyle,
                valueTypeConverter,
                formattedValueTypeConverter,
                context
            );
    }

    /// <inheritdoc cref="DataGridViewCell.ParseFormattedValue"/>
    public override object ParseFormattedValue
        (
            object formattedValue,
            DataGridViewCellStyle cellStyle,
            TypeConverter formattedValueTypeConverter,
            TypeConverter valueTypeConverter
        )
    {
        if (formattedValue is string value)
        {
            return decimal.Parse (value);
        }

        return base.ParseFormattedValue
            (
                formattedValue,
                cellStyle,
                formattedValueTypeConverter,
                valueTypeConverter
            );
    }

    /// <inheritdoc cref="DataGridViewTextBoxCell.Clone"/>
    public override object Clone()
    {
        var result = (DataGridViewCell) base.Clone();
        if (result is DataGridViewNumericCell clone)
        {
            clone.DecimalPoints = DecimalPoints;
            clone.Maximum = Maximum;
            clone.Minimum = Minimum;
        }

        return result;
    }

    #endregion
}
