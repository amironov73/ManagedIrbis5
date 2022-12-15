// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Text;

#endregion

#nullable enable

namespace AM.Skia.RichTextKit;

/// <summary>
/// Helper class to swap two values
/// </summary>
public static class SwapHelper
{
    /// <summary>
    /// Swaps two values
    /// </summary>
    /// <typeparam name="T">The value type</typeparam>
    /// <param name="a">The first value</param>
    /// <param name="b">The second value</param>
    public static void Swap<T> (ref T a, ref T b)
    {
        var temp = a;
        a = b;
        b = temp;
    }
}
