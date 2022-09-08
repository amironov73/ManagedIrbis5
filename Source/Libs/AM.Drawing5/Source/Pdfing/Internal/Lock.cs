// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* Lock.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Threading;

#endregion

#nullable enable

namespace PdfSharpCore.Internal;

/// <summary>
/// Static locking functions to make PDFsharp thread save.
/// </summary>
internal static class Lock
{
    public static void EnterGdiPlus()
    {
        //if (_fontFactoryLockCount > 0)
        //    throw new InvalidOperationException("");

        Monitor.Enter (GdiPlus);
        _gdiPlusLockCount++;
    }

    public static void ExitGdiPlus()
    {
        _gdiPlusLockCount--;
        Monitor.Exit (GdiPlus);
    }

    static readonly object GdiPlus = new object();
    static int _gdiPlusLockCount;

    public static void EnterFontFactory()
    {
        Monitor.Enter (FontFactory);
        _fontFactoryLockCount++;
    }

    public static void ExitFontFactory()
    {
        _fontFactoryLockCount--;
        Monitor.Exit (FontFactory);
    }

    static readonly object FontFactory = new object();

    [ThreadStatic]
    static int _fontFactoryLockCount;
}
