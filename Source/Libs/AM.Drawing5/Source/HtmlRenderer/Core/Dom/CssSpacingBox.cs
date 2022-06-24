// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* CssSpacingBox.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

using AM.Drawing.HtmlRenderer.Core.Utils;

#endregion

#nullable enable

namespace AM.Drawing.HtmlRenderer.Core.Dom;

/// <summary>
/// Used to make space on vertical cell combination
/// </summary>
internal sealed class CssSpacingBox 
    : CssBox
{
    #region Fields and Consts

    private readonly CssBox _extendedBox;

    /// <summary>
    /// the index of the row where box starts
    /// </summary>
    private readonly int _startRow;

    /// <summary>
    /// the index of the row where box ends
    /// </summary>
    private readonly int _endRow;

    #endregion

    #region Construction

    public CssSpacingBox(CssBox tableBox, ref CssBox extendedBox, int startRow)
        : base(tableBox, new HtmlTag("none", false, new Dictionary<string, string> { { "colspan", "1" } }))
    {
        _extendedBox = extendedBox;
        Display = CssConstants.None;

        _startRow = startRow;
        _endRow = startRow + Int32.Parse(extendedBox.GetAttribute("rowspan", "1")) - 1;
    }

    #endregion

    public CssBox ExtendedBox
    {
        get { return _extendedBox; }
    }

    /// <summary>
    /// Gets the index of the row where box starts
    /// </summary>
    public int StartRow
    {
        get { return _startRow; }
    }

    /// <summary>
    /// Gets the index of the row where box ends
    /// </summary>
    public int EndRow
    {
        get { return _endRow; }
    }
}
