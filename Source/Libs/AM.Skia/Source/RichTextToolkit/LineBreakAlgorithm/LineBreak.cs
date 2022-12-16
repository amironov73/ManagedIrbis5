// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* LineBreak.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics;

#endregion

#nullable enable

namespace AM.Skia.RichTextKit;

/// <summary>
/// Information about a potential line break position
/// </summary>
[DebuggerDisplay ("{PositionMeasure}/{PositionWrap} @ {Required}")]
internal struct LineBreak
{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="positionMeasure">The code point index to measure to</param>
    /// <param name="positionWrap">The code point index to actually break the line at</param>
    /// <param name="required">True if this is a required line break; otherwise false</param>
    public LineBreak (int positionMeasure, int positionWrap, bool required = false)
    {
        PositionMeasure = positionMeasure;
        PositionWrap = positionWrap;
        Required = required;
    }


    /// <summary>
    /// The break position, before any trailing whitespace
    /// </summary>
    /// <remarks>
    /// This doesn't include trailing whitespace
    /// </remarks>
    public int PositionMeasure;

    /// <summary>
    /// The break position, after any trailing whitespace
    /// </summary>
    /// <remarks>
    /// This includes trailing whitespace
    /// </remarks>
    public int PositionWrap;

    /// <summary>
    /// True if there should be a forced line break here
    /// </summary>
    public bool Required;
}
