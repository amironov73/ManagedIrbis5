// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* PointD.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Drawing.Charting;

/// <summary>
/// Simple struct that stores X and Y coordinates as doubles.
/// </summary>
[Serializable]
public struct PointD
{
    /// <summary>
    /// The X coordinate
    /// </summary>
    public double X;

    /// <summary>
    /// The Y coordinate
    /// </summary>
    public double Y;

    /// <summary>
    /// Construct a <see cref="PointD" /> object from two double values.
    /// </summary>
    /// <param name="x">The X coordinate</param>
    /// <param name="y">The Y coordinate</param>
    public PointD (double x, double y)
    {
        X = x;
        Y = y;
    }
}
