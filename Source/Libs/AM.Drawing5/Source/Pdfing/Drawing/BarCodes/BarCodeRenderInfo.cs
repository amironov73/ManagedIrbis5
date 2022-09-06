// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* BarcodeRenderInfo.cs -- вся необходимая информация для рендеринга штрих-кода
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace PdfSharpCore.Drawing.BarCodes;

/// <summary>
/// Вся (временно) необходимая информация для рендеринга штрих-кода.
/// </summary>
class BarCodeRenderInfo
{
    #region Properties

    /// <summary>
    /// Канва.
    /// </summary>
    public XGraphics Graphics { get; set; }

    /// <summary>
    /// Кисть.
    /// </summary>
    public XBrush Brush { get; set; }

    /// <summary>
    /// Шрифт.
    /// </summary>
    public XFont Font { get; set; }

    /// <summary>
    /// Позиция.
    /// </summary>
    /// <remarks>Структура!</remarks>
    public XPoint Position;

    /// <summary>
    /// Высота полоски штрих-кода.
    /// </summary>
    public double BarHeight { get; set; }

    /// <summary>
    /// Текущая позиция.
    /// </summary>
    /// <remarks>Структура!</remarks>
    public XPoint CurrentPosition;

    /// <summary>
    /// Текущая позиция в строке.
    /// </summary>
    public int CurrentPositionInString { get; set; }

    /// <summary>
    /// Ширина узкой полоски.
    /// </summary>
    public double ThinBarWidth { get; set; }

    #endregion

    #region Construction

    public BarCodeRenderInfo
        (
            XGraphics graphics,
            XBrush brush,
            XFont font,
            XPoint position
        )
    {
        Graphics = graphics;
        Brush = brush;
        Font = font;
        Position = position;
    }

    #endregion
}
