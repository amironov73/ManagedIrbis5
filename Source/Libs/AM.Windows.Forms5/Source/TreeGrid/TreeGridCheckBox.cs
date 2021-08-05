// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* TreeGridCheckBox.cs
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Windows.Forms;

#endregion

namespace AM.Windows.Forms
{
    /// <summary>
    ///
    /// </summary>
    public class TreeGridCheckBox
        : TreeGridEditor
    {
        #region Construction

        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        public TreeGridCheckBox()
        {
            _control = new CheckBox();
        }

        #endregion

        #region Private members

        private readonly CheckBox _control;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the control.
        /// </summary>
        /// <value>The control.</value>
        public override Control Control => _control;

        /// <summary>
        /// Gets the check box.
        /// </summary>
        /// <value>The check box.</value>
        public CheckBox CheckBox => _control;

        #endregion

        #region TreeGridEditor members

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="value">The value.</param>
        public override void SetValue(string? value)
        {
            _control.Checked = Convert.ToBoolean(value);
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <returns></returns>
        public override string GetValue()
        {
            return _control.Checked.ToString();
        }

        /// <summary>
        /// Selects the text.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="length">The length.</param>
        public override void SelectText(int start, int length)
        {
            // Nothing to do
        }

        #endregion
    }
}
