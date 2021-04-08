// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PftDebuggerForm.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

#endregion

#nullable enable

namespace ManagedIrbis.WinForms.Pft
{
    /// <summary>
    /// Form for PFT debugger.
    /// </summary>
    public partial class PftDebuggerForm
        : Form
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftDebuggerForm()
        {
            InitializeComponent();
        }

        #endregion
    }
}
