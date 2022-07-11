// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable LocalizableElement
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PeriodicStatusLabel.cs -- текстовый элемент статусной строки, выполняющий периодические действия
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Windows.Forms;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace AM.Windows.Forms.AppServices;

/// <summary>
/// Текстовый элемент статусной строки, выполняющий некие
/// периодические действия.
/// </summary>
public class PeriodicStatusLabel
    : StatusLabel
{
    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public PeriodicStatusLabel()
    {
        _Initialize();
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public PeriodicStatusLabel
        (
            string? text
        )
        : base (text)
    {
        _Initialize();
    }

    #endregion

    #region Private members

    private Timer _timer = null!;

    private Action<PeriodicStatusLabel>? _action;

    private void _Initialize()
    {
        _timer = new Timer
        {
            Interval = 1000,
            Enabled = true
        };

        _timer.Tick += _InvokeAction;
    }

    private void _InvokeAction
        (
            object? sender,
            EventArgs eventArgs
        )
    {
        try
        {
            _action?.Invoke (this);
        }
        catch (Exception exception)
        {
            Magna.Logger.LogError (exception, nameof (PeriodicStatusLabel));
        }
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Задание периодического действия.
    /// </summary>
    public void SetAction
        (
            Action<PeriodicStatusLabel> action
        )
    {
        Sure.NotNull (action);

        _action = action;
    }

    #endregion

    #region ToolStripItem members

    /// <inheritdoc cref="ToolStripItem.Dispose(bool)"/>
    protected override void Dispose
        (
            bool disposing
        )
    {
        _timer.Tick -= _InvokeAction;
        base.Dispose (disposing);
    }

    #endregion
}
