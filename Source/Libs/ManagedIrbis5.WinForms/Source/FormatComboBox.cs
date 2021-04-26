// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* FormatComboBox.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Windows.Forms;

#endregion

#nullable enable

namespace ManagedIrbis.WinForms
{
    /// <summary>
    ///
    /// </summary>
    [System.ComponentModel.DesignerCategory("Code")]
    public class FormatComboBox
        : ComboBox
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public FormatComboBox()
        {
            DropDownStyle = ComboBoxStyle.DropDownList;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Fill the combo box with scenarios list.
        /// </summary>
        public void FillWithFormats
            (
                ISyncProvider connection,
                string database
            )
        {
            throw new NotImplementedException();

        }

        #endregion
    }
}
