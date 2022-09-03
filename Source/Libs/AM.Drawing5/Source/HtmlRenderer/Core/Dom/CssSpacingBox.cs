// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* CssSpacingBox.cs -- используется для создания пробела в вертикальной комбинации ячеек
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

using AM.Drawing.HtmlRenderer.Core.Utils;

#endregion

#nullable enable

namespace AM.Drawing.HtmlRenderer.Core.Dom;

/// <summary>
/// Используется для создания пробела в вертикальной комбинации ячеек.
/// </summary>
internal sealed class CssSpacingBox
    : CssBox
{
    #region Properties

    /// <summary>
    /// Расширенный блок.
    /// </summary>
    public CssBox ExtendedBox { get; }

    /// <summary>
    /// Получает индекс строки, в которой начинается блок.
    /// </summary>
    public int StartRow { get; }

    /// <summary>
    /// Получает индекс строки, где заканчивается блок.
    /// </summary>
    public int EndRow { get; }

    #endregion

    #region Construction

    public CssSpacingBox
        (
            CssBox tableBox,
            ref CssBox extendedBox,
            int startRow
        )
        : base
            (
                tableBox,
                new HtmlTag ("none", false, new Dictionary<string, string> { { "colspan", "1" } })
            )
    {
        ExtendedBox = extendedBox;
        Display = CssConstants.None;

        StartRow = startRow;
        EndRow = startRow + int.Parse (extendedBox.GetAttribute ("rowspan", "1")!) - 1;
    }

    #endregion

}
