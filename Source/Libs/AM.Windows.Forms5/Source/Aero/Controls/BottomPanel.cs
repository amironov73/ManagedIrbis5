// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global
// ReSharper disable VirtualMemberCallInConstructor

/* BottomPanel.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

using AM;

#endregion

#nullable enable

namespace AeroSuite.Controls;

/// <summary>
/// A "Bottom Panel" drawn by Windows via Visual Styles if available.
/// This Panel can be used for providing additional information or Buttons on the bottom of a Form/Dialog.
/// </summary>
/// <remarks>
/// The panel is drawn with Visual Styles (TaskDialog > SecondaryPanel). If running on XP or another OS, the panel is drawn manually
/// </remarks>
[DesignerCategory ("Code")]
[DisplayName ("Bottom Panel")]
[Description (
    "A \"Bottom Panel\" that can be used for providing additional information or Buttons on the bottom of a Form/Dialog.")]
[ToolboxItem (true)]
[ToolboxBitmap (typeof (BottomPanel))]
public class BottomPanel
    : Panel
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BottomPanel"/> class.
    /// </summary>
    public BottomPanel()
    {
        Dock = DockStyle.Bottom;
        SetStyle
            (
                ControlStyles.AllPaintingInWmPaint
                | ControlStyles.OptimizedDoubleBuffer
                | ControlStyles.ResizeRedraw
                | ControlStyles.UserPaint,
                true
            );
        UpdateStyles();
    }

    /// <summary>
    /// Returns the default size.
    /// </summary>
    /// <value>
    /// The default size.
    /// </value>
    protected override Size DefaultSize => new (base.DefaultSize.Width, 40);

    /// <summary>
    /// Raises the <see cref="E:Paint" /> event.
    /// </summary>
    /// <param name="eventArgs">The <see cref="PaintEventArgs"/> instance containing the event data.</param>
    protected override void OnPaint
        (
            PaintEventArgs eventArgs
        )
    {
        Sure.NotNull (eventArgs);

        if (Application.RenderWithVisualStyles &&
            VisualStyleRenderer.IsElementDefined
                (
                    VisualStyleElement.CreateElement ("TaskDialog", 8, 0)
                ) /*PlatformHelper.VistaOrHigher && PlatformHelper.VisualStylesEnabled*/
           ) //This seems to be the right check according to the MSDN: https://msdn.microsoft.com/en-us/library/vstudio/ms171735(v=vs.100).aspx
        {
            PaintWithVisualStyles (eventArgs.Graphics);
        }
        else
        {
            PaintManually (eventArgs.Graphics);
        }

        base.OnPaint (eventArgs);
    }

    /// <summary>
    /// Paints the panel with visual styles.
    /// </summary>
    /// <param name="graphics">The targeted graphics.</param>
    protected virtual void PaintWithVisualStyles
        (
            Graphics graphics
        )
    {
        Sure.NotNull (graphics);

        new VisualStyleRenderer
            (
                "TaskDialog",
                8,
                0
            )
            .DrawBackground (graphics, DisplayRectangle);
    }

    /// <summary>
    /// Paints the panel manually.
    /// </summary>
    /// <param name="graphics">The targeted graphics.</param>
    protected virtual void PaintManually
        (
            Graphics graphics
        )
    {
        Sure.NotNull (graphics);

        graphics.Clear (SystemColors.Control);
        graphics.DrawLine (SystemPens.ControlDark, new Point (0, 0), new Point (Width, 0));
    }
}
