// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable CoVariantArrayConversion
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* EnumComboBox.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Windows.Forms;

using AM.Reflection;

#endregion

#nullable enable

namespace AM.Windows.Forms
{
    /// <summary>
    ///
    /// </summary>
    // ReSharper disable RedundantNameQualifier
    [System.ComponentModel.DesignerCategory("Code")]
    // ReSharper restore RedundantNameQualifier
    public class EnumComboBox
        : ComboBox
    {
        #region Properties

        private Type? _enumType;

        /// <summary>
        /// Gets or sets the type of the enum.
        /// </summary>
        /// <value>The type of the enum.</value>
        [DefaultValue(null)]
        [TypeConverter(typeof(EnumTypeConverter))]
        public Type? EnumType
        {
            get => _enumType;
            set => _SetEnumType(value);
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public int? Value
        {
            get
            {
                int? result = null;
                var member = (EnumMemberInfo?)SelectedItem;
                if (member != null)
                {
                    result = member.Value;
                }
                return result;
            }
            set
            {
                if (value == null)
                {
                    SelectedItem = null;
                }
                else
                {
                    foreach (EnumMemberInfo info in Items)
                    {
                        if (info.Value == value)
                        {
                            SelectedItem = info;
                            break;
                        }
                    }
                }
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Default constructor.
        /// </summary>
        public EnumComboBox()
        {
            _SetupControl();
        }

        #endregion

        #region Private members

        private void _SetupControl()
        {
            DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void _SetEnumType
            (
                Type? enumType
            )
        {
            _enumType = enumType;
            var members = enumType is null
                ? Array.Empty<EnumMemberInfo>()
                : EnumMemberInfo.Parse(enumType);
            Items.Clear();
            Items.AddRange(members);
        }

        #endregion
    }
}
