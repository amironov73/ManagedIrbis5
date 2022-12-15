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
/// Used to return hit test information from the
/// <see cref="TextBlock.HitTest(float, float)"/> method.
/// </summary>
public struct HitTestResult
{
    /// <summary>
    /// The zero based index of the line number the y-coordinate is directly
    /// over, or -1 if the y-coordinate is before the first line, or after the
    /// last line.
    /// </summary>
    /// <remarks>
    /// The x-coordinate isn't used in calculating this value and the left/right
    /// limits aren't checked.
    /// </remarks>
    public int OverLine;

    /// <summary>
    /// The zero based index of the closest line to the passed y-coordinate.
    /// </summary>
    /// <remarks>
    /// If the point is directly over a line this value will be the same as the
    /// <see cref="OverLine"/> property.  If the point is before the first line,
    /// this property will be 0.  If the point is after the last line this value
    /// will be the index of the last line.
    /// </remarks>
    public int ClosestLine;

    /// <summary>
    /// The code point index of the first code point in the cluster that the
    /// point is actually over, or -1 if not over a cluster.
    /// </summary>
    public int OverCodePointIndex;

    /// <summary>
    /// The code point index of the first code point in the cluster that the
    /// point is closest to.
    /// </summary>
    /// <remarks>
    /// If the point is over a cluster, the returned code point index will vary
    /// depending whether the point is in the left or right half of the cluster
    /// and the text direction of that cluster.
    ///
    /// This value represents the code point index that the caret should be moved to
    /// if the user clicked the mouse at this position.  To determine the co-ordinates
    /// and shape of the caret, see [Caret Information](/caret).
    /// </remarks>
    public int ClosestCodePointIndex;

    /// <summary>
    /// Indicates that the point is closest to the alternate caret position
    /// of ClosestCodePointIndex.
    /// </summary>
    /// <remarks>
    /// This property indicates if the tested point is beyond the end of
    /// a word wrapped line and not at the start of the following line.
    /// </remarks>
    public bool AltCaretPosition;


    /// <summary>
    /// Helper to get the closest position as a CaretPosition
    /// </summary>
    public CaretPosition CaretPosition => new CaretPosition (ClosestCodePointIndex, AltCaretPosition);

    /// <summary>
    /// Compares this object to another instance
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals (object obj)
    {
        return obj is HitTestResult result &&
               OverLine == result.OverLine &&
               ClosestLine == result.ClosestLine &&
               OverCodePointIndex == result.OverCodePointIndex &&
               ClosestCodePointIndex == result.ClosestCodePointIndex;
    }

    /// <summary>
    /// Gets a hash code for this object
    /// </summary>
    /// <returns>The hash value</returns>
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    /// <summary>
    /// Check is this is the "none" hit test result
    /// </summary>
    public bool IsNone => ClosestCodePointIndex < 0;

    /// <summary>
    /// Hit test result indicating no hit, or untested hit
    /// </summary>
    public static HitTestResult None = new HitTestResult()
    {
        OverLine = -1,
        OverCodePointIndex = -1,
        ClosestLine = -1,
        ClosestCodePointIndex = -1,
    };
}
