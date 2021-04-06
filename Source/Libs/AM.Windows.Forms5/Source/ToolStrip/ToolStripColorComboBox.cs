// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* ToolStripColorComboBox.cs -- ComboBox that appears in ToolStrip
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
    /// <see cref="T:System.Windows.Forms.ComboBox"/> that
    /// appears in <see cref="T:System.Windows.Forms.ToolStrip"/>.
    /// </summary>
    [System.ComponentModel.DesignerCategory("Code")]
    [ToolStripItemDesignerAvailability
        (ToolStripItemDesignerAvailability.ToolStrip
          | ToolStripItemDesignerAvailability.StatusStrip)]
    public class ToolStripColorComboBox
        : ToolStripControlHost
    {
        #region Properties

        /// <summary>
        /// Gets the ColorComboBox.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ColorComboBox ColorComboBox
        {
            [DebuggerStepThrough]
            get
            {
                return (ColorComboBox) Control;
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="ToolStripColorComboBox"/> class.
        /// </summary>
        public ToolStripColorComboBox()
            : base(new ColorComboBox())
        {
        }

        #endregion
    }
}
