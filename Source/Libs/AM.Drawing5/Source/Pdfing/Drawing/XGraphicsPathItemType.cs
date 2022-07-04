// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* XGraphicsPathItemType.cs --
 * Ars Magna project, http://arsmagna.ru
 */

namespace PdfSharpCore.Drawing
{
    /// <summary>
    /// Type of the path data.
    /// </summary>
    internal enum XGraphicsPathItemType
    {
        Lines,
        Beziers,
        Curve,
        Arc,
        Rectangle,
        RoundedRectangle,
        Ellipse,
        Polygon,
        CloseFigure,
        StartFigure,
    }
}
