// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable LocalizableElement
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* StatusLabel.cs -- текстовый элемент статусной строки
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Drawing;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms.AppServices;

/// <summary>
/// Текстовый элемент статусной строки.
/// </summary>
public class StatusLabel
    : ToolStripLabel
{
    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public StatusLabel()
    {
        Initialize();
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public StatusLabel
        (
            string? text
        )
        : base (text)
    {
        Initialize();
    }

    #endregion

    #region Private members

    /// <summary>
    /// Инициализация.
    /// </summary>
    private void Initialize()
    {
        Padding = new Padding (2);
    }

    #endregion

    #region ToolStripLabel members

    /// <inheritdoc cref="ToolStripItem.OnPaint"/>
    protected override void OnPaint
        (
            PaintEventArgs eventArgs
        )
    {
        var graphics = eventArgs.Graphics;
        var bounds = eventArgs.ClipRectangle;
        bounds.Inflate (-1, -1);
        graphics.DrawRectangle (Pens.Black, bounds);

        base.OnPaint (eventArgs);
    }

    #endregion
}
