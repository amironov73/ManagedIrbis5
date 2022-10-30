// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* AeroListView.cs --
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
/// An aero-styled ListView.
/// </summary>
/// <remarks>
/// A ListView with the "Explorer"-WindowTheme applied.
/// If the operating system is Windows XP or older, nothing will be changed.
/// </remarks>
[DesignerCategory ("Code")]
[DisplayName ("Aero ListView")]
[Description ("An aero-styled ListView.")]
[ToolboxItem (true)]
[ToolboxBitmap (typeof (ListView))]
public class AeroListView
    : ListView
{
    private const int LVS_EX_DOUBLEBUFFER = 0x10000;
    private const int LVM_SETEXTENDEDLISTVIEWSTYLE = 4150;

    /// <summary>
    /// Initializes a new instance of the <see cref="AeroListView"/> class.
    /// </summary>
    public AeroListView()
        : base()
    {
        this.FullRowSelect = true;
    }


    /// <summary>
    /// Raises the <see cref="E:HandleCreated" /> event.
    /// </summary>
    /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    protected override void OnHandleCreated (EventArgs e)
    {
        base.OnHandleCreated (e);

        if (PlatformHelper.VistaOrHigher)
        {
            NativeMethods.SetWindowTheme (this.Handle, "explorer", null);
            NativeMethods.SendMessage (this.Handle, LVM_SETEXTENDEDLISTVIEWSTYLE, new IntPtr (LVS_EX_DOUBLEBUFFER),
                new IntPtr (LVS_EX_DOUBLEBUFFER));
        }
    }
}
