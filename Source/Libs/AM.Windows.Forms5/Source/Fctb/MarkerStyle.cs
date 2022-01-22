// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* MarkerSytle.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Drawing;

#endregion

#nullable enable

namespace Fctb;

/// <summary>
/// Marker style
/// Draws background color for text
/// </summary>
public class MarkerStyle
    : Style
{
    #region Properties

    /// <summary>
    /// Кисть для фона.
    /// </summary>
    public Brush? BackgroundBrush { get; set; }

    #endregion

    /// <summary>
    /// Конструктор.
    /// </summary>
    public MarkerStyle
        (
            Brush backgroundBrush
        )
    {
        BackgroundBrush = backgroundBrush;
        IsExportable = true;
    }

    #region Style members

    /// <inheritdoc cref="Style.Draw"/>
    public override void Draw
        (
            Graphics graphics,
            Point position,
            TextRange range
        )
    {
        //draw background
        if (BackgroundBrush != null)
        {
            var rect = new Rectangle (position.X, position.Y,
                (range.End.Column - range.Start.Column) * range._textBox.CharWidth, range._textBox.CharHeight);
            if (rect.Width == 0)
            {
                return;
            }

            graphics.FillRectangle (BackgroundBrush, rect);
        }
    }

    /// <inheritdoc cref="Style.GetCSS"/>
    public override string GetCSS()
    {
        var result = string.Empty;

        if (BackgroundBrush is SolidBrush solidBrush)
        {
            var s = ExportToHtml.GetColorAsString (solidBrush.Color);
            if (s != string.Empty)
            {
                result += "background-color:" + s + ";";
            }
        }

        return result;
    }

    #endregion
}
