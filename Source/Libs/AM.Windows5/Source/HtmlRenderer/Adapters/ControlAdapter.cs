// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* ControlAdapter.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using AM.Drawing.HtmlRenderer.Adapters;
using AM.Drawing.HtmlRenderer.Adapters.Entities;
using AM.Drawing.HtmlRenderer.Core.Utils;
using AM.Windows.HtmlRenderer.Utilities;

#endregion

#nullable enable

namespace AM.Windows.HtmlRenderer.Adapters;

/// <summary>
/// Adapter for WPF Control for core.
/// </summary>
internal sealed class ControlAdapter 
    : RControl
{
    /// <summary>
    /// the underline WPF control.
    /// </summary>
    private readonly Control _control;

    /// <summary>
    /// Init.
    /// </summary>
    public ControlAdapter(Control control)
        : base(WpfAdapter.Instance)
    {
        ArgChecker.AssertArgNotNull(control, "control");

        _control = control;
    }

    /// <summary>
    /// Get the underline WPF control
    /// </summary>
    public Control Control
    {
        get { return _control; }
    }

    public override RPoint MouseLocation
    {
        get { return Utils.Convert(_control.PointFromScreen(Mouse.GetPosition(_control))); }
    }

    public override bool LeftMouseButton
    {
        get { return Mouse.LeftButton == MouseButtonState.Pressed; }
    }

    public override bool RightMouseButton
    {
        get { return Mouse.RightButton == MouseButtonState.Pressed; }
    }

    public override void SetCursorDefault()
    {
        _control.Cursor = Cursors.Arrow;
    }

    public override void SetCursorHand()
    {
        _control.Cursor = Cursors.Hand;
    }

    public override void SetCursorIBeam()
    {
        _control.Cursor = Cursors.IBeam;
    }

    public override void DoDragDropCopy(object dragDropData)
    {
        DragDrop.DoDragDrop(_control, dragDropData, DragDropEffects.Copy);
    }

    public override void MeasureString(string str, RFont font, double maxWidth, out int charFit, out double charFitWidth)
    {
        using (var g = new GraphicsAdapter())
        {
            g.MeasureString(str, font, maxWidth, out charFit, out charFitWidth);
        }
    }

    public override void Invalidate()
    {
        _control.InvalidateVisual();
    }
}
