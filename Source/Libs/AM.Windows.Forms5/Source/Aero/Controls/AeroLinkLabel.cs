// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global
// ReSharper disable VirtualMemberCallInConstructor

/* AeroLinkLabel.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

#endregion

#nullable enable

namespace AeroSuite.Controls;

/// <summary>
/// A LinkLabel whose colors fit the Windows style. It also has a fixed hand cursor.
/// </summary>
/// <remarks>
/// The colors are extracted with Visual Styles (TextStyle > HyperLinkText). If running on XP or another OS, the colors are taken from the <see cref="SystemColors"/> class.
/// The cursor is the IDC_HAND cursor from the IDC_STANDARD_CURSORS enum.
/// </remarks>
[DesignerCategory ("Code")]
[DisplayName ("Aero LinkLabel")]
[Description ("A LinkLabel whose colors fit the Windows style. It also has a fixed hand cursor.")]
[ToolboxItem (true)]
[ToolboxBitmap (typeof (LinkLabel))]
public class AeroLinkLabel
    : LinkLabel
{
    private const int WM_SETCURSOR = 0x20;
    private const int IDC_HAND = 32649;

    /// <summary>
    ///
    /// </summary>
    protected Color DefaultColor;

    /// <summary>
    ///
    /// </summary>
    protected Color ActiveColor;

    /// <summary>
    ///
    /// </summary>
    protected Color PressedColor;

    /// <summary>
    ///
    /// </summary>
    protected Color VisitedColor;

    /// <summary>
    ///
    /// </summary>
    protected Color DisabledColor;

    /// <summary>
    /// Initializes a new instance of the <see cref="AeroLinkLabel"/> class.
    /// </summary>
    public AeroLinkLabel()
    {
        AutoSize = true;

        if (PlatformHelper.VistaOrHigher && PlatformHelper.VisualStylesEnabled)
        {
            LinkBehavior = LinkBehavior.HoverUnderline;

            //Extract colors from visual styles
            DefaultColor = new VisualStyleRenderer ("TextStyle", 6, 1).GetColor (ColorProperty.TextColor);
            ActiveColor = new VisualStyleRenderer ("TextStyle", 6, 2).GetColor (ColorProperty.TextColor);
            PressedColor = new VisualStyleRenderer ("TextStyle", 6, 3).GetColor (ColorProperty.TextColor);
            VisitedColor = new VisualStyleRenderer ("TextStyle", 6, 0).GetColor (ColorProperty.TextColor);
            DisabledColor = new VisualStyleRenderer ("TextStyle", 6, 4).GetColor (ColorProperty.TextColor);
        }
        else
        {
            //Extract colors from system colors
            DefaultColor = SystemColors.HotTrack;
            ActiveColor = SystemColors.Highlight;
            PressedColor = SystemColors.Highlight;
            VisitedColor = VisitedLinkColor; //Don't know how to get this property so I use the default one.
            DisabledColor = SystemColors.GrayText;
        }

        //Apply the extracted colors
        LinkColor = DefaultColor;
        ActiveLinkColor = ActiveColor;
        VisitedLinkColor = VisitedColor;
        DisabledLinkColor = DisabledColor;
    }


    /// <summary>
    /// Gets or sets the color used to display links in normal cases.
    /// </summary>
    /// <value>
    /// The color used to display links in normal cases.
    /// </value>
    /// <remarks>
    /// This property had to be overriden to prevent the designer creating code for it and so disabling the automatic style adaption.
    /// </remarks>
    [Category ("Appearance")]
    [DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
    public new virtual Color LinkColor
    {
        get => base.LinkColor;
        set => base.LinkColor = value;
    }

    /// <summary>
    /// Gets or sets the color used to display active links.
    /// </summary>
    /// <value>
    /// The color used to display active links.
    /// </value>
    /// <remarks>
    /// This property had to be overriden to prevent the designer creating code for it and so disabling the automatic style adaption.
    /// </remarks>
    [Category ("Appearance")]
    [DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
    public new virtual Color ActiveLinkColor
    {
        get => base.ActiveLinkColor;
        set => base.ActiveLinkColor = value;
    }

    /// <summary>
    /// Gets or sets the color used to display disabled links.
    /// </summary>
    /// <value>
    /// The color used to display disabled links.
    /// </value>
    /// <remarks>
    /// This property had to be overriden to prevent the designer creating code for it and so disabling the automatic style adaption.
    /// </remarks>
    [Category ("Appearance")]
    [DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
    public new virtual Color DisabledLinkColor
    {
        get => base.DisabledLinkColor;
        set => base.DisabledLinkColor = value;
    }

    /// <summary>
    /// Gets or sets the color used to display the link once it has been visited.
    /// </summary>
    /// <value>
    /// The color used to display the link once it has been visited..
    /// </value>
    /// <remarks>
    /// This property had to be overriden to prevent the designer creating code for it and so disabling the automatic style adaption.
    /// </remarks>
    [Category ("Appearance")]
    [DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
    public new virtual Color VisitedLinkColor
    {
        get => base.VisitedLinkColor;
        set => base.VisitedLinkColor = value;
    }

    /// <summary>
    /// Gets or sets a value that represents how the link will be underlined.
    /// </summary>
    /// <value>
    /// A value that represents how the link will be underlined.
    /// </value>
    /// <remarks>
    /// This property had to be overriden to prevent the designer creating code for it and so disabling the automatic style adaption.
    /// </remarks>
    [Category ("Appearance")]
    [DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
    public new virtual LinkBehavior LinkBehavior
    {
        get => base.LinkBehavior;
        set => base.LinkBehavior = value;
    }


    /// <summary>
    /// Raises the <see cref="E:MouseDown" /> event.
    /// </summary>
    /// <param name="eventArgs">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
    protected override void OnMouseDown
        (
            MouseEventArgs eventArgs
        )
    {
        ActiveLinkColor = PressedColor;

        base.OnMouseDown (eventArgs);
    }

    /// <summary>
    /// Raises the <see cref="E:MouseUp" /> event.
    /// </summary>
    /// <param name="eventArgs">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
    protected override void OnMouseUp
        (
            MouseEventArgs eventArgs
        )
    {
        ActiveLinkColor = ActiveColor;

        base.OnMouseUp (eventArgs);
    }

    /// <summary>
    /// Processes the specified Windows Message.
    /// </summary>
    /// <param name="m">The Message to process.</param>
    protected override void WndProc
        (
            ref Message m
        )
    {
        //Set the cursor to the Hand Cursor specified by the current Windows Theme
        if (m.Msg == WM_SETCURSOR && PlatformHelper.XpOrHigher)
        {
            NativeMethods.SetCursor (NativeMethods.LoadCursor (IntPtr.Zero, IDC_HAND));
            m.Result = IntPtr.Zero;
            return;
        }

        //Pass the WndProc call to the base
        base.WndProc (ref m);
    }
}
