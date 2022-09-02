// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* CssRectImage.cs -- представляет картинку внутри inline-блока
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Drawing.HtmlRenderer.Adapters;
using AM.Drawing.HtmlRenderer.Adapters.Entities;

#endregion

#nullable enable

namespace AM.Drawing.HtmlRenderer.Core.Dom;

/// <summary>
/// Представляет картинку внутри inline-блока
/// </summary>
internal sealed class CssRectImage
    : CssRect
{
    #region Properties

    /// <summary>
    /// Gets the image this words represents (if one exists)
    /// </summary>
    public override RImage? Image { get; set; }

    /// <summary>
    /// Gets if the word represents an image.
    /// </summary>
    public override bool IsImage => true;

    /// <summary>
    /// the image rectange restriction as returned from image load event
    /// </summary>
    public RRect ImageRectangle { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Creates a new BoxWord which represents an image
    /// </summary>
    /// <param name="owner">the CSS box owner of the word</param>
    public CssRectImage
        (
            CssBox owner
        )
        : base (owner)
    {
        // пустое тело конструктора
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return "Image";
    }

    #endregion
}
