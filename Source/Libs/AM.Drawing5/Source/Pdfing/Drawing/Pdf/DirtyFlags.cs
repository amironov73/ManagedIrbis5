// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* DirtyFlags.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

namespace PdfSharpCore.Drawing.Pdf
{
    [Flags]
    enum DirtyFlags
    {
        Ctm = 0x00000001,
        ClipPath = 0x00000002,
        LineWidth = 0x00000010,
        LineJoin = 0x00000020,
        MiterLimit = 0x00000040,
        StrokeFill = 0x00000070,
    }
}
