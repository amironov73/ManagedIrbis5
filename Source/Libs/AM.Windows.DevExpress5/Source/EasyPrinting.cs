// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* EasyPrinting.cs -- легко и просто рисуем на PrintingSystem
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Drawing;

using DevExpress.XtraPrinting;

#endregion

#nullable enable

namespace AM.Windows.DevExpress;

/// <summary>
/// Легко и просто рисуем на PrintingSystem.
/// </summary>
public class EasyPrinting
    : IDisposable
{
    #region Properties

    /// <summary>
    /// Система печати DevExpress.
    /// </summary>
    public PrintingSystem Printing { get; }

    /// <summary>
    /// Канва для рисования
    /// </summary>
    public BrickGraphics Graphics { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public EasyPrinting()
    {
        Printing = new PrintingSystem();
        Printing.Begin();
        Graphics = Printing.Graph;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Вывод строки.
    /// </summary>
    public TextBrick DrawString
        (
            string text
        )
    {
        var size = Graphics.MeasureString (text);

        return Graphics.DrawString
            (
                text,
                new RectangleF (new PointF (), size)
            );
    }

    /// <summary>
    /// Вывод строки.
    /// </summary>
    public TextBrick DrawString
        (
            string text,
            RectangleF rectangle
        )
    {
        return Graphics.DrawString (text, rectangle);
    }

    /// <summary>
    /// Вывод строки с границами.
    /// </summary>
    public TextBrick DrawString
        (
            string text,
            RectangleF rectangle,
            Color foreColor,
            BorderSide borders
        )
    {
        return Graphics.DrawString (text, foreColor, rectangle, borders);
    }

    /// <summary>
    /// Сохранение в картинку.
    /// </summary>
    public void ExportToImage
        (
            string fileName
        )
    {
        Printing.End();
        Printing.ExportToImage (fileName);
    }

    /// <summary>
    /// Предварительный просмотр.
    /// </summary>
    public void ShowPreview()
    {
        Printing.End();
        Printing.PreviewFormEx.ShowDialog();
    }

    #endregion

    #region IDisposable members

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public void Dispose()
    {
        Printing.End();
        Printing.Dispose();
    }

    #endregion
}
