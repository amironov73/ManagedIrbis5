// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global
// ReSharper disable VirtualMemberCallInConstructor

/* AnimationController.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms.Animation;

/// <summary>
/// DoubleBitmap displays animation
/// </summary>
public class AnimationController
{
    #region Events

    /// <summary>
    ///
    /// </summary>
    public event EventHandler<TransfromNeededEventArg>? TransformNeeded;

    /// <summary>
    ///
    /// </summary>
    public event EventHandler<NonLinearTransfromNeededEventArg>? NonLinearTransformNeeded;

    /// <summary>
    ///
    /// </summary>
    public event EventHandler<PaintEventArgs>? FramePainting;

    /// <summary>
    ///
    /// </summary>
    public event EventHandler<PaintEventArgs>? FramePainted;

    /// <summary>
    ///
    /// </summary>
    public event EventHandler<MouseEventArgs>? MouseDown;

    #endregion

    #region Properties

    /// <summary>
    ///
    /// </summary>
    public float CurrentTime { get; private set; }

    /// <summary>
    ///
    /// </summary>
    public Control DoubleBitmap { get; }

    /// <summary>
    ///
    /// </summary>
    public Control AnimatedControl { get; set; }

    /// <summary>
    ///
    /// </summary>
    public bool IsCompleted =>
        TimeStep >= 0f && CurrentTime >= _animation.MaxTime
        || TimeStep <= 0f && CurrentTime <= _animation.MinTime;

    /// <summary>
    ///
    /// </summary>
    public Bitmap? Frame
    {
        get => (DoubleBitmap as IFakeControl)!.Frame;
        set => (DoubleBitmap as IFakeControl)!.Frame = value;
    }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public AnimationController
        (
            Control control,
            AnimateMode animationMode,
            Animation animation,
            float timeStep,
            Rectangle controlClipRect
        )
    {
        Sure.NotNull (control);

        DoubleBitmap = new DoubleBitmapControl();
        var fakeControl = (IFakeControl)DoubleBitmap;
        fakeControl.FramePainting += OnFramePainting;
        fakeControl.FramePainted += OnFramePainting;
        fakeControl.TransformNeeded += OnTransformNeeded;
        DoubleBitmap.MouseDown += OnMouseDown;

        _animation = animation;
        AnimatedControl = control;
        _animationMode = animationMode;
        clipRect = controlClipRect == default
            ? new Rectangle (Point.Empty, GetBounds().Size)
            : ControlRectToMyRect (controlClipRect);

        if (animationMode == AnimateMode.Show || animationMode == AnimateMode.BeginUpdate)
        {
            timeStep = -timeStep;
        }

        TimeStep = timeStep * (animation.TimeCoefficient == 0f ? 1f : animation.TimeCoefficient);
        if (TimeStep == 0f)
            timeStep = 0.01f;

        switch (animationMode)
        {
            case AnimateMode.Hide:
                BgBmp = GetBackground (control);
                fakeControl.InitParent (control, animation.Padding);
                ctrlBmp = GetForeground (control);
                DoubleBitmap.Visible = true;
                control.Visible = false;
                break;

            case AnimateMode.Show:
                BgBmp = GetBackground (control);
                fakeControl.InitParent (control, animation.Padding);
                DoubleBitmap.Visible = true;
                DoubleBitmap.Refresh();
                control.Visible = true;
                ctrlBmp = GetForeground (control);
                break;

            case AnimateMode.BeginUpdate:
            case AnimateMode.Update:
                fakeControl.InitParent (control, animation.Padding);
                BgBmp = GetBackground (control, true);
                DoubleBitmap.Visible = true;
                break;
        }

        CurrentTime = timeStep > 0 ? animation.MinTime : animation.MaxTime;
    }

    #endregion

    #region Private members

    /// <summary>
    ///
    /// </summary>
    protected Bitmap? BgBmp
    {
        get => (DoubleBitmap as IFakeControl)?.BgBmp;
        set => (DoubleBitmap as IFakeControl)!.BgBmp = value;
    }

    /// <summary>
    ///
    /// </summary>
    protected Bitmap ctrlBmp;

    /// <summary>
    ///
    /// </summary>
    protected float TimeStep { get; }

    private Point[] _buffer;

    private byte[] _pixelsBuffer;

    /// <summary>
    ///
    /// </summary>
    protected Rectangle clipRect;

    private AnimateMode _animationMode;

    private readonly Animation _animation;

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    protected virtual Rectangle GetBounds()
    {
        return new Rectangle
            (
                AnimatedControl.Left - _animation.Padding.Left,
                AnimatedControl.Top - _animation.Padding.Top,
                AnimatedControl.Size.Width + _animation.Padding.Left + _animation.Padding.Right,
                AnimatedControl.Size.Height + _animation.Padding.Top + _animation.Padding.Bottom
            );
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="rect"></param>
    /// <returns></returns>
    protected virtual Rectangle ControlRectToMyRect
        (
            Rectangle rect
        )
    {
        return new Rectangle (
            _animation.Padding.Left + rect.Left,
            _animation.Padding.Top + rect.Top,
            rect.Width + _animation.Padding.Left + _animation.Padding.Right,
            rect.Height + _animation.Padding.Top + _animation.Padding.Bottom);
    }

    /// <summary>
    ///
    /// </summary>
    protected virtual void OnMouseDown
        (
            object? sender,
            MouseEventArgs e
        )
    {
        MouseDown?.Invoke (this, e);
    }

    /// <summary>
    ///
    /// </summary>
    protected virtual void OnFramePainting
        (
            object? sender,
            PaintEventArgs e
        )
    {
        Frame?.Dispose();
        Frame = null;

        if (_animationMode == AnimateMode.BeginUpdate)
        {
            return;
        }

        Frame = OnNonLinearTransformNeeded();

        var time = CurrentTime + TimeStep;
        if (time > _animation.MaxTime) time = _animation.MaxTime;
        if (time < _animation.MinTime) time = _animation.MinTime;
        CurrentTime = time;

        FramePainting?.Invoke (this, e);
    }

    /// <summary>
    ///
    /// </summary>
    protected virtual void OnFramePainted
        (
            object? sender,
            PaintEventArgs e
        )
    {
        FramePainted?.Invoke (this, e);
    }

    #endregion

    /// <summary>
    ///
    /// </summary>
    protected virtual Bitmap GetBackground
        (
            Control control,
            bool includeForeground = false,
            bool clip = false
        )
    {
        if (control is Form)
        {
            return GetScreenBackground (control, includeForeground, clip);
        }

        var bounds = GetBounds();
        var w = bounds.Width;
        var h = bounds.Height;
        if (w == 0) w = 1;
        if (h == 1) h = 1;
        var bmp = new Bitmap (w, h);

        var clientRect = new Rectangle (0, 0, bmp.Width, bmp.Height);
        var ea = new PaintEventArgs (Graphics.FromImage (bmp), clientRect);
        if (clip)
            ea.Graphics.SetClip (clipRect);

        for (var i = control.Parent.Controls.Count - 1; i >= 0; i--)
        {
            var c = control.Parent.Controls[i];
            if (c == control && !includeForeground) break;
            if (c.Visible && !c.IsDisposed)
                if (c.Bounds.IntersectsWith (bounds))
                {
                    using (var cb = new Bitmap (c.Width, c.Height))
                    {
                        c.DrawToBitmap (cb, new Rectangle (0, 0, c.Width, c.Height));
                        /*if (c == ctrl)
                            ea.Graphics.SetClip(clipRect);*/
                        ea.Graphics.DrawImage (cb, c.Left - bounds.Left, c.Top - bounds.Top, c.Width, c.Height);
                    }
                }

            if (c == control) break;
        }

        ea.Graphics.Dispose();

        return bmp;
    }

    private Bitmap GetScreenBackground
        (
            Control control,
            bool includeForeground,
            bool clip
        )
    {
        control.NotUsed();
        includeForeground.NotUsed();
        clip.NotUsed();

        var size = Screen.PrimaryScreen.Bounds.Size;
        var temp = DoubleBitmap.CreateGraphics(); //???
        var bmp = new Bitmap (size.Width, size.Height, temp);
        var gr = Graphics.FromImage (bmp);
        gr.CopyFromScreen (0, 0, 0, 0, size);
        return bmp;
    }

    /// <summary>
    ///
    /// </summary>
    protected virtual Bitmap GetForeground
        (
            Control control
        )
    {
        var bmp = new Bitmap (DoubleBitmap.Width, DoubleBitmap.Height);

        if (!control.IsDisposed)
        {
            if (DoubleBitmap.Parent == null)
            {
                control.DrawToBitmap
                    (
                        bmp,
                        new Rectangle (_animation.Padding.Left, _animation.Padding.Top, control.Width, control.Height)
                    );
            }
            else
            {
                control.DrawToBitmap
                    (
                        bmp,
                        new Rectangle (control.Left - DoubleBitmap.Left, control.Top - DoubleBitmap.Top, control.Width, control.Height)
                    );
            }
        }

        return bmp;
    }

    /// <summary>
    ///
    /// </summary>
    protected virtual void OnTransformNeeded
        (
            object? sender,
            TransfromNeededEventArg e
        )
    {
        try
        {
            e.ClipRectangle = clipRect;
            e.CurrentTime = CurrentTime;

            if (TransformNeeded != null)
            {
                TransformNeeded (this, e);
            }
            else
            {
                e.UseDefaultMatrix = true;
            }

            if (e.UseDefaultMatrix)
            {
                TransformHelper.DoScale (e, _animation);
                TransformHelper.DoRotate (e, _animation);
                TransformHelper.DoSlide (e, _animation);
            }
        }
        catch (Exception exception)
        {
            Debug.WriteLine (exception.Message);
        }
    }

    /// <summary>
    ///
    /// </summary>
    protected virtual Bitmap? OnNonLinearTransformNeeded()
    {
        Bitmap? bmp = null;
        if (ctrlBmp == null)
        {
            return null;
        }

        try
        {
            bmp = (Bitmap)ctrlBmp.Clone();

            const int bytesPerPixel = 4;
            var pxf = PixelFormat.Format32bppArgb;
            var rect = new Rectangle (0, 0, bmp.Width, bmp.Height);
            var bmpData = bmp.LockBits (rect, ImageLockMode.ReadWrite, pxf);
            var ptr = bmpData.Scan0;
            var numBytes = bmp.Width * bmp.Height * bytesPerPixel;
            var argbValues = new byte[numBytes];

            System.Runtime.InteropServices.Marshal.Copy (ptr, argbValues, 0, numBytes);

            var e = new NonLinearTransfromNeededEventArg()
            {
                CurrentTime = CurrentTime, ClientRectangle = DoubleBitmap.ClientRectangle, Pixels = argbValues,
                Stride = bmpData.Stride
            };

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
                TransformHelper.DoBlind (e, _animation);
                TransformHelper.DoMosaic (e, _animation, ref _buffer, ref _pixelsBuffer);

                TransformHelper.DoTransparent (e, _animation);
                TransformHelper.DoLeaf (e, _animation);
            }

            System.Runtime.InteropServices.Marshal.Copy (argbValues, 0, ptr, numBytes);
            bmp.UnlockBits (bmpData);
        }
        catch (Exception exception)
        {
            Debug.WriteLine (exception.Message);
        }

        return bmp;
    }

    internal void BuildNextFrame()
    {
        DoubleBitmap.Invalidate();
    }

    #region Public methods

    /// <summary>
    ///
    /// </summary>
    public void Hide()
    {
        if (DoubleBitmap != null)
            try
            {
                DoubleBitmap.BeginInvoke (new MethodInvoker (() =>
                {
                    if (DoubleBitmap.Visible) DoubleBitmap.Hide();
                    DoubleBitmap.Parent = null;

                    //DoubleBitmap.Dispose();
                }));
            }
            catch
            {
            }
    }

    /// <summary>
    ///
    /// </summary>
    public void EndUpdate()
    {
        var bmp = GetBackground (AnimatedControl, true, true);
        if (_animation.AnimateOnlyDifferences)
        {
            TransformHelper.CalcDifference (bmp, BgBmp);
        }

        ctrlBmp = bmp;
        _animationMode = AnimateMode.Update;
    }

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public void Dispose()
    {
        if (ctrlBmp != null)
        {
            BgBmp.Dispose();
        }

        if (ctrlBmp != null)
        {
            ctrlBmp.Dispose();
        }

        if (Frame != null)
        {
            Frame.Dispose();
        }

        AnimatedControl = null;

        Hide();
    }

    #endregion
}
