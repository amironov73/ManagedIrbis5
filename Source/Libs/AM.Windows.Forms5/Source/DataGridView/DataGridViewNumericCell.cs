// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
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

namespace AM.Windows.Forms
{
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
        [DefaultValue(0)]
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

        ///<summary>
        ///Initializes the control used to edit the cell.
        ///</summary>
        ///
        ///<param name="initialFormattedValue">An <see cref="T:System.Object"></see> that represents the value displayed by the cell when editing is started.</param>
        ///<param name="rowIndex">The zero-based row index of the cell's location.</param>
        ///<param name="dataGridViewCellStyle">A <see cref="T:System.Windows.Forms.DataGridViewCellStyle"></see> that represents the style of the cell.</param>
        ///<exception cref="T:System.InvalidOperationException">There is no associated <see cref="T:System.Windows.Forms.DataGridView"></see> or if one is present, it does not have an associated editing control. </exception><filterpriority>1</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" /><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /></PermissionSet>
        public override void InitializeEditingControl
            (
                int rowIndex,
                object initialFormattedValue,
                DataGridViewCellStyle dataGridViewCellStyle
            )
        {
            base.InitializeEditingControl(rowIndex,
                                            initialFormattedValue,
                                            dataGridViewCellStyle);
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

        ///<summary>
        ///Gets the type of the cell's hosted editing control.
        ///</summary>
        ///
        ///<returns>
        ///A <see cref="T:System.Type"></see> representing the <see cref="T:System.Windows.Forms.DataGridViewTextBoxEditingControl"></see> type.
        ///</returns>
        ///<filterpriority>1</filterpriority>
        public override Type EditType => typeof(DataGridViewNumericEditingControl);

        ///<summary>
        ///Gets or sets the data type of the values in the cell.
        ///</summary>
        ///
        ///<returns>
        ///A <see cref="T:System.Type"></see> representing the data type of the value in the cell.
        ///</returns>
        ///<filterpriority>1</filterpriority>
        public override Type ValueType => base.ValueType ?? typeof(decimal);

        ///<summary>
        ///Gets the default value for a cell in the row for new records.
        ///</summary>
        ///
        ///<returns>
        ///An <see cref="T:System.Object"></see> representing the default value.
        ///</returns>
        ///<filterpriority>1</filterpriority>
        public override object DefaultNewRowValue => DefaultValue;

        ///<summary>
        ///Gets the value of the cell as formatted for display.
        ///</summary>
        ///
        ///<returns>
        ///The formatted value of the cell or null if the cell does not belong to a <see cref="T:System.Windows.Forms.DataGridView"></see> control.
        ///</returns>
        ///
        ///<param name="cellStyle">The <see cref="T:System.Windows.Forms.DataGridViewCellStyle"></see> in effect for the cell.</param>
        ///<param name="context">A bitwise combination of <see cref="T:System.Windows.Forms.DataGridViewDataErrorContexts"></see> values describing the context in which the formatted value is needed.</param>
        ///<param name="valueTypeConverter">A <see cref="T:System.ComponentModel.TypeConverter"></see> associated with the value type that provides custom conversion to the formatted value type, or null if no such custom conversion is needed.</param>
        ///<param name="rowIndex">The index of the cell's parent row. </param>
        ///<param name="value">The value to be formatted. </param>
        ///<param name="formattedValueTypeConverter">A <see cref="T:System.ComponentModel.TypeConverter"></see> associated with the formatted value type that provides custom conversion from the value type, or null if no such custom conversion is needed.</param>
        ///<exception cref="T:System.Exception">Formatting failed and either there is no handler for the <see cref="E:System.Windows.Forms.DataGridView.DataError"></see> event of the <see cref="T:System.Windows.Forms.DataGridView"></see> control or the handler set the <see cref="P:System.Windows.Forms.DataGridViewDataErrorEventArgs.ThrowException"></see> property to true. The exception object can typically be cast to type <see cref="T:System.FormatException"></see>.</exception>
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

            return base.GetFormattedValue(value,
                                         rowIndex,
                                         ref cellStyle,
                                         valueTypeConverter,
                                         formattedValueTypeConverter,
                                         context);
        }

        ///<summary>
        ///Converts a value formatted for display to an actual cell value.
        ///</summary>
        ///
        ///<returns>
        ///The cell value.
        ///</returns>
        ///
        ///<param name="formattedValue">The display value of the cell.</param>
        ///<param name="formattedValueTypeConverter">A <see cref="T:System.ComponentModel.TypeConverter"></see> for the display value type, or null to use the default converter.</param>
        ///<param name="valueTypeConverter">A <see cref="T:System.ComponentModel.TypeConverter"></see> for the cell value type, or null to use the default converter.</param>
        ///<param name="cellStyle">The <see cref="T:System.Windows.Forms.DataGridViewCellStyle"></see> in effect for the cell.</param>
        ///<exception cref="T:System.FormatException">The <see cref="P:System.Windows.Forms.DataGridViewCell.FormattedValueType"></see> property value is null.-or-The <see cref="P:System.Windows.Forms.DataGridViewCell.ValueType"></see> property value is null.-or-formattedValue cannot be converted.</exception>
        ///<exception cref="T:System.ArgumentNullException">cellStyle is null.</exception>
        ///<exception cref="T:System.ArgumentException">formattedValue is null.-or-The type of formattedValue does not match the type indicated by the <see cref="P:System.Windows.Forms.DataGridViewCell.FormattedValueType"></see> property. </exception>
        public override object ParseFormattedValue
            (
                object formattedValue,
                DataGridViewCellStyle cellStyle,
                TypeConverter formattedValueTypeConverter,
                TypeConverter valueTypeConverter
            )
        {
            if (formattedValue is string)
            {
                return decimal.Parse((string)formattedValue);
            }

            return base.ParseFormattedValue(formattedValue,
                                           cellStyle,
                                           formattedValueTypeConverter,
                                           valueTypeConverter);
        }

        /// <summary>
        /// Creates an exact copy of this cell.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Object"></see> that represents the cloned <see cref="T:System.Windows.Forms.DataGridViewTextBoxCell"></see>.
        /// </returns>
        public override object Clone()
        {
            var result = (DataGridViewCell)base.Clone();
            var clone = result as DataGridViewNumericCell;
            if (clone != null)
            {
                clone.DecimalPoints = DecimalPoints;
                clone.Maximum = Maximum;
                clone.Minimum = Minimum;
            }
            return result;
        }

        #endregion
    }
}
