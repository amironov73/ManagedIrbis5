// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* DragForm.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Drawing;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms.Docking;

// Inspired by Chris Sano's article:
// http://msdn.microsoft.com/smartclient/default.aspx?pull=/library/en-us/dnwinforms/html/colorpicker.asp
// In Sano's article, the DragForm needs to meet the following criteria:
// (1) it was not to show up in the task bar;
//     ShowInTaskBar = false
// (2) it needed to be the top-most window;
//     TopMost = true
// (3) its icon could not show up in the ALT+TAB window if the user pressed ALT+TAB during a drag-and-drop;
//     FormBorderStyle = FormBorderStyle.None;
//     Create with WS_EX_TOOLWINDOW window style.
//     Compares with the solution in the artile by setting FormBorderStyle as FixedToolWindow,
//     and then clip the window caption and border, this way is much simplier.
// (4) it was not to steal focus from the application when displayed.
//     User Win32 ShowWindow API with SW_SHOWNOACTIVATE
// In addition, this form should only for display and therefore should act as transparent, otherwise
// WindowFromPoint will return this form, instead of the control beneath. Need BOTH of the following to
// achieve this (don't know why, spent hours to try it out :( ):
//  1. Enabled = false;
//  2. WM_NCHITTEST returns HTTRANSPARENT

/// <summary>
///
/// </summary>
public class DragForm
    : Form
{
    /// <summary>
    ///
    /// </summary>
    public DragForm()
    {
        FormBorderStyle = FormBorderStyle.None;
        ShowInTaskbar = false;
        SetStyle (ControlStyles.Selectable, false);
        Enabled = false;
        TopMost = true;
        SizeChanged += (sender, args) =>
        {
            if (BackgroundColor != null)
            {
                Invalidate();
            }
        };
    }

    /// <summary>
    ///
    /// </summary>
    public Color? BackgroundColor { get; set; }

    /// <inheritdoc cref="Form.CreateParams"/>
    protected override CreateParams CreateParams
    {
        get
        {
            CreateParams createParams = base.CreateParams;
            createParams.ExStyle |=
                (int)(Win32.WindowExStyles.WS_EX_NOACTIVATE | Win32.WindowExStyles.WS_EX_TOOLWINDOW);
            return createParams;
        }
    }

    /// <inheritdoc cref="Form.WndProc"/>
    protected override void WndProc (ref Message m)
    {
        if (m.Msg == (int)Win32.Msgs.WM_NCHITTEST)
        {
            m.Result = (IntPtr)Win32.HitTest.HTTRANSPARENT;
            return;
        }

        base.WndProc (ref m);
    }

    /// <inheritdoc cref="Form.OnPaint"/>
    protected override void OnPaint (PaintEventArgs e)
    {
        base.OnPaint (e);
        if (BackgroundColor == null)
        {
            return;
        }

        var all = ClientRectangle;
        if (all is { Width: > 10, Height: > 10 })
        {
            var newLocation = new Point (all.Location.X + 5, all.Location.Y + 5);
            var newSize = new Size (all.Width - 10, all.Height - 10);
            var center = new Rectangle (newLocation, newSize);
            e.Graphics.FillRectangle (new SolidBrush (BackgroundColor.Value), center);
        }
    }

    //The form can be still activated by explicity calling Activate
    /// <inheritdoc cref="Form.ShowWithoutActivation"/>
    protected override bool ShowWithoutActivation => true;

    /// <summary>
    ///
    /// </summary>
    /// <param name="bActivate"></param>
    public virtual void Show (bool bActivate)
    {
        Show();

        if (bActivate)
        {
            Activate();
        }
    }
}
