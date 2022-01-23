// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* DoubleBitmapControl.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms.Animation;

/// <summary>
///
/// </summary>
public partial class DoubleBitmapControl
    : Control, IFakeControl
{
    #region Events

    /// <summary>
    ///
    /// </summary>
    public event EventHandler<TransfromNeededEventArg>? TransformNeeded;

    /// <summary>
    ///
    /// </summary>
    public event EventHandler<PaintEventArgs>? FramePainted;

    /// <summary>
    ///
    /// </summary>
    public event EventHandler<PaintEventArgs>? FramePainting;

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public DoubleBitmapControl()
    {
        InitializeComponent();

        Visible = false;
        SetStyle (ControlStyles.Selectable, false);
        SetStyle
            (
                ControlStyles.AllPaintingInWmPaint
                | ControlStyles.OptimizedDoubleBuffer
                | ControlStyles.UserPaint,
                true
            );
    }

    #endregion

    #region Private members

    Bitmap? IFakeControl.BgBmp { get; set; }

    Bitmap? IFakeControl.Frame { get; set; }

    /// <inheritdoc cref="Control.OnPaint"/>
    protected override void OnPaint
        (
            PaintEventArgs e
        )
    {
        var graphics = e.Graphics;

        OnFramePainting (e);

        try
        {
            var bgBmp = ((IFakeControl)this).BgBmp;
            if (bgBmp != null)
            {
                graphics.DrawImage (bgBmp, 0, 0);
            }

            if (((IFakeControl)this).Frame != null)
            {
                var ea = new TransfromNeededEventArg()
                {
                    ClientRectangle = new Rectangle (0, 0, Width, Height)
                };
                OnTransformNeeded (ea);
                graphics.SetClip (ea.ClipRectangle);
                graphics.Transform = ea.Matrix;
                graphics.DrawImage (((IFakeControl)this).Frame, 0, 0);
            }
        }
        catch (Exception exception)
        {
            Debug.WriteLine (exception.Message);
        }

        OnFramePainted (e);
    }


    private void OnTransformNeeded (TransfromNeededEventArg ea)
    {
        TransformNeeded?.Invoke (this, ea);
    }

    /// <summary>
    ///
    /// </summary>
    protected virtual void OnFramePainting (PaintEventArgs e)
    {
        FramePainting?.Invoke (this, e);
    }

    /// <summary>
    ///
    /// </summary>
    protected virtual void OnFramePainted
        (
            PaintEventArgs e
        )
    {
        FramePainted?.Invoke (this, e);
    }

    #endregion

    #region Public methods

    /// <summary>
    ///
    /// </summary>
    public void InitParent
        (
            Control control,
            Padding padding
        )
    {
        Sure.NotNull (control);

        Parent = control.Parent;
        var i = control.Parent.Controls.GetChildIndex (control);
        control.Parent.Controls.SetChildIndex (this, i);
        Bounds = new Rectangle (
            control.Left - padding.Left,
            control.Top - padding.Top,
            control.Size.Width + padding.Left + padding.Right,
            control.Size.Height + padding.Top + padding.Bottom);
    }

    #endregion
}
