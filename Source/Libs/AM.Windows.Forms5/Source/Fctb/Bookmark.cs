// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* Bookmark.cs -- закладка в текстовом редакторе
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Drawing;
using System.Drawing.Drawing2D;

using AM;

#endregion

#nullable enable

namespace Fctb;

/// <summary>
/// Закладка в текстовом редакторе.
/// </summary>
public class Bookmark
{
    #region Properties

    /// <summary>
    /// Редактор, в котором установлена закладка.
    /// </summary>
    public SyntaxTextBox TextBox { get; }

    /// <summary>
    /// Name of bookmark
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Line index
    /// </summary>
    public int LineIndex { get; set; }

    /// <summary>
    /// Color of bookmark sign
    /// </summary>
    public Color Color { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public Bookmark
        (
            SyntaxTextBox textBox,
            string name,
            int lineIndex
        )
    {
        Sure.NotNull (textBox);

        TextBox = textBox;
        Name = name;
        LineIndex = lineIndex;
        Color = textBox.BookmarkColor;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Scroll textbox to the bookmark
    /// </summary>
    public virtual void DoVisible()
    {
        TextBox.Selection.Start = new Place (0, LineIndex);
        TextBox.DoRangeVisible (TextBox.Selection, true);
        TextBox.Invalidate();
    }

    /// <summary>
    /// Отрисовка закладки.
    /// </summary>
    /// <param name="graphics"></param>
    /// <param name="lineRect"></param>
    public virtual void Paint
        (
            Graphics graphics,
            Rectangle lineRect
        )
    {
        var size = TextBox.CharHeight - 1;
        using (var brush =
               new LinearGradientBrush (new Rectangle (0, lineRect.Top, size, size), Color.White, Color, 45))
            graphics.FillEllipse (brush, 0, lineRect.Top, size, size);
        using (var pen = new Pen (Color))
            graphics.DrawEllipse (pen, 0, lineRect.Top, size, size);
    }

    #endregion
}
