// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CoVariantArrayConversion
// ReSharper disable PropertyCanBeMadeInitOnly.Global

/* EnumListBox.cs --
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
    public class EnumListBox
        : ListBox
    {
        #region Properties

        private Type? _enumType;

        /// <summary>
        /// Gets or sets the type of the enum.
        /// </summary>
        /// <value>The type of the enum.</value>
        [DefaultValue (null)]
        [TypeConverter (typeof (EnumTypeConverter))]
        public Type? EnumType
        {
            get => _enumType;
            set => _SetEnumType (value.ThrowIfNull());
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>The value.</value>
        public int Value
        {
            get
            {
                var result = 0;
                foreach (EnumMemberInfo item in SelectedItems)
                {
                    result |= item.Value;
                }

                return result;
            }
        }

        #endregion

        #region Private members

        private void _SetEnumType
            (
                Type enumType
            )
        {
            _enumType = enumType;

            if (_enumType is not null)
            {
                var members = EnumMemberInfo.Parse (enumType);
                Items.Clear();
                Items.AddRange(members);
            }
        }

        #endregion
    }
}
