// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* EventedTextBox.cs -- дополнительные события для TextBox
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms;

/// <summary>
/// <see cref="TextBox"/> с дополнительными событиями.
/// </summary>
[System.ComponentModel.DesignerCategory ("Code")]
public sealed class EventedTextBox
    : TextBox
{
    #region Events

    /// <summary>
    /// Отложенная версия события <see cref="Control.TextChanged"/>.
    /// </summary>
    public event EventHandler? DelayedTextChanged;

    /// <summary>
    /// Событие, возникающее при нажатии на клавишу <c>Enter</c>.
    /// </summary>
    public event EventHandler? EnterPressed;

    #endregion

    #region Properties

    /// <summary>
    /// Величина задержки для <see cref="DelayedTextChanged"/>,
    /// миллисекунды.
    /// </summary>
    public int Delay
    {
        get => _delay;
        set
        {
            if (value < 1)
            {
                throw new ArgumentOutOfRangeException
                    (
                        nameof (value)
                    );
            }

            if (_timer is not null)
            {
                _timer.Enabled = false;
            }

            _delay = value;
            _count = 0;
        }
    }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public EventedTextBox()
    {
        _delay = 750;
        _timer = new Timer { Interval = 50 };
        _timer.Tick += _timer_Tick;
    }

    #endregion

    #region Private members

    private int _delay;
    private int _count;
    private Timer? _timer;

    /// <inheritdoc cref="TextBox.Dispose(bool)" />
    protected override void Dispose
        (
            bool disposing
        )
    {
        base.Dispose (disposing);

        if (disposing)
        {
            _timer?.Dispose();
        }

        _timer = null;
    }

    /// <inheritdoc />
    protected override void OnKeyDown
        (
            KeyEventArgs eventArgs
        )
    {
        base.OnKeyDown (eventArgs);

        if (eventArgs.KeyData == Keys.Enter)
        {
            if (_timer is not null)
            {
                _timer.Enabled = false;
            }

            _count = 0;

            EnterPressed?.Invoke
                (
                    this,
                    EventArgs.Empty
                );
        }
    }

    /// <inheritdoc cref="TextBoxBase.OnTextChanged" />
    protected override void OnTextChanged
        (
            EventArgs eventArgs
        )
    {
        _count = 0;
        if (_timer is not null)
        {
            _timer.Enabled = true;
        }
    }

    private void _timer_Tick
        (
            object? sender,
            EventArgs eventArgs
        )
    {
        if (_timer is not null)
        {
            _count += _timer.Interval;
        }

        if (_count >= _delay)
        {
            if (_timer is not null)
            {
                _timer.Enabled = false;
            }

            _count = 0;

            DelayedTextChanged?.Invoke
                (
                    this,
                    EventArgs.Empty
                );
        }
    }

    #endregion
}
