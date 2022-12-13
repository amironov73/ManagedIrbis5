// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* BusyStripe.cs -- беспокойно бегающая полоска
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading.Tasks;
using System.Windows.Forms;

using AM.Threading;

#endregion

#nullable enable

namespace AM.Windows.Forms;

/// <summary>
/// Беспокойно бегающая полоска, означающая занятость программы
/// каким-либо важным делом.
/// </summary>
[System.ComponentModel.DesignerCategory ("Code")]
[ToolboxBitmap (typeof (BusyStripe), "Images.BusyStripe.bmp")]
public class BusyStripe
    : Control
{
    #region Properties

    private bool _moving;

    /// <summary>
    /// Gets or sets a value indicating whether this
    /// <see cref="BusyStripe"/> is moving.
    /// </summary>
    /// <value><c>true</c> if moving; otherwise,
    /// <c>false</c>.</value>
    [System.ComponentModel.Category ("Behavior")]
    public bool Moving
    {
        get => _moving;
        set
        {
            _moving = value;
            if (_timer is not null)
            {
                _timer.Enabled = value;
            }
        }
    }

    #endregion

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    public BusyStripe()
    {
        ResizeRedraw = true;
        SetStyle (ControlStyles.OptimizedDoubleBuffer, true);
        SetStyle (ControlStyles.AllPaintingInWmPaint, true);

        _timer = new Timer
        {
            Interval = 100
        };
        _timer.Tick += _timer_Tick;
    }

    #endregion

    #region Private members

    private Timer? _timer;
    private float _position;
    private readonly float _speed = 0.05f;
    private bool _back;

    private void _timer_Tick
        (
            object? sender,
            EventArgs e
        )
    {
        if (Moving && Visible && !DesignMode)
        {
            _position += _speed;
            if (_position >= 0.95f)
            {
                _back = !_back;
                _position = 0.0f;
            }

            Invalidate();
        }
    }

    private void Busy_StateChanged
        (
            object? sender,
            EventArgs e
        )
    {
        var state = (BusyState?)sender;

        if (state is not null)
        {
            this.InvokeIfRequired
                (
                    () =>
                    {
                        Moving = state;
                        Invalidate();
                    }
                );
        }
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Run some code.
    /// </summary>
    public void Run
        (
            BusyState busyState,
            Action action
        )
    {
        busyState.StateChanged += Busy_StateChanged;
        try
        {
            busyState.Run (action);
        }
        finally
        {
            busyState.StateChanged -= Busy_StateChanged;
        }
    }

    /// <summary>
    /// Run some code in asychronous manner.
    /// </summary>
    public Task RunAsync
        (
            BusyState busyState,
            Action action
        )
    {
        var result = Task.Factory.StartNew
            (
                () => Run (busyState, action)
            );

        // .ConfigureSafe();

        return result;
    }


    /// <summary>
    /// Subscribe.
    /// </summary>
    public void SubscribeTo
        (
            BusyState busyState
        )
    {
        busyState.StateChanged += Busy_StateChanged;
    }

    /// <summary>
    /// Unsubscribe.
    /// </summary>
    public void UnsubscribeFrom
        (
            BusyState busyState
        )
    {
        busyState.StateChanged -= Busy_StateChanged;
    }

    #endregion

    #region Control members

    /// <inheritdoc cref="Control.Dispose(bool)" />
    protected override void Dispose (bool disposing)
    {
        if (disposing)
        {
            _timer?.Dispose();
            _timer = null;
        }

        base.Dispose (disposing);
    }

    /// <inheritdoc cref="Control.OnPaint" />
    protected override void OnPaint
        (
            PaintEventArgs e
        )
    {
        var g = e.Graphics;
        var r = ClientRectangle;
        r.X -= 2;
        r.Width += 4;

        if (Moving)
        {
            using (var brush
                   = new LinearGradientBrush
                       (
                           r,
                           BackColor,
                           ForeColor,
                           _back ? 0 : 180
                       ))
            {
                brush.SetBlendTriangularShape (_position, 0.5f);
                g.FillRectangle (brush, ClientRectangle);
            }

            if (!string.IsNullOrEmpty (Text))
            {
                using var brush = new SolidBrush (Color.Black);
                using var format = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };

                g.DrawString
                    (
                        Text,
                        Font,
                        brush,
                        ClientRectangle,
                        format
                    );
            }
        }
        else
        {
            using var brush = new SolidBrush (BackColor);
            g.FillRectangle (brush, ClientRectangle);
        }

        base.OnPaint (e);
    }

    #endregion
}
