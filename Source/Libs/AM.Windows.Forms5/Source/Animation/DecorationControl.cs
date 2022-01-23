// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* DecorationControl.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms.Animation;

internal class DecorationControl
    : UserControl
{
    #region Events

    /// <summary>
    ///
    /// </summary>
    public event EventHandler<NonLinearTransfromNeededEventArg>? NonLinearTransformNeeded;

    #endregion

    #region Properties

    /// <summary>
    ///
    /// </summary>
    public DecorationType DecorationType { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Control DecoratedControl { get; set; }

    /// <summary>
    ///
    /// </summary>
    public new Padding Padding { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Bitmap CtrlBmp { get; set; }

    /// <summary>
    ///
    /// </summary>
    public byte[] CtrlPixels { get; set; }

    /// <summary>
    ///
    /// </summary>
    public int CtrlStride { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Bitmap Frame { get; set; }

    /// <summary>
    ///
    /// </summary>
    public float CurrentTime { get; set; }

    #endregion

    #region Construction

    /// <summary>
    ///
    /// </summary>
    public DecorationControl
        (
            DecorationType type,
            Control decoratedControl
        )
    {
        DecorationType = type;
        DecoratedControl = decoratedControl;

        decoratedControl.VisibleChanged += control_VisibleChanged;
        decoratedControl.ParentChanged += control_VisibleChanged;
        decoratedControl.LocationChanged += control_VisibleChanged;

        decoratedControl.Paint += decoratedControl_Paint;

        SetStyle (ControlStyles.Selectable, false);
        SetStyle (ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint,
            true);

        //BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        InitPadding();

        _timer = new Timer();
        _timer.Interval = 100;
        _timer.Tick += _timer_Tick;
        _timer.Enabled = true;
    }

    #endregion

    #region Private members

    private readonly Timer _timer;

    private bool _isSnapshotNow;

    private void InitPadding()
    {
        switch (DecorationType)
        {
            case DecorationType.BottomMirror:
                Padding = new Padding (0, 0, 0, 20);
                break;
        }
    }

    private void _timer_Tick (object sender, EventArgs e)
    {
        switch (DecorationType)
        {
            case DecorationType.BottomMirror:
            case DecorationType.Custom:
                Invalidate();
                break;
        }
    }

    private void decoratedControl_Paint
        (
            object? sender,
            PaintEventArgs e
        )
    {
        if (!_isSnapshotNow)
        {
            /*
            if (Frame != null)
            {
                e.Graphics.DrawImage(Frame, new Point(-Padding.Left, -Padding.Top));
                wasDraw = true;
            }*/
            /*
            CtrlBmp = GetForeground(DecoratedControl);
            CtrlPixels = GetPixels(CtrlBmp);*/ /*does not work for TextBox*/
            //wasRepainted = true;
            Invalidate();
        }
    }

    /// <inheritdoc cref="Control.OnPaint"/>
    protected override void OnPaint
        (
            PaintEventArgs e
        )
    {
        CtrlBmp = GetForeground (DecoratedControl);
        CtrlPixels = GetPixels (CtrlBmp);

        if (Frame != null)
        {
            Frame.Dispose();
        }

        Frame = OnNonLinearTransformNeeded();
        if (Frame != null)
        {
            e.Graphics.DrawImage (Frame, Point.Empty);
        }
    }

    private void control_VisibleChanged
        (
            object? sender,
            EventArgs e
        )
    {
        Init();
    }

    private void Init()
    {
        Parent = DecoratedControl.Parent;
        Visible = DecoratedControl.Visible;
        Location = new Point (DecoratedControl.Left - Padding.Left, DecoratedControl.Top - Padding.Top);


        if (Parent != null)
        {
            var i = Parent.Controls.GetChildIndex (DecoratedControl);
            Parent.Controls.SetChildIndex (this, i + 1);
        }

        var newSize = new Size (DecoratedControl.Width + Padding.Left + Padding.Right,
            DecoratedControl.Height + Padding.Top + Padding.Bottom);
        if (newSize != Size)
        {
            Size = newSize;
        }
    }

    protected virtual Bitmap GetForeground (Control ctrl)
    {
        Bitmap bmp = new Bitmap (Width, Height);

        if (!ctrl.IsDisposed)
        {
            _isSnapshotNow = true;
            ctrl.DrawToBitmap (bmp, new Rectangle (Padding.Left, Padding.Top, ctrl.Width, ctrl.Height));
            _isSnapshotNow = false;
        }

        return bmp;
    }

    byte[] GetPixels (Bitmap bmp)
    {
        const int bytesPerPixel = 4;
        PixelFormat pxf = PixelFormat.Format32bppArgb;
        Rectangle rect = new Rectangle (0, 0, bmp.Width, bmp.Height);
        BitmapData bmpData = bmp.LockBits (rect, ImageLockMode.ReadOnly, pxf);
        IntPtr ptr = bmpData.Scan0;
        int numBytes = bmp.Width * bmp.Height * bytesPerPixel;
        byte[] argbValues = new byte[numBytes];
        Marshal.Copy (ptr, argbValues, 0, numBytes);

        //Marshal.Copy(argbValues, 0, ptr, numBytes);
        bmp.UnlockBits (bmpData);
        return argbValues;
    }

    protected virtual Bitmap? OnNonLinearTransformNeeded()
    {
        Bitmap? bmp = null;
        if (CtrlBmp == null)
        {
            return null;
        }

        try
        {
            bmp = new Bitmap (Width, Height);

            const int bytesPerPixel = 4;
            PixelFormat pxf = PixelFormat.Format32bppArgb;
            Rectangle rect = new Rectangle (0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits (rect, ImageLockMode.ReadWrite, pxf);
            IntPtr ptr = bmpData.Scan0;
            int numBytes = bmp.Width * bmp.Height * bytesPerPixel;
            byte[] argbValues = new byte[numBytes];

            Marshal.Copy (ptr, argbValues, 0, numBytes);

            var e = new NonLinearTransfromNeededEventArg()
            {
                CurrentTime = CurrentTime, ClientRectangle = ClientRectangle, Pixels = argbValues,
                Stride = bmpData.Stride, SourcePixels = CtrlPixels,
                SourceClientRectangle = new Rectangle
                    (
                        Padding.Left,
                        Padding.Top,
                        DecoratedControl.Width,
                        DecoratedControl.Height
                    ),
                SourceStride = CtrlStride
            };

            try
            {
                if (NonLinearTransformNeeded != null)
                {
                    NonLinearTransformNeeded (this, e);
                }
                else
                {
                    e.UseDefaultTransform = true;
                }

                if (e.UseDefaultTransform)
                {
                    switch (DecorationType)
                    {
                        case DecorationType.BottomMirror:
                            TransformHelper.DoBottomMirror (e);
                            break;
                    }
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine (exception.Message);
            }

            Marshal.Copy (argbValues, 0, ptr, numBytes);
            bmp.UnlockBits (bmpData);
        }
        catch (Exception exception)
        {
            Debug.WriteLine (exception.Message);
        }

        return bmp;
    }

    /// <inheritdoc cref="ContainerControl.Dispose(bool)"/>
    protected override void Dispose
        (
            bool disposing
        )
    {
        _timer.Stop();
        _timer.Dispose();
        base.Dispose (disposing);
    }

    #endregion
}
