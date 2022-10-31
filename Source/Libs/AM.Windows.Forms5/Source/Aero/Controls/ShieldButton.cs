// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* ShieldButton.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AeroSuite.Controls;

/// <summary>
/// A normal button with a Shield.
/// </summary>
/// <remarks>
/// The shield is extracted from the system with LoadImage if possible. Otherwise the shield will be enabled by sending the BCM_SETSHIELD Message to the control.
/// If the operating system is not Windows Vista or higher, no shield will be displayed as there's no such thing as UAC on the target system -> the shield is obsolete.
/// </remarks>
[DesignerCategory ("Code")]
[DisplayName ("Shield Button")]
[Description ("A normal button with a Shield.")]
[ToolboxItem (true)]
[ToolboxBitmap (typeof (ShieldButton))]
public class ShieldButton
    : Button
{
    private const int BCM_SETSHIELD = 0x160C;
    private static bool? isSystemAbleToLoadShield = null;

    /// <summary>
    /// Initializes a new instance of the <see cref="ShieldButton"/> class.
    /// </summary>
    public ShieldButton()
    {
        Size = new Size ((int)(Width * 1.5), Height + 1);
        if (PlatformHelper.VistaOrHigher)
        {
            //Only try to load the icon if it did not fail before
            if (!isSystemAbleToLoadShield.HasValue || isSystemAbleToLoadShield.Value)
            {
                try
                {
                    var icon = IconExtractor.LoadIcon (IconExtractor.IconType.Shield, SystemInformation.SmallIconSize);
                    if (icon != null)
                    {
                        Image = icon.ToBitmap();
                        TextImageRelation = TextImageRelation.ImageBeforeText;
                        ImageAlign = ContentAlignment.MiddleRight;

                        isSystemAbleToLoadShield = true;
                        return;
                    }
                    else
                    {
                        isSystemAbleToLoadShield = false;
                    }
                }
                catch (PlatformNotSupportedException)
                {
                    //This happens when the system does not support this call
                    //Prevent future calling
                    isSystemAbleToLoadShield = false;
                }
            }

            //Preferred way not possible
            base.FlatStyle =
                FlatStyle.System; //Need to access the base property as the overriden one would cause some overhead
            NativeMethods.SendMessage (Handle, BCM_SETSHIELD, IntPtr.Zero, new IntPtr (1));
        }
    }

    /// <summary>
    /// Gets or sets the flat style appearance of the shield button control.
    /// </summary>
    /// <value>
    /// The flat style.
    /// </value>
    [Category ("Appearance")]
    [Description ("The flat style appearance of the shield button control.")]
    public new virtual FlatStyle FlatStyle
    {
        get => base.FlatStyle;
        set
        {
            if (value != base.FlatStyle)
            {
                base.FlatStyle = value;

                //Update the shield icon if needed
                if (PlatformHelper.VistaOrHigher)
                {
                    if (base.FlatStyle == FlatStyle.System)
                    {
                        //Apply Shield the WinAPI-Way
                        NativeMethods.SendMessage (Handle, BCM_SETSHIELD, IntPtr.Zero, new IntPtr (1));
                    }
                    else
                    {
                        //Try applying it the other way
                        if (isSystemAbleToLoadShield.Value)
                        {
                            Image = IconExtractor
                                .LoadIcon (IconExtractor.IconType.Shield, SystemInformation.SmallIconSize).ToBitmap();
                            TextImageRelation = TextImageRelation.ImageBeforeText;
                            ImageAlign = ContentAlignment.MiddleRight;
                        }
                    }
                }
            }
        }
    }
}
