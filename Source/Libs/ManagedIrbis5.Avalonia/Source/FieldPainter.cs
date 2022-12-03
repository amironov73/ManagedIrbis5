// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable StringLiteralTypo

/* FieldPainter.cs -- умеет отрисовывать содержимое поля
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Globalization;

using AM;
using AM.Text;

using Avalonia;
using Avalonia.Media;

#endregion

#nullable enable

namespace ManagedIrbis.Avalonia;

/// <summary>
/// Умеет отрисовывать содержимое поля.
/// </summary>
public sealed class FieldPainter
{
    #region Properties

    /// <summary>
    /// Кисть для отрисовки "шляпы" и кода подполя.
    /// </summary>
    public IBrush CodeBrush { get; set; }

    /// <summary>
    /// Кисть для отрисовки прочего содержимого поля.
    /// </summary>
    public IBrush TextBrush { get; set; }

    /// <summary>
    /// Используемый шрифт.
    /// </summary>
    public Typeface Typeface { get; set; }

    /// <summary>
    /// Размер шрифта.
    /// </summary>
    public double FontSize { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию
    /// </summary>
    public FieldPainter()
    {
        CodeBrush = Brushes.Red;
        TextBrush = Brushes.Black;
        Typeface = Typeface.Default;
        FontSize = 14.0;
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="codeBrush"></param>
    /// <param name="textBrush"></param>
    /// <param name="typeface"></param>
    /// <param name="fontSize"></param>
    public FieldPainter
        (
            IBrush codeBrush,
            IBrush textBrush,
            Typeface typeface,
            double fontSize
        )
    {
        Sure.NotNull (codeBrush);
        Sure.NotNull (textBrush);

        CodeBrush = codeBrush;
        TextBrush = textBrush;
        Typeface = typeface;
        FontSize = fontSize;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Отрисовка заданного текста.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="bounds"></param>
    /// <param name="text"></param>
    public void Paint
        (
            DrawingContext context,
            Rect bounds,
            string? text
        )
    {
        if (string.IsNullOrEmpty (text))
        {
            return;
        }

        var formatted = new FormattedText
            (
                text,
                CultureInfo.InvariantCulture,
                FlowDirection.LeftToRight,
                Typeface,
                FontSize,
                TextBrush
            );
        formatted.MaxTextWidth = bounds.Width;
        formatted.MaxLineCount = 1;
        formatted.Trimming = TextTrimming.None;

        var navigator = new TextNavigator (text);
        while (true)
        {
            navigator.ReadUntil ('^');
            if (navigator.IsEOF || navigator.Position > navigator.Length - 1)
            {
                break;
            }

            formatted.SetForegroundBrush (CodeBrush, navigator.Position, 2);
            formatted.SetFontWeight (FontWeight.Bold, navigator.Position, 2);
            navigator.Move (2);
        }

        context.DrawText (formatted, new Point ());
    }

    #endregion
}
