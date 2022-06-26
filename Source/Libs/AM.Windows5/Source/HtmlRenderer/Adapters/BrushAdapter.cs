// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* BrushAdapter.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Windows.Media;

using AM.Drawing.HtmlRenderer.Adapters;

#endregion

#nullable enable

namespace AM.Windows.HtmlRenderer.Adapters;

/// <summary>
/// Adapter for WPF brushes.
/// </summary>
internal sealed class BrushAdapter 
    : RBrush
{
    /// <summary>
    /// The actual WPF brush instance.
    /// </summary>
    private readonly Brush _brush;

    /// <summary>
    /// Init.
    /// </summary>
    public BrushAdapter(Brush brush)
    {
        _brush = brush;
    }

    /// <summary>
    /// The actual WPF brush instance.
    /// </summary>
    public Brush Brush
    {
        get { return _brush; }
    }

    public override void Dispose()
    { }
}