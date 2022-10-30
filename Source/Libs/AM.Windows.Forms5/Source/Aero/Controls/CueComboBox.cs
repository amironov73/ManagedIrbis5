// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* CueComboBox.cs --
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
/// A ComboBox with cue banner support.
/// </summary>
/// <remarks>
/// A cue banner is the text that is shown when the ComboBox does not have a selected item.
/// </remarks>
[DesignerCategory ("Code")]
[DisplayName ("Cue ComboBox")]
[Description ("A ComboBox with cue banner support.")]
[ToolboxItem (true)]
[ToolboxBitmap (typeof (ComboBox))]
public class CueComboBox
    : ComboBox
{
    private const int CB_SETCUEBANNER = 0x1703;

    /// <summary>
    /// Initializes a new instance of the <see cref="CueComboBox"/> class.
    /// </summary>
    public CueComboBox()
    {
        // пустое тело конструктора
    }


    private string cue = string.Empty;

    /// <summary>
    /// The text shown on the Cue Banner.
    /// </summary>
    /// <value>
    /// The cue.
    /// </value>
    [Category ("Appearance")]
    [Description ("The text shown on the Cue Banner.")]
    [Localizable (true)]
    [Bindable (true)]
    public virtual string Cue
    {
        get => cue;
        set
        {
            cue = value;
            UpdateCue();
        }
    }

    /// <summary>
    /// Updates the cue.
    /// </summary>
    private void UpdateCue()
    {
        if (IsHandleCreated &&
            PlatformHelper.VistaOrHigher)
            //Cue banners in ComboBoxes are not supported on Windows XP altough they are in TextBoxes??
        {
            NativeMethods.SendMessage (Handle, CB_SETCUEBANNER, IntPtr.Zero, cue);
        }
    }

    /// <summary>
    /// Raises the <see cref="Control.HandleCreated" /> event.
    /// </summary>
    /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    protected override void OnHandleCreated (EventArgs e)
    {
        base.OnHandleCreated (e);
        UpdateCue();
    }
}
