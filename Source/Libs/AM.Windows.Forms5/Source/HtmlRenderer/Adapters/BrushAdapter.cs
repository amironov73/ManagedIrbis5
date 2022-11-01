// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

#region Using directives

using System.Drawing;

using AM.Drawing.HtmlRenderer.Adapters;

#endregion

#nullable enable

namespace AM.Windows.Forms.HtmlRenderer.Adapters;

/// <summary>
/// Adapter for WinForms brushes objects for core.
/// </summary>
internal sealed class BrushAdapter
    : RBrush
{
    #region Properties

    /// <summary>
    /// The actual WinForms brush instance.
    /// </summary>
    public Brush Brush { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Init.
    /// </summary>
    public BrushAdapter
        (
            Brush brush,
            bool dispose
        )
    {
        Sure.NotNull (brush);

        Brush = brush;
        _dispose = dispose;
    }

    #endregion

    #region Private members

    /// <summary>
    /// If to dispose the brush when <see cref="Dispose"/> is called.<br/>
    /// Ignore dispose for cached brushes.
    /// </summary>
    private readonly bool _dispose;

    #endregion

    #region RBrush members

    /// <inheritdoc cref="RBrush.Dispose"/>
    public override void Dispose()
    {
        if (_dispose)
        {
            Brush.Dispose();
        }
    }

    #endregion
}
