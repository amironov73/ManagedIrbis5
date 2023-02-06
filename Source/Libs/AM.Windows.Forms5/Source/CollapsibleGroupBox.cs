// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* CollapsibleGroupBox.cs -- GroupBox, который можно свернуть
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms;

/// <summary>
/// <see cref="GroupBox"/>, который можно свернуть.
/// </summary>
[System.ComponentModel.DesignerCategory ("Code")]
public class CollapsibleGroupBox
    : GroupBox
{
    #region Events

    /// <summary>
    /// Возникает при свертывании.
    /// </summary>
    public event EventHandler? Collapse;

    #endregion

    #region Properties

    private bool _collapsed;

    /// <summary>
    /// Gets or sets a value indicating whether this
    /// <see cref="CollapsibleGroupBox"/> is collapsed.
    /// </summary>
    /// <value><c>true</c> if collapsed;
    /// otherwise, <c>false</c>.</value>
    [System.ComponentModel.DefaultValue (false)]
    public bool Collapsed
    {
        get => _collapsed;
        set
        {
            if (_collapsed != value)
            {
                _Collapse (value);
            }
        }
    }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор без параметров.
    /// </summary>
    public CollapsibleGroupBox()
    {
        _savedState = new Dictionary<Control, bool>();
        MouseDoubleClick += _MouseDoubleClick;
    }

    #endregion

    #region Private members

    private int _savedHeight;
    private readonly Dictionary<Control, bool> _savedState;

    private void _Collapse 
        (
            bool collapse
        )
    {
        if (collapse)
        {
            _savedState.Clear();
            _savedHeight = Height;
            Height = FontHeight + 2;
        }
        else
        {
            Height = _savedHeight;
        }

        foreach (Control control in Controls)
        {
            if (collapse)
            {
                _savedState.Add (control, control.Enabled);
                control.Enabled = false;
            }
            else
            {
                control.Enabled = !_savedState.ContainsKey (control) || _savedState[control];
            }
        }

        _collapsed = collapse;

        Collapse?.Invoke (this, EventArgs.Empty);
    }

    private void _MouseDoubleClick
        (
            object? sender,
            MouseEventArgs e
        )
    {
        if (e.Y < FontHeight)
        {
            _Collapse (!Collapsed);
        }
    }

    #endregion

    #region Control members

    /// <inheritdoc cref="Control.OnPaintBackground"/>
    protected override void OnPaintBackground
        (
            PaintEventArgs paintEvent
        )
    {
        base.OnPaintBackground (paintEvent);
        var g = paintEvent.Graphics;
        var rectangle = ClientRectangle;
        rectangle.Height = FontHeight + 2;
        using var b = new LinearGradientBrush
            (
                rectangle,
                SystemColors.ControlDark,
                SystemColors.ControlLight,
                0f
            );
        g.FillRectangle (b, rectangle);
    }

    #endregion
}
