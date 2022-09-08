// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* Configuration.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace PdfSharpCore;

/// <summary>
/// Floating point formatting.
/// </summary>
static class Config
{
    public const string SignificantFigures2 = "0.##";
    public const string SignificantFigures3 = "0.###";
    public const string SignificantFigures4 = "0.####";
    public const string SignificantFigures7 = "0.#######";
    public const string SignificantFigures10 = "0.##########";
    public const string SignificantFigures1Plus9 = "0.0#########";
}

static class Const
{
    /// <summary>
    /// Factor to convert from degree to radian measure.
    /// </summary>
    public const double Deg2Rad = Math.PI / 180;  // = 0.017453292519943295

    /// <summary>
    /// Sinus of the angle to turn a regular font to look oblique. Used for italic simulation.
    /// </summary>
    public const double ItalicSkewAngleSinus = 0.34202014332566873304409961468226;  // = sin(20°)

    /// <summary>
    /// Factor of the em size of a regular font to look bold. Used for bold simulation.
    /// Value of 2% found in original XPS 1.0 documentation.
    /// </summary>
    public const double BoldEmphasis = 0.02;

    // The κ (kappa) for drawing a circle or an ellipse with four Bézier splines, specifying the distance of the influence point from the starting or end point of a spline.
    // Petzold: 4/3 * tan(α / 4)
    public const double κ = 0.5522847498307933984022516322796;  // := 4/3 * (1 - cos(-π/4)) / sin(π/4)) <=> 4/3 * (sqrt(2) - 1) <=> 4/3 * tan(π/8)
}
