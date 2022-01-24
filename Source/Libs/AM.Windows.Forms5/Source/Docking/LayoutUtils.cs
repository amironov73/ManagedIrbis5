// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* 
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Drawing;

#endregion

#nullable enable

namespace AM.Windows.Forms.Docking;

public static class LayoutUtils
{
    public static bool IsZeroWidthOrHeight (Rectangle rectangle)
    {
        return (rectangle.Width == 0 || rectangle.Height == 0);
    }
}