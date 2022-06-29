// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* BrushAdapter.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM.Drawing.HtmlRenderer.Adapters;

using PdfSharpCore.Drawing;

#endregion

#nullable enable

namespace AM.Drawing.HtmlRenderer.PdfSharp.Adapters;

/// <summary>
/// Adapter for WinForms brushes objects for core.
/// </summary>
internal sealed class BrushAdapter
    : RBrush
{
    /// <summary>
    /// The actual PdfSharp brush instance.<br/>
    /// Should be <see cref="XBrush"/> but there is some fucking issue inheriting from it =/
    /// </summary>
    private readonly Object _brush;

    /// <summary>
    /// Init.
    /// </summary>
    public BrushAdapter(Object brush)
    {
        _brush = brush;
    }

    /// <summary>
    /// The actual WinForms brush instance.
    /// </summary>
    public Object Brush
    {
        get { return _brush; }
    }

    public override void Dispose()
    { }
}
