// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using Avalonia.Controls;
using Avalonia.Layout;

#endregion

#nullable enable

namespace Avalonia.ExtendedToolkit.Controls;

//ported from https://github.com/HandyOrg/HandyControl.git

/// <summary>
/// panel which size is changed by its children
/// </summary>
public class SimplePanel
    : Panel
{
    /// <inheritdoc cref="Layoutable.MeasureOverride"/>
    protected override Size MeasureOverride
        (
            Size availableSize
        )
    {
        var maxSize = new Size();

        foreach (Control child in Children)
        {
            if (child != null)
            {
                child.Measure(availableSize);
                var width = Math.Max(maxSize.Width, child.DesiredSize.Width);
                var height = Math.Max(maxSize.Height, child.DesiredSize.Height);
                maxSize = new Size(width, height);
            }
        }

        return maxSize;
    }

    /// <inheritdoc cref="Layoutable.ArrangeOverride"/>
    protected override Size ArrangeOverride
        (
            Size arrangeSize
        )
    {
        foreach (Control child in Children)
        {
            child?.Arrange(new Rect(arrangeSize));
        }

        return arrangeSize;
    }
}
