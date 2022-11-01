// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

#region Using directives

using System.Windows.Forms;

using AM.Drawing.HtmlRenderer.Adapters.Entities;
using AM.Drawing.HtmlRenderer.Adapters;
using AM.Windows.Forms.HtmlRenderer.Utilities;

#endregion

#nullable enable

namespace AM.Windows.Forms.HtmlRenderer.Adapters;

/// <summary>
/// Adapter for WinForms Control for core.
/// </summary>
internal sealed class ControlAdapter
    : RControl
{
    #region Properties

    /// <summary>
    /// Get the underline win forms control
    /// </summary>
    public Control Control { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ControlAdapter
        (
            Control control,
            bool useGdiPlusTextRendering
        )
        : base (WinFormsAdapter.Instance)
    {
        Sure.NotNull (control);

        Control = control;
        _useGdiPlusTextRendering = useGdiPlusTextRendering;
    }

    #endregion

    #region Private members

    /// <summary>
    /// Use GDI+ text rendering to measure/draw text.
    /// </summary>
    private readonly bool _useGdiPlusTextRendering;

    #endregion

    #region RControl members

    /// <inheritdoc cref="RControl.MouseLocation"/>
    public override RPoint MouseLocation =>
        Utils.Convert (Control.PointToClient (Control.MousePosition));

    /// <inheritdoc cref="RControl.LeftMouseButton"/>
    public override bool LeftMouseButton =>
        (Control.MouseButtons & MouseButtons.Left) != 0;

    /// <inheritdoc cref="RControl.RightMouseButton"/>
    public override bool RightMouseButton =>
        (Control.MouseButtons & MouseButtons.Right) != 0;

    /// <inheritdoc cref="RControl.SetCursorDefault"/>
    public override void SetCursorDefault()
    {
        Control.Cursor = Cursors.Default;
    }

    /// <inheritdoc cref="RControl.SetCursorHand"/>
    public override void SetCursorHand()
    {
        Control.Cursor = Cursors.Hand;
    }

    /// <inheritdoc cref="RControl.SetCursorIBeam"/>
    public override void SetCursorIBeam()
    {
        Control.Cursor = Cursors.IBeam;
    }

    /// <inheritdoc cref="RControl.DoDragDropCopy"/>
    public override void DoDragDropCopy
        (
            object dragDropData
        )
    {
        Control.DoDragDrop (dragDropData, DragDropEffects.Copy);
    }

    /// <inheritdoc cref="RControl.MeasureString"/>
    public override void MeasureString
        (
            string str,
            RFont font,
            double maxWidth,
            out int charFit,
            out double charFitWidth
        )
    {
        using var graphicsAdapter = new GraphicsAdapter
            (
                Control.CreateGraphics(),
                _useGdiPlusTextRendering,
                true
            );
        graphicsAdapter.MeasureString (str, font, maxWidth, out charFit, out charFitWidth);
    }

    /// <inheritdoc cref="RControl.Invalidate"/>
    public override void Invalidate()
    {
        Control.Invalidate();
    }

    #endregion
}
