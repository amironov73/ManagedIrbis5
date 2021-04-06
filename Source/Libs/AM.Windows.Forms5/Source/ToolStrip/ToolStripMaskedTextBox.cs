// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* ToolStripMaskedTextBox.cs -- MaskedTextBox that appears in ToolStrip.
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;
using System.Windows.Forms.Design;

#endregion

#nullable enable

namespace AM.Windows.Forms
{
    /// <summary>
    /// <see cref="T:System.Windows.Forms.MaskedTextBox"/> that
    /// appears in <see cref="T:System.Windows.Forms.ToolStrip"/>.
    /// </summary>
    // ReSharper disable once RedundantNameQualifier
    [System.ComponentModel.DesignerCategory("Code")]
    [ToolStripItemDesignerAvailability
        (ToolStripItemDesignerAvailability.ToolStrip
          | ToolStripItemDesignerAvailability.StatusStrip)]
    public class ToolStripMaskedTextBox
        : ToolStripControlHost
    {
        #region Properties

        /// <summary>
        /// Gets the MaskedTextBox.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public MaskedTextBox MaskedTextBox
        {
            [DebuggerStepThrough]
            get
            {
                return (MaskedTextBox) Control;
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ToolStripMaskedTextBox()
            : base(new MaskedTextBox())
        {
        }

        #endregion
    }
}
