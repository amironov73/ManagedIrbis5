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
    /// Init.
    /// </summary>
    public ControlAdapter (Control control)
        : base (WpfAdapter.Instance)
    {
        Sure.NotNull (control);

        Control = control;
    }

    /// <summary>
    /// Get the underline WPF control
    /// </summary>
    public Control Control { get; }

    public override RPoint MouseLocation => Utils.Convert (Control.PointFromScreen (Mouse.GetPosition (Control)));

    public override bool LeftMouseButton => Mouse.LeftButton == MouseButtonState.Pressed;

    public override bool RightMouseButton => Mouse.RightButton == MouseButtonState.Pressed;

    public override void SetCursorDefault()
    {
        Control.Cursor = Cursors.Arrow;
    }

    public override void SetCursorHand()
    {
        Control.Cursor = Cursors.Hand;
    }

    public override void SetCursorIBeam()
    {
        Control.Cursor = Cursors.IBeam;
    }

    public override void DoDragDropCopy (object dragDropData)
    {
        DragDrop.DoDragDrop (Control, dragDropData, DragDropEffects.Copy);
    }

    public override void MeasureString
        (
            string str,
            RFont font,
            double maxWidth,
            out int charFit,
            out double charFitWidth
        )
    {
        using var adapter = new GraphicsAdapter();
        adapter.MeasureString (str, font, maxWidth, out charFit, out charFitWidth);
    }

    public override void Invalidate()
    {
        Control.InvalidateVisual();
    }
}
