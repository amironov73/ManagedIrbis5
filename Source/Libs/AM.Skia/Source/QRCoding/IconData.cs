// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* IconData.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using SkiaSharp;

#endregion

#nullable enable

namespace AM.Skia.QrCoding;

/// <summary>
///
/// </summary>
public class IconData
{
    #region Properties

    /// <summary>
    ///
    /// </summary>
    public SKBitmap? Icon { get; set; }

    /// <summary>
    ///
    /// </summary>
    public int IconSizePercent { get; set; } = 10;

    #endregion
}
