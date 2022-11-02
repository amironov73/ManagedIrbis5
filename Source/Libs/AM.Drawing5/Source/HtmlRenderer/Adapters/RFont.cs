// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* RFont.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Drawing.HtmlRenderer.Adapters;

/// <summary>
/// Adapter for platform specific font object - used to render text using specific font.
/// </summary>
public abstract class RFont
{
    #region Properties

    /// <summary>
    /// Gets the em-size of this Font measured in the units specified by the Unit property.
    /// </summary>
    public abstract double Size { get; }

    /// <summary>
    /// The line spacing, in pixels, of this font.
    /// </summary>
    public abstract double Height { get; }

    /// <summary>
    /// Get the vertical offset of the font underline location from the top of the font.
    /// </summary>
    public abstract double UnderlineOffset { get; }

    /// <summary>
    /// Get the left padding, in pixels, of the font.
    /// </summary>
    public abstract double LeftPadding { get; }

    #endregion

    #region Public methods

    /// <summary>
    ///
    /// </summary>
    /// <param name="graphics"></param>
    /// <returns></returns>
    public abstract double GetWhitespaceWidth (RGraphics graphics);

    #endregion
}
