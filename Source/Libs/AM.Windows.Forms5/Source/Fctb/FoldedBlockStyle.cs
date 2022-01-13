// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* FoldedBlockSytle.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Drawing;

#endregion

#nullable enable

namespace Fctb;

/// <summary>
/// Renderer for folded block
/// </summary>
public class FoldedBlockStyle
    : TextStyle
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public FoldedBlockStyle
        (
            Brush foreBrush,
            Brush backgroundBrush,
            FontStyle fontStyle
        )
        : base (foreBrush, backgroundBrush, fontStyle)
    {
    }

    #endregion

    #region Style members

    /// <inheritdoc cref="TextStyle.Draw"/>
    public override void Draw
        (
            Graphics graphics,
            Point position,
            TextRange range
        )
    {
        if (range.End.Column > range.Start.Column)
        {
            base.Draw (graphics, position, range);

            var firstNonSpaceSymbolX = position.X;

            //find first non space symbol
            for (var i = range.Start.Column; i < range.End.Column; i++)
            {
                if (range.tb[range.Start.Line][i].c != ' ')
                {
                    break;
                }

                firstNonSpaceSymbolX += range.tb.CharWidth;
            }

            //create marker
            range.tb.AddVisualMarker (new FoldedAreaMarker (range.Start.Line,
                new Rectangle (firstNonSpaceSymbolX, position.Y,
                    position.X + (range.End.Column - range.Start.Column) * range.tb.CharWidth - firstNonSpaceSymbolX,
                    range.tb.CharHeight)));
        }
        else
        {
            //draw '...'
            using (var f = new Font (range.tb.Font, FontStyle))
            {
                graphics.DrawString ("...", f, ForeBrush, range.tb.LeftIndent, position.Y - 2);
            }

            //create marker
            range.tb.AddVisualMarker (new FoldedAreaMarker (range.Start.Line,
                new Rectangle (range.tb.LeftIndent + 2, position.Y, 2 * range.tb.CharHeight, range.tb.CharHeight)));
        }
    }

    #endregion
}
