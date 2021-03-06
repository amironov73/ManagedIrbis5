﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedType.Global

/* InputLanguageAwareForm.cs -- форма, умеющая менять язык ввода по запросу
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms
{
    /// <summary>
    /// Форма, умеющая менять язык ввода по запросу.
    /// </summary>
    public class InputLanguageAwareForm
        : Form
    {
        #region Control members

        /// <inheritdoc cref="Control.WndProc"/>
        protected override void WndProc
            (
                ref Message message
            )
        {
            if (!InputLanguageUtility.HandleWmInputLanguageRequest(ref message))
            {
                base.WndProc(ref message);
            }
        } // method WndProc

        #endregion

    } // class InputLanguageAwareForm

} // namespace AM.Windows.Forms
