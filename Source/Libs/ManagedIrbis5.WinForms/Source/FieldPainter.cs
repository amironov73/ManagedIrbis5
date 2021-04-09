// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* FieldPainter.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Drawing;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.WinForms
{
    /// <summary>
    ///
    /// </summary>
    public sealed class FieldPainter
        : IDisposable
    {
        #region Nested classes

        class TextSegment
        {
            #region Properties

            public bool Code { get; private set; }

            public string Text { get; private set; }

            #endregion

            #region Construction

            public TextSegment(bool code, string text)
            {
                Code = code;
                Text = text;
            }

            #endregion

            #region Object members

            public override string ToString()
            {
                return $"Code: {Code}, Text: {Text}";
            }

            #endregion
        }

        #endregion

        #region Properties

        /// <summary>
        /// Color of hat sign and code letter.
        /// </summary>
        public Color CodeColor
        {
            get => _codeColor;
            set
            {
                _codeColor = value;
                _codeBrush?.Dispose();
                _codeBrush = new SolidBrush(value);
            }
        }

        /// <summary>
        /// Color of ordinary text.
        /// </summary>
        public Color TextColor
        {
            get => _textColor;
            set
            {
                _textColor = value;
                _textBrush?.Dispose();
                _textBrush = new SolidBrush(value);
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public FieldPainter()
            : this(Color.Red, Color.Black)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public FieldPainter
            (
                Color codeColor,
                Color textColor
            )
        {
            CodeColor = codeColor;
            TextColor = textColor;
        }

        #endregion

        #region Private members

        private Color _codeColor, _textColor;

        private Brush? _codeBrush, _textBrush;

        private static TextSegment[] _SplitText
            (
                string text
            )
        {
            List<TextSegment> result = new List<TextSegment>();

            int start = 0, offset = 0, length = text.Length;
            TextSegment segment;

            while (offset < length)
            {
                if (text[offset] == '^')
                {
                    if (offset != start)
                    {
                        segment = new TextSegment
                            (
                                false,
                                text.Substring(start, offset - start)
                            );
                        result.Add(segment);
                    }

                    if (offset + 1 < length)
                    {
                        segment = new TextSegment
                            (
                                true,
                                text.Substring(offset, 2)
                            );
                        result.Add(segment);

                        start = offset + 2;
                        offset++;
                    }
                    else
                    {
                        segment = new TextSegment
                            (
                                true,
                                text.Substring(offset, 1)
                            );
                        result.Add(segment);

                        start = offset + 1;
                    }
                }

                offset++;
            }

            if (offset != start)
            {
                segment = new TextSegment
                    (
                        false,
                        text.Substring(start, offset - start)
                    );
                result.Add(segment);
            }

            return result.ToArray();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Draw line of field text.
        /// </summary>
        public void DrawLine
            (
                Graphics graphics,
                Font font,
                RectangleF rectangle,
                string? text
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            using var format = new StringFormat
            {
                Alignment = StringAlignment.Near,
                FormatFlags = StringFormatFlags.NoWrap
                              | StringFormatFlags.MeasureTrailingSpaces
            };

            var em = graphics.MeasureString
                (
                    "m",
                    font,
                    rectangle.Size,
                    format
                );
            var em6 = em.Width / 5f;

            var segments = _SplitText(text);
            foreach (var segment in segments)
            {
                var size = graphics.MeasureString
                    (
                        segment.Text,
                        font,
                        rectangle.Size,
                        format
                    );

                var brush = (segment.Code
                    ? _codeBrush
                    : _textBrush)
                    .ThrowIfNull("brush");

                graphics.DrawString
                    (
                        segment.Text,
                        font,
                        brush,
                        rectangle,
                        format
                    );

                /*

                var flags = TextFormatFlags.TextBoxControl
                            | TextFormatFlags.EndEllipsis
                            | TextFormatFlags.NoPrefix
                            | TextFormatFlags.VerticalCenter;
                TextRenderer.DrawText
                    (
                        graphics,
                        text,
                        Grid.Font,
                        rectangle,
                        foreColor,
                        flags
                    );

                */

                var delta = size.Width - em6;
                rectangle.X += delta;
                rectangle.Width -= delta;

                if (rectangle.Width <= 0)
                {
                    break;
                }
            }
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc/>
        public void Dispose()
        {
            if (!ReferenceEquals(_codeBrush, null))
            {
                _codeBrush.Dispose();
                _codeBrush = null;
            }

            if (!ReferenceEquals(_textBrush, null))
            {
                _textBrush.Dispose();
                _textBrush = null;
            }
        }

        #endregion

        #region Object members

        /// <inheritdoc/>
        public override string ToString() => $"CodeColor: {CodeColor}, TextColor: {TextColor}";

        #endregion
    }
}
