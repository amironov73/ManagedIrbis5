﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Windows.Forms.VisualStyles;

namespace AeroSuite.Controls
{
    /// <summary>
    /// A "Bottom Panel" drawn by Windows via Visual Styles if available.
    /// This Panel can be used for providing additional information or Buttons on the bottom of a Form/Dialog.
    /// </summary>
    /// <remarks>
    /// The panel is drawn with Visual Styles (TaskDialog > SecondaryPanel). If running on XP or another OS, the panel is drawn manually
    /// </remarks>
    [DesignerCategory("Code")]
    [DisplayName("Bottom Panel")]
    [Description("A \"Bottom Panel\" that can be used for providing additional information or Buttons on the bottom of a Form/Dialog.")]
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(BottomPanel))]
    public class BottomPanel
        : Panel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BottomPanel"/> class.
        /// </summary>
        public BottomPanel()
        {
            this.Dock = DockStyle.Bottom;
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
            this.UpdateStyles();
        }

        /// <summary>
        /// Returns the default size.
        /// </summary>
        /// <value>
        /// The default size.
        /// </value>
        protected override Size DefaultSize
        {
            get
            {
                return new Size(base.DefaultSize.Width, 40);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:Paint" /> event.
        /// </summary>
        /// <param name="e">The <see cref="PaintEventArgs"/> instance containing the event data.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            if (Application.RenderWithVisualStyles && VisualStyleRenderer.IsElementDefined(VisualStyleElement.CreateElement("TaskDialog", 8, 0))/*PlatformHelper.VistaOrHigher && PlatformHelper.VisualStylesEnabled*/) //This seems to be the right check according to the MSDN: https://msdn.microsoft.com/en-us/library/vstudio/ms171735(v=vs.100).aspx
            {
                this.PaintWithVisualStyles(e.Graphics);
            }
            else
            {
                this.PaintManually(e.Graphics);
            }

            base.OnPaint(e);
        }

        /// <summary>
        /// Paints the panel with visual styles.
        /// </summary>
        /// <param name="g">The targeted graphics.</param>
        protected virtual void PaintWithVisualStyles(Graphics g)
        {
            new VisualStyleRenderer("TaskDialog", 8, 0).DrawBackground(g, this.DisplayRectangle);
        }

        /// <summary>
        /// Paints the panel manually.
        /// </summary>
        /// <param name="g">The targeted graphics.</param>
        protected virtual void PaintManually(Graphics g)
        {
            g.Clear(SystemColors.Control);
            g.DrawLine(SystemPens.ControlDark, new Point(0, 0), new Point(this.Width, 0));
        }
    }
}
