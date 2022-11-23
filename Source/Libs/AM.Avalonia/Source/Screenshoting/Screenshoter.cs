// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local

/* Screenshoter.cs -- снятие скриншотов с указанных окон и контролов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Linq;

using Avalonia.Media;
using Avalonia.Rendering;
using Avalonia.Skia;
using Avalonia.Skia.Helpers;
using Avalonia.VisualTree;

using SkiaSharp;

#endregion

namespace AM.Avalonia.Screenshoting;

/// <summary>
/// Снятие снимка с указанных окон и контролов.
/// </summary>
public static class Screenshoter
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="visuals"></param>
    public static void ToPdfFile (string fileName, params IVisual[] visuals) =>
        ToPdfFile (fileName, visuals.AsEnumerable());

    /// <summary>
    ///
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="visuals"></param>
    public static void ToPdfFile
        (
            string fileName,
            IEnumerable<IVisual> visuals
        )
    {
        using var doc = SKDocument.CreatePdf (fileName);
        foreach (var visual in visuals)
        {
            var bounds = visual.Bounds;
            var page = doc.BeginPage ((float) bounds.Width, (float) bounds.Height);
            using var context = new DrawingContext
                (
                    DrawingContextHelper.WrapSkiaCanvas (page, SkiaPlatform.DefaultDpi)
                );
            ImmediateRenderer.Render (visual, context);
            doc.EndPage();
        }

        doc.Close();
    }
}
