// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* SimpleTextPrinter.cs -- простейший принтер для плоского текста
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Drawing;
using System.Drawing.Printing;
using System.Drawing.Text;

#endregion

#nullable enable

namespace AM.Windows.Forms.Printing;

/// <summary>
/// Простейший принтер для плоского текста
/// без малейших украшательств.
/// </summary>
public class PlainTextPrinter
    : TextPrinter
{
    #region Private members

    private string? _text;

    private int _offset;

    /// <inheritdoc cref="TextPrinter.OnPrintPage"/>
    protected override void OnPrintPage
        (
            object sender,
            PrintPageEventArgs eventArgs
        )
    {
        base.OnPrintPage (sender, eventArgs);
        var graphics = eventArgs.Graphics.ThrowIfNull();
        var text = _text?[_offset..];
        if (string.IsNullOrWhiteSpace (text))
        {
            return;
        }

        using var brush = new SolidBrush (TextColor);
        using var format = new StringFormat
        {
            Alignment = StringAlignment.Near,
            LineAlignment = StringAlignment.Near,
            HotkeyPrefix = HotkeyPrefix.None,
            Trimming = StringTrimming.Word,
            FormatFlags = StringFormatFlags.LineLimit
        };
        RectangleF rect = eventArgs.PageBounds;
        rect.X += Borders.Left;
        rect.Width -= Borders.Left + Borders.Right;
        rect.Y += Borders.Top;
        rect.Height -= Borders.Top + Borders.Bottom;
        rect.Height = rect.Height / TextFont.Size * TextFont.Size;
        graphics.DrawString (text, TextFont, brush, rect, format);

        graphics.MeasureString
            (
                text,
                TextFont,
                rect.Size,
                format,
                out var charFitted,
                out _
            );

        eventArgs.HasMorePages = charFitted < text.Length;
        _offset += charFitted;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Выводит заданный текст на принтер.
    /// </summary>
    public override bool Print
        (
            string text
        )
    {
        Sure.NotNull (text);

        _offset = 0;
        _text = text;
        return base.Print (text);
    }

    #endregion
}
