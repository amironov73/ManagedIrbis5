// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* Seperator.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Windows.Forms.VisualStyles;

#endregion

#nullable enable

namespace AeroSuite.Controls;

/// <summary>
/// A seperator line.
/// </summary>
/// <remarks>
/// The line is drawn with Visual Styles (TaskDialog > FootnoteSeperator).
/// If running on XP or another OS, the line is drawn manually.
/// </remarks>
[DesignerCategory ("Code")]
[Designer (typeof (SeparatorDesigner))]
[DisplayName ("Separator")]
[DefaultProperty ("")]
[Description ("A seperator line drawn by Windows via Visual Styles if available.")]
[ToolboxItem (true)]
[ToolboxBitmap (typeof (Separator))]
public class Separator
    : Control
{
    /// <summary>
    /// Returns the default size.
    /// </summary>
    /// <value>
    /// The default size.
    /// </value>
    protected override Size DefaultSize => new Size (250, 2);

    /// <summary>
    /// Initializes a new instance of the <see cref="Separator"/> class.
    /// </summary>
    public Separator()
    {
        TabStop = false;
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
    /// Hidden because the property is not used
    /// </summary>
    /// <value>
    /// The text.
    /// </value>
    [Browsable (false), EditorBrowsable (EditorBrowsableState.Never), Bindable (false)]
    public override string Text
    {
        get => base.Text;
        set => base.Text = value;

        //Make it work in the test project & designer -> remove Exception
        //throw new NotSupportedException("This control does not support a Text");
    }

    /// <inheritdoc cref="Control.OnPaint"/>
    protected override void OnPaint
        (
            PaintEventArgs eventArgs
        )
    {
        var graphics = eventArgs.Graphics;
        if (Application.RenderWithVisualStyles &&
            VisualStyleRenderer.IsElementDefined (
                VisualStyleElement.CreateElement ("TaskDialog", 17,
                    0)) /*PlatformHelper.VistaOrHigher && PlatformHelper.VisualStylesEnabled*/
           ) //This seems to be the right check according to the MSDN: https://msdn.microsoft.com/en-us/library/vstudio/ms171735(v=vs.100).aspx
        {
            new VisualStyleRenderer ("TaskDialog", 17, 0)
                .DrawBackground (graphics, DisplayRectangle);
        }
        else
        {
            graphics.DrawLine (SystemPens.ControlDark, new Point (0, 0), new Point (Width, 0));
            graphics.DrawLine (SystemPens.ControlLightLight, new Point (0, 1), new Point (Width, 1));
        }

        base.OnPaint (eventArgs);
    }


    /// <summary>
    /// Provides a ControlDesigner for the <see cref="Separator"/> Control.
    /// </summary>
    internal class SeparatorDesigner
        : ControlDesigner
    {
        /// <summary>
        /// Returns selection rules for the <see cref="Separator"/> Control.
        /// </summary>
        /// <value>
        /// The selection rules.
        /// </value>
        public override SelectionRules SelectionRules => SelectionRules.Moveable | SelectionRules.LeftSizeable | SelectionRules.RightSizeable;
    }
}
