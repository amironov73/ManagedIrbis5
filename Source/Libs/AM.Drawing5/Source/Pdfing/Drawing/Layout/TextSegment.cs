// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* TextSegment.cs -- текстовый сегмент
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace PdfSharpCore.Drawing.Layout;

/// <summary>
/// Текстовый сегмент.
/// </summary>
public class TextSegment
{
    /// <summary>
    /// Шрифт.
    /// </summary>
    public XFont? Font { get; set; }

    /// <summary>
    /// Кисть.
    /// </summary>
    public XBrush? Brush { get; set; }

    /// <summary>
    /// Собственно текст.
    /// </summary>
    public string? Text { get; set; }

    /// <summary>
    /// Отступ.
    /// </summary>
    public double LineIndent { get; set; }

    /// <summary>
    ///
    /// </summary>
    public bool SkipParagraphAlignment { get; set; }

    /// <summary>
    /// Промежуток между строками.
    /// </summary>
    public double LineSpace { get; set; }

    /// <summary>
    ///
    /// </summary>
    public double CyAscent { get; set; }

    /// <summary>
    ///
    /// </summary>
    public double CyDescent { get; set; }

    /// <summary>
    /// Ширина пробела.
    /// </summary>
    public double SpaceWidth { get; set; }
}
