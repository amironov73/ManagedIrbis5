// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* TreeGridDateBox.cs
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms
{
    /// <summary>
    ///
    /// </summary>
    public class TreeGridDateBox
        : TreeGridEditor
    {
        #region Construction

        public TreeGridDateBox()
        {
            _control = new DateTimePicker();
        }

        #endregion

        #region Private members

        private readonly DateTimePicker _control;

        #endregion

        #region Properties

        public DateTimePicker DateTimePicker
        {
            get { return _control;  }
        }

        #endregion

        #region TreeGridEditor members

        public override Control Control
        {
            get { return _control; }
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="value">The value.</param>
        public override void SetValue(string value)
        {
            //ComboBox.Text = value;
            Control.Text = value;
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <returns></returns>
        public override string GetValue()
        {
            return Control.Text;
        }

        /// <summary>
        /// Selects the text.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="length">The length.</param>
        public override void SelectText(int start, int length)
        {
        }

        #endregion
    }
}
