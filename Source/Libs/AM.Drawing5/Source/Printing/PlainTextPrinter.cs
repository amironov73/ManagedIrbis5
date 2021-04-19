// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* SimplestTextPrinter.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Drawing;
using System.Drawing.Printing;
using System.Drawing.Text;

#endregion

#nullable enable

namespace AM.Drawing.Printing
{
    /// <summary>
    ///
    /// </summary>
    [System.ComponentModel.DesignerCategory("Code")]
    public class PlainTextPrinter
        : TextPrinter
    {
        #region Private members

        private string? _text;

        private int _offset;

        /// <summary>
        /// Called when [print page].
        /// </summary>
        protected override void OnPrintPage
            (
                object sender,
                PrintPageEventArgs e
            )
        {
            base.OnPrintPage(sender, e);

            if (_text is null)
            {
                return;
            }

            var graphics = e.Graphics.ThrowIfNull("e.Graphics");
            var text = _text.Substring(_offset);
            using var brush = new SolidBrush(TextColor);
            using var format = new StringFormat
            {
                Alignment = StringAlignment.Near,
                LineAlignment = StringAlignment.Near,
                HotkeyPrefix = HotkeyPrefix.None,
                Trimming = StringTrimming.Word,
                FormatFlags = StringFormatFlags.LineLimit
            };

            RectangleF rect = e.PageBounds;
            rect.X += Borders.Left;
            rect.Width -= (Borders.Left + Borders.Right);
            rect.Y += Borders.Top;
            rect.Height -= (Borders.Top + Borders.Bottom);
            rect.Height = (rect.Height / TextFont.Size) * TextFont.Size;
            graphics.DrawString(text, TextFont, brush, rect, format);
            graphics.MeasureString
                (
                    text,
                    TextFont,
                    rect.Size,
                    format,
                    out var charFitted,
                    out _
                );
            e.HasMorePages = (charFitted < text.Length);
            _offset += charFitted;
        }

        #endregion

        #region Public methods

        /// <inheritdoc cref="TextPrinter.Print"/>
        public override bool Print
            (
                string text
            )
        {
            _offset = 0;
            _text = text;

            return base.Print(text);
        }

        #endregion

    } // class PlainTextPrinter

} // namespace AM.Windows.Printing
