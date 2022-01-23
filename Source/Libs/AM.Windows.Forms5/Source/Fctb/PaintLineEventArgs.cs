// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* PaintLineEventAgrs.cs -- аргумент для события рисования строки
 * Ars Magna project, http://arsmagna.ru
 */

using System.Drawing;
using System.Windows.Forms;

namespace Fctb;

/// <summary>
/// Аргумент для события рисования строки.
/// </summary>
public sealed class PaintLineEventArgs
    : PaintEventArgs
{
    #region Properties

    /// <summary>
    /// Индекс строки.
    /// </summary>
    public int LineIndex { get; }

    /// <summary>
    /// Прямоугольник строки.
    /// </summary>
    public Rectangle LineRect { get; }

    #endregion

    #region Construciton

    /// <summary>
    /// Конструктор.
    /// </summary>
    public PaintLineEventArgs
        (
            int lineIndex,
            Rectangle rect,
            Graphics graphics,
            Rectangle clipRect
        )
        : base (graphics, clipRect)
    {
        LineIndex = lineIndex;
        LineRect = rect;
    }

    #endregion
}
