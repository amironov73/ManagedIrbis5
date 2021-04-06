// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* ToolStripNumericUpDown.cs -- NumericUpDown that appears in ToolStrip
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
    /// <see cref="T:System.Windows.Forms.NumericUpDown"/> that
    /// appears in <see cref="T:System.Windows.Forms.ToolStrip"/>.
    /// </summary>
    // ReSharper disable once RedundantNameQualifier
    [System.ComponentModel.DesignerCategory("Code")]
    [ToolStripItemDesignerAvailability
        (ToolStripItemDesignerAvailability.ToolStrip
          | ToolStripItemDesignerAvailability.StatusStrip)]
    public class ToolStripNumericUpDown
        : ToolStripControlHost
    {
        #region Properties

        /// <summary>
        /// Gets the NumericUpDown.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public NumericUpDown NumericUpDown
        {
            [DebuggerStepThrough]
            get
            {
                return (NumericUpDown) Control;
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="ToolStripNumericUpDown"/> class.
        /// </summary>
        public ToolStripNumericUpDown()
            : base(new NumericUpDown())
        {
        }

        #endregion
    }
}
