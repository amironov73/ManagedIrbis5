// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
// ReSharper disable VirtualMemberCallInConstructor

/* BorderlessForm.cs -- форма без рамок
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using AM;

#endregion

#nullable enable

namespace AeroSuite.Forms;

/// <summary>
/// A <see cref="Form"/> base class which allows the creation of borderless
/// windows which supports AeroSnap, AeroPeek, the system window context menu,
/// optionally an Aero shadow, all of Windows' window animations and even
/// windows-like dragging &amp; resizing in specified areas.
/// </summary>
/// <remarks>
/// The various features of this borderless form are implemented by using
/// a normal window as a base and removing all of its "features" like the
/// titlebar and borders visually and functionally during runtime by handling
/// the appropriate windows messages.
/// That makes this variant of a borderless window superior to just setting
/// the border style to <see cref="System.Windows.Forms.FormBorderStyle.None"/>
/// which lacks all of the features described in the summary.
/// To implement the various areas for resizing &amp; dragging, you have
/// to override the <see cref="BorderlessForm.PerformHitTest(Point)"/>-method
/// and check for the areas you desire.
/// </remarks>
[DesignerCategory ("Code")]
[DisplayName ("Borderless Form")]
[Description ("A borderless form with support")]
public class BorderlessForm
    : Form
{
    private const int CS_DROPSHADOW = 0x20000;

    /// <summary>
    /// Initializes a new instance of the <see cref="BorderlessForm"/> class.
    /// </summary>
    public BorderlessForm()
    {
        base.FormBorderStyle = FormBorderStyle.None;
        BackColor = SystemColors.Window;
        Font = SystemFonts.MessageBoxFont;
    }

    /// <summary>
    /// Gets or sets the border style of the form.
    /// </summary>
    [EditorBrowsable (EditorBrowsableState.Never)]
    [Browsable (false)]
    [DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
    [DefaultValue (FormBorderStyle.None)]
    protected new virtual FormBorderStyle FormBorderStyle
    {
        get => Borderless ? FormBorderStyle.None : FormBorderStyle.Sizable;
        set
        {
            switch (value)
            {
                case FormBorderStyle.None:
                    Borderless = true;
                    break;

                default:
                    _borderless = false;
                    break;
            }
        }
    }

    private bool _borderless = true;

    /// <summary>
    /// Gets or sets a value indicating whether this <see cref="BorderlessForm"/> should be displayed without a window border.
    /// </summary>
    /// <value>
    ///   <c>true</c> if borderless; otherwise, <c>false</c>.
    /// </value>
    [Bindable (true)]
    [DefaultValue (true)]
    [Category ("Appearance")]
    [Description ("Determines, whether this form should be displayed without a window border.")]
    public virtual bool Borderless
    {
        get => _borderless;
        set
        {
            _borderless = value;
            base.FormBorderStyle = value ? FormBorderStyle.None : FormBorderStyle.Sizable;

            //if (this.IsHandleCreated)
            //{
            //    this.UpdateStyle();
            //}
        }
    }

    private bool _showShadow = true;

    /// <summary>
    /// Gets or sets a value indicating whether this <see cref="BorderlessForm"/> should be displayed with an aero shadow underneath. This only applies when the form is borderless.
    /// </summary>
    /// <value>
    ///   <c>true</c> if a shadow is cast; otherwise, <c>false</c>.
    /// </value>
    [Bindable (true)]
    [DefaultValue (true)]
    [Category ("Appearance")]
    [Description (
        "Determines, whether this form should be displayed with an aero shadow underneath. This only applies when the form is borderless.")]
    public virtual bool Shadow
    {
        get => _showShadow;
        set
        {
            _showShadow = value;
            if (IsHandleCreated)
            {
                //this.UpdateStyle();
                RecreateHandle();
            }
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the default behaviour when clicking on the form is to drag it or to ignore the action. The effect of this property can change if <see cref="BorderlessForm.PerformHitTest(Point)"/> is overriden.
    /// </summary>
    /// <value>
    ///   <c>true</c> if the form is automatically dragged when the cursor clicks on it; otherwise, <c>false</c>.
    /// </value>
    [Bindable (true)]
    [DefaultValue (true)]
    [Category ("Behaviour")]
    [Description (
        "Determines, whether the default behaviour when clicking on the form is to drag it or to ignore the action.")]
    public virtual bool AutoDrag { get; set; } = true;


    /// <summary>
    /// Performs a hit-test which determines how the form behaves when the cursor hovers or clicks on certain areas of the form. Override this method to customize how this form handles those events.
    /// </summary>
    /// <param name="location">The location of the cursor.</param>
    /// <returns>A <see cref="HitTestResult"/> determining the area underneath the cursor and therefore the forms behaviour at that location.</returns>
    protected virtual HitTestResult PerformHitTest (Point location)
    {
        return AutoDrag ? HitTestResult.Caption : HitTestResult.Client;
    }

    /// <summary>
    /// Creates an unsigned integer determining the window style of this <see cref="BorderlessForm"/>.
    /// </summary>
    protected virtual uint GetWindowStyle()
    {
        if (Borderless)
        {
            if (PlatformHelper.VistaOrHigher && NativeMethods.DwmIsCompositionEnabled())
            {
                return (uint)(WindowStyles.Overlapped | WindowStyles.ThickFrame | WindowStyles.Caption |
                              WindowStyles.SysMenu | WindowStyles.MinimizeBox | WindowStyles.MaximizeBox);
            }
            else
            {
                return (uint)(WindowStyles.Overlapped | WindowStyles.ThickFrame | WindowStyles.SysMenu);
            }
        }
        else
        {
            //return (uint)(WindowStyles.Popup | WindowStyles.ThickFrame | WindowStyles.Caption | WindowStyles.SysMenu | WindowStyles.MinimizeBox | WindowStyles.MaximizeBox);
            return (uint)base.CreateParams.Style;
        }
    }

    /// <summary>
    /// Initializes the shadow underneath this <see cref="BorderlessForm"/>. Only call this method if <see cref="BorderlessForm.Borderless"/> is set to <c>true</c>, otherwise it might cause undefined behaviour.
    /// </summary>
    protected virtual void InitializeShadow()
    {
        if (Shadow && Borderless)
        {
            if (PlatformHelper.VistaOrHigher && NativeMethods.DwmIsCompositionEnabled())
            {
                if (Shadow)
                {
                    var margins = new Margins { BottomHeight = 1, LeftWidth = 1, RightWidth = 1, TopHeight = 1 };
                    NativeMethods.DwmExtendFrameIntoClientArea (Handle, ref margins);
                }
                else
                {
                    var margins = new Margins { BottomHeight = 0, LeftWidth = 0, RightWidth = 0, TopHeight = 0 };
                    margins.NotUsed();
                }
            }
        }
    }

    ///// <summary>
    ///// Updates the style of this <see cref="BorderlessForm"/>. This method is called after every custom property change which affects the visual appearance. Try calling this method if you try to change something about the form and it does not work as expected.
    ///// </summary>
    //protected virtual void UpdateStyle()
    //{
    //    //This should mostly work but it causes some visual errors to occur and failes to load the form's icon so I'm going to disable this option for now and use the quick and not even so dirty alternative way
    //    //if (IntPtr.Size == 8)
    //    //{
    //    //    NativeMethods.SetWindowLongPtr64(this.Handle, GWL_STYLE, new IntPtr(unchecked((int)this.GetWindowStyle())));
    //    //}
    //    //else
    //    //{
    //    //    NativeMethods.SetWindowLong32(this.Handle, GWL_STYLE, new IntPtr(unchecked((int)this.GetWindowStyle())));
    //    //}
    //
    //    //if (this.Borderless)
    //    //{
    //    //    this.InitializeShadow();
    //    //}
    //
    //    //NativeMethods.SetWindowPos(this.Handle, IntPtr.Zero, 0, 0, 0, 0, SWP_FRAMECHANGED | SWP_NOMOVE | SWP_NOSIZE);
    //    //this.Show();
    //}


    /// <summary>
    /// Gets the required creation parameters when the control handle is created.
    /// </summary>
    protected override CreateParams CreateParams
    {
        get
        {
            if (DesignMode)
            {
                return base.CreateParams;
            }

            var baseParams = base.CreateParams;
            base.CreateParams.Style = unchecked ((int)GetWindowStyle());

            if (Borderless)
            {
                InitializeShadow();

                if (!PlatformHelper.VistaOrHigher || !NativeMethods.DwmIsCompositionEnabled())
                {
                    baseParams.ClassStyle |= CS_DROPSHADOW;
                }
            }

            return baseParams;
        }
    }

    /// <summary>
    /// Processes Windows messages.
    /// </summary>
    /// <param name="m">The Windows <see cref="T:System.Windows.Forms.Message" /> to process.</param>
    protected override void WndProc (ref Message m)
    {
        if (!DesignMode)
        {
            switch (m.Msg)
            {
                case (int)WindowsMessages.NonClientCalcSize:
                    if (Borderless)
                    {
                        m.Result = IntPtr.Zero;
                        return;
                    }

                    break;
                case (int)WindowsMessages.NonClientHitTest:
                    if (Borderless)
                    {
                        m.Result = new IntPtr ((int)PerformHitTest (new Point (unchecked ((short)m.LParam.ToInt64()),
                            unchecked ((short)(m.LParam.ToInt64() >> 16)))));
                    }
                    else
                    {
                        base.WndProc (ref m);
                        if (AutoDrag && m.Result.ToInt32() == (int)HitTestResult.Client)
                        {
                            m.Result = new IntPtr ((int)HitTestResult.Caption);
                        }
                    }

                    return;
                case (int)WindowsMessages.NonClientActivate:
                    if (Borderless && (!PlatformHelper.VistaOrHigher || !NativeMethods.DwmIsCompositionEnabled()))
                    {
                        m.Result = new IntPtr (1);
                        return;
                    }

                    break;
                case (int)WindowsMessages.ThemeChanged:
                case (int)WindowsMessages.DwmCompositionChanged:
                    RecreateHandle();
                    break;
            }
        }

        base.WndProc (ref m);
    }

    /// <inheritdoc cref="Control.OnGotFocus"/>
    protected override void OnGotFocus (EventArgs e)
    {
        if (Borderless && (!PlatformHelper.VistaOrHigher || !NativeMethods.DwmIsCompositionEnabled()))
        {
            Invalidate();
        }

        base.OnGotFocus (e);
    }

    /// <inheritdoc cref="Control.OnLostFocus"/>
    protected override void OnLostFocus (EventArgs e)
    {
        if (Borderless && (!PlatformHelper.VistaOrHigher || !NativeMethods.DwmIsCompositionEnabled()))
        {
            Invalidate();
        }

        base.OnLostFocus (e);
    }


    /// <summary>
    ///
    /// </summary>
    [StructLayout (LayoutKind.Sequential)]
    protected internal struct Margins
    {
        /// <summary>
        ///
        /// </summary>
        public int LeftWidth;

        /// <summary>
        ///
        /// </summary>
        public int RightWidth;

        /// <summary>
        ///
        /// </summary>
        public int TopHeight;

        /// <summary>
        ///
        /// </summary>
        public int BottomHeight;
    }

    /// <summary>
    ///
    /// </summary>
    [Flags]
    protected internal enum WindowStyles
        : uint
    {
        /// <summary>
        ///
        /// </summary>
        Overlapped = 0,

        /// <summary>
        ///
        /// </summary>
        MaximizeBox = 0x10000,

        /// <summary>
        ///
        /// </summary>
        MinimizeBox = 0x20000,

        /// <summary>
        ///
        /// </summary>
        ThickFrame = 0x40000,

        /// <summary>
        ///
        /// </summary>
        SysMenu = 0x80000,

        /// <summary>
        ///
        /// </summary>
        Caption = 0xC00000,

        /// <summary>
        ///
        /// </summary>
        Popup = 0x80000000,
    }

    /// <summary>
    ///
    /// </summary>
    protected internal enum WindowsMessages
    {
        /// <summary>
        ///
        /// </summary>
        NonClientCalcSize = 0x83,

        /// <summary>
        ///
        /// </summary>
        NonClientHitTest = 0x84,

        /// <summary>
        ///
        /// </summary>
        NonClientActivate = 0x86,

        /// <summary>
        ///
        /// </summary>
        ThemeChanged = 0x31A,

        /// <summary>
        ///
        /// </summary>
        DwmCompositionChanged = 0x31E
    }
}

/// <summary>
///
/// </summary>
public enum HitTestResult
{
    /// <summary>
    ///
    /// </summary>
    Nowhere = 0,

    /// <summary>
    ///
    /// </summary>
    Client = 1,

    /// <summary>
    ///
    /// </summary>
    Caption = 2,

    /// <summary>
    ///
    /// </summary>
    Left = 10,

    /// <summary>
    ///
    /// </summary>
    Right = 11,

    /// <summary>
    ///
    /// </summary>
    Top = 12,

    /// <summary>
    ///
    /// </summary>
    TopLeft = 13,

    /// <summary>
    ///
    /// </summary>
    TopRight = 14,

    /// <summary>
    ///
    /// </summary>
    Bottom = 15,

    /// <summary>
    ///
    /// </summary>
    BottomLeft = 16,

    /// <summary>
    ///
    /// </summary>
    BottomRight = 17,

    /// <summary>
    ///
    /// </summary>
    Border = 18
}
