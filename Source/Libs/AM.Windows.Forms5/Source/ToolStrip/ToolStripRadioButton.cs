// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* ToolStripRadioButton.cs -- RadioButton that appears in ToolStrip
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
    /// <see cref="T:System.Windows.Forms.RadioButton"/> that
    /// appears in <see cref="T:System.Windows.Forms.ToolStrip"/>.
    /// </summary>
    // ReSharper disable once RedundantNameQualifier
    [System.ComponentModel.DesignerCategory("Code")]
    [ToolStripItemDesignerAvailability
        (ToolStripItemDesignerAvailability.ToolStrip
          | ToolStripItemDesignerAvailability.StatusStrip)]
    public class ToolStripRadioButton
        : ToolStripControlHost
    {
        #region Properties

        /// <summary>
        /// Gets the RadioButton.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public RadioButton RadioButton
        {
            [DebuggerStepThrough]
            get
            {
                return (RadioButton) Control;
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="ToolStripRadioButton"/> class.
        /// </summary>
        public ToolStripRadioButton()
            : base(new RadioButton())
        {
        }

        #endregion
    }
}
