// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedType.Global

/* HotkeyBox.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using AM;

#endregion

#nullable enable

namespace AeroSuite.Controls;

/// <summary>
/// A TextBox used for setting and displaying keyboard shortcuts/hotkeys.
/// </summary>
/// <remarks>
/// This control implements the 'msctls_hotkey32' common control which
/// means that all the actions are handled by windows internally
/// so that the language is
/// </remarks>
[DesignerCategory ("Code")]
[DisplayName ("Hotkey Box")]
[Description ("A TextBox used for hotkeys.")]
[ToolboxItem (true)]
[ToolboxBitmap (typeof (TextBox))]
public class HotkeyBox
    : TextBox
{
    private const int HKM_SETHOTKEY = 1025;
    private const int HKM_GETHOTKEY = 1026;
    private const int HKM_SETRULES = 1027;

    /// <summary>
    /// Gets the required creation parameters when the control handle is created.
    /// </summary>
    protected override CreateParams CreateParams
    {
        get
        {
            var baseParams = base.CreateParams;

            baseParams.ClassName = "msctls_hotkey32";

            return baseParams;
        }
    }

    /// <summary>
    /// Gets or sets the hotkey selected in this hotkey box.
    /// </summary>
    /// <value>
    /// The hotkey.
    /// </value>
    [Category ("Data")]
    [Description ("The hotkey selected in this hotkey box.")]
    [Localizable (false)]
    [Bindable (true)]
    public Keys Hotkey
    {
        get => (Keys)(NativeMethods.SendMessage (Handle, HKM_GETHOTKEY, IntPtr.Zero, IntPtr.Zero));
        set => NativeMethods.SendMessage (Handle, HKM_SETHOTKEY, (IntPtr)value, IntPtr.Zero);
    }

    /// <summary>
    /// Gets or sets the text.
    /// </summary>
    /// <value>
    /// The text.
    /// </value>
    [EditorBrowsable (EditorBrowsableState.Never)]
    [Browsable (false)]
    [DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
    public override string Text
    {
        get => base.Text;
        set => value.NotUsed();
    }
}
