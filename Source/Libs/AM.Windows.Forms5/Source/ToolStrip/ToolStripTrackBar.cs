﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* ToolStripTrackBar.cs -- TrackBar that appears in ToolStrip
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
    /// <see cref="T:System.Windows.Forms.TrackBar"/> that
    /// appears in <see cref="T:System.Windows.Forms.ToolStrip"/>.
    /// </summary>
    // ReSharper disable once RedundantNameQualifier
    [System.ComponentModel.DesignerCategory("Code")]
    [ToolStripItemDesignerAvailability
        (ToolStripItemDesignerAvailability.ToolStrip
          | ToolStripItemDesignerAvailability.StatusStrip)]
    public class ToolStripTrackBar
        : ToolStripControlHost
    {
        #region Nested classes

        class TrackBarWithTransparentBackground
            : TrackBar
        {
            public TrackBarWithTransparentBackground()
            {
                SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the TrackBar.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public TrackBar TrackBar
        {
            [DebuggerStepThrough]
            get
            {
                return (TrackBar) Control;
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="ToolStripTrackBar"/> class.
        /// </summary>
        public ToolStripTrackBar()
            : base(new TrackBarWithTransparentBackground())
        {
            // TODO support transparent background
            TrackBar.BackColor = Color.Transparent;
            TrackBar.BackColor = SystemColors.InactiveCaption;
            TrackBar.AutoSize = false;
            AutoSize = false;
        }

        #endregion
    }
}
