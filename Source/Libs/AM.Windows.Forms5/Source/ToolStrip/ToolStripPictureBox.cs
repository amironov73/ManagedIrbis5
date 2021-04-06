// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* ToolStripPictureBox.cs -- PictureBox that appears in ToolStrip
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;

#endregion

#nullable enable

namespace AM.Windows.Forms
{
    /// <summary>
    /// <see cref="T:System.Windows.Forms.PictureBox"/> that
    /// appears in <see cref="T:System.Windows.Forms.ToolStrip"/>.
    /// </summary>
    // ReSharper disable once RedundantNameQualifier
    [System.ComponentModel.DesignerCategory("Code")]
    [ToolStripItemDesignerAvailability
        (ToolStripItemDesignerAvailability.ToolStrip
        | ToolStripItemDesignerAvailability.StatusStrip)]
    public class ToolStripPictureBox
        : ToolStripControlHost
    {
        #region Properties

        /// <summary>
        /// Gets the PictureBox.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public PictureBox PictureBox
        {
            [DebuggerStepThrough]
            get
            {
                return (PictureBox) Control;
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="ToolStripPictureBox"/> class.
        /// </summary>
        public ToolStripPictureBox()
            : base(new PictureBox())
        {
            PictureBox.BackColor = Color.Transparent;
        }

        #endregion

        #region Control members

        /// <summary>
        /// Gets or sets the size of the
        /// <see cref="T:System.Windows.Forms.ToolStripItem"/>.
        /// </summary>
        /// <returns>
        /// An ordered pair of type
        /// <see cref="T:System.Drawing.Size"/> representing
        /// the width and height of a rectangle.
        ///</returns>
        public override Size Size
        {
            [DebuggerStepThrough]
            get
            {
                return base.Size;
            }
            set
            {
                base.Size = value;
                if (PictureBox != null)
                {
                    PictureBox.Location = new Point
                    (
                        (Size.Width - PictureBox.Size.Width) / 2,
                        (Size.Height - PictureBox.Size.Height) / 2
                    );
                }
            }
        }

        #endregion
    }
}
