// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* GdiPlusUtility.cs -- some useful methods from GDI+
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;
using System.Drawing;
using System.Reflection;

#endregion

#nullable enable

namespace AM.Win32;

/// <summary>
/// Some useful methods from GDI+
/// </summary>
public static class GdiPlusUtility
{
    #region Public methods

    /// <summary>
    /// Gets the bitmap from GDI plus.
    /// </summary>
    public static Bitmap? GetBitmapFromGdiPlus
        (
            IntPtr gdiPlusBitmap
        )
    {
        var method = typeof (Bitmap).GetMethod
                (
                    "FromGDIplus",
                    BindingFlags.Static | BindingFlags.NonPublic,
                    null,
                    new[] { typeof (IntPtr) },
                    null
                )
            .ThrowIfNull();

        var result = (Bitmap?)method.Invoke
            (
                null,
                new object[] { gdiPlusBitmap }
            );

        return result;
    }

    #endregion
}
