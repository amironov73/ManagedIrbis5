// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

using System.Drawing;
using AM.Drawing.HtmlRenderer.Adapters;

namespace AM.Windows.Forms.HtmlRenderer.Adapters;

/// <summary>
/// Adapter for WinForms brushes objects for core.
/// </summary>
internal sealed class BrushAdapter : RBrush
{
    /// <summary>
    /// The actual WinForms brush instance.
    /// </summary>
    private readonly Brush _brush;

    /// <summary>
    /// If to dispose the brush when <see cref="Dispose"/> is called.<br/>
    /// Ignore dispose for cached brushes.
    /// </summary>
    private readonly bool _dispose;

    /// <summary>
    /// Init.
    /// </summary>
    public BrushAdapter(Brush brush, bool dispose)
    {
        _brush = brush;
        _dispose = dispose;
    }

    /// <summary>
    /// The actual WinForms brush instance.
    /// </summary>
    public Brush Brush
    {
        get { return _brush; }
    }

    public override void Dispose()
    {
        if (_dispose)
        {
            _brush.Dispose();
        }
    }
}