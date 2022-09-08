// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* Calc.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using PdfSharpCore.Drawing;

#endregion

#nullable enable

namespace PdfSharpCore.Internal;

/// <summary>
/// Some static helper functions for calculations.
/// </summary>
internal static class Calc
{
    /// <summary>
    /// Degree to radiant factor.
    /// </summary>
    public const double Deg2Rad = Math.PI / 180;

    ///// <summary>
    ///// Half of pi.
    ///// </summary>
    //public const double πHalf = Math.PI / 2;
    //// α - β κ

    /// <summary>
    /// Get page size in point from specified PageSize.
    /// </summary>
    public static XSize PageSizeToSize (PageSize value)
    {
        return value switch
        {
            PageSize.A0 => new XSize (2380, 3368),
            PageSize.A1 => new XSize (1684, 2380),
            PageSize.A2 => new XSize (1190, 1684),
            PageSize.A3 => new XSize (842, 1190),
            PageSize.A4 => new XSize (595, 842),
            PageSize.A6 => new XSize (298, 420),
            PageSize.A5 => new XSize (420, 595),
            PageSize.B4 => new XSize (729, 1032),
            PageSize.B5 => new XSize (516, 729),

            // The strange sizes from overseas...
            PageSize.Letter => new XSize (612, 792),
            PageSize.Legal => new XSize (612, 1008),
            PageSize.Tabloid => new XSize (792, 1224),
            PageSize.Ledger => new XSize (1224, 792),
            PageSize.Statement => new XSize (396, 612),
            PageSize.Executive => new XSize (540, 720),
            PageSize.Folio => new XSize (612, 936),
            PageSize.Quarto => new XSize (610, 780),
            PageSize.Size10x14 => new XSize (720, 1008),

            _ => throw new ArgumentException ("Invalid PageSize.")
        };
    }
}
