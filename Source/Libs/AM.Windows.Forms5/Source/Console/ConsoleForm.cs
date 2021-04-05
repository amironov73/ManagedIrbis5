﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* ConsoleForm.cs -- form with ConsoleControl
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Media;
using System.Text;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms
{
    /// <summary>
    /// Form with <see cref="ConsoleControl"/> inside.
    /// </summary>
    public sealed class ConsoleForm
        : ModalForm
    {
        #region Properties

        /// <summary>
        /// Console control.
        /// </summary>
        public ConsoleControl Console => _console;

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ConsoleForm()
        {
            _console = new ConsoleControl();
            int consoleWidth = _console.Width;
            int consoleHeight = _console.Height;

            Controls.Add(_console);
            ClientSize = new Size(consoleWidth, consoleHeight);
        }

        #endregion

        #region Private members

        private readonly ConsoleControl _console;

        #endregion
    }
}
