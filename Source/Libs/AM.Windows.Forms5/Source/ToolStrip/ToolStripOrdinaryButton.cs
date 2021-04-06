// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* ToolStripOrdinaryButton.cs -- Button that appears in ToolStrip.
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
    /// <see cref="T:System.Windows.Forms.Button"/> that
    /// appears in <see cref="T:System.Windows.Forms.ToolStrip"/>.
    /// </summary>
    // ReSharper disable once RedundantNameQualifier
    [System.ComponentModel.DesignerCategory("Code")]
    [ToolStripItemDesignerAvailability
        (ToolStripItemDesignerAvailability.ToolStrip
        | ToolStripItemDesignerAvailability.StatusStrip)]
    public class ToolStripOrdinaryButton
        : ToolStripControlHost
    {
        #region Properties

        /// <summary>
        /// Gets the Button.
        /// </summary>
        [DesignerSerializationVisibility
            (DesignerSerializationVisibility.Content)]
        public Button Button
        {
            [DebuggerStepThrough]
            get
            {
                return (Button) Control;
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ToolStripOrdinaryButton()
            : base(new Button())
        {
        }

        #endregion
    }
}
