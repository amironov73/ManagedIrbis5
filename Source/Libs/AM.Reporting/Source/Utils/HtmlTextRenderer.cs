// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;

#endregion

#nullable enable

namespace AM.Reporting.Utils
{
    public class HtmlTextRenderer : IDisposable
    {
        #region Definitions

        /// <summary>
        /// Context of HTML rendering
        /// It is better to put this structure instead of class' private fields.
        /// For future optimization. Then we can avoid constructor with dozen arguments
        /// </summary>
        public struct RendererContext
        {
            internal string text;
            internal IGraphics g;
            internal FontFamily font;
            internal float size;
            internal FontStyle style; // no keep
            internal Color color; // no keep
            internal Color underlineColor;
            internal RectangleF rect;
            internal bool underlines;
            internal StringFormat format; // no keep
            internal HorizontalAlign horizontalAlign;
            internal VerticalAlign verticalAlign;
            internal ParagraphFormat paragraphFormat;
            internal bool forceJustify;
            internal float scale;
            internal float fontScale;
            internal InlineImageCache cache;
            internal bool isPrinting;
            internal bool isDifferentTabPositions;
            internal bool keepLastLineSpace; // Classic objects need false, translated objects need true
        }

        #endregion

        #region Internal Fields

        public static readonly System.Globalization.CultureInfo CultureInfo =
            System.Globalization.CultureInfo.InvariantCulture;

        #endregion Internal Fields

        #region Private Fields

        private const char SOFT_ENTER = '\u2028';
        private List<RectangleFColor> backgrounds;
        private InlineImageCache cache;
        private RectangleF displayRect;
        private bool everUnderlines;
        private FontFamily font;
        private float fontLineHeight;
        private bool forceJustify;
        private StringFormat format;
        private IGraphics graphics;
        private List<Paragraph> paragraphs;
        private float size;
        private List<LineFColor> strikeouts;
        private string text;
        private Color underlineColor;
        private List<LineFColor> underlines;
        private VerticalAlign _verticalAlign;
        private StyleDescriptor initalStyle;
        private FastString cacheString = new FastString (100);
        private bool isPrinting;
        private bool isDifferentTabPositions;
        internal bool keepLastSpace;

        #endregion Private Fields

        #region Public Properties

        public IEnumerable<RectangleFColor> Backgrounds => backgrounds;

        public RectangleF DisplayRect => displayRect;

        public float Scale { get; }

        public float FontScale { get; set; }

        public HorizontalAlign HorizontalAlign { get; }

        public ParagraphFormat ParagraphFormat { get; }

        public IEnumerable<Paragraph> Paragraphs => paragraphs;

        public bool RightToLeft { get; }

        public IEnumerable<LineFColor> Stikeouts => strikeouts;

        public float[] TabPositions
        {
            get
            {
                return format.GetTabStops (out var firstTabStop);
            }
        }

        public float TabSize
        {
            get
            {
                // re fix tab offset #2823 sorry linux users, on linux firstTab is firstTab not tabSizes[0]
                var tabSizes = TabPositions;
                if (tabSizes.Length > 1)
                {
                    return tabSizes[1];
                }

                return 0;
            }
        }

        public float TabOffset
        {
            get
            {
                // re fix tab offset #2823 sorry linux users, on linux firstTab is firstTab not tabSizes[0]
                var tabSizes = TabPositions;
                if (tabSizes.Length > 0)
                {
                    return tabSizes[0];
                }

                return 0;
            }
        }

        public IEnumerable<LineFColor> Underlines => underlines;

        public bool WordWrap => (format.FormatFlags & StringFormatFlags.NoWrap) == 0;

        #endregion Public Properties

        ////TODO this is a problem with dotnet, because typographic width
        ////float width_dotnet = 2.7f;

        #region Public Constructors

        /// <summary>
        ///  Contexted version of HTML renderer
        /// </summary>
        /// <param name="context"></param>
        public HtmlTextRenderer (RendererContext context)
        {
            text = context.text;
            graphics = context.g;
            font = context.font;
            size = context.size;
            underlineColor = context.underlineColor;
            displayRect = context.rect;
            everUnderlines = context.underlines;
            format = context.format;
            HorizontalAlign = context.horizontalAlign;
            _verticalAlign = context.verticalAlign;
            ParagraphFormat = context.paragraphFormat;
            forceJustify = context.forceJustify;
            Scale = context.scale;
            FontScale = context.fontScale;
            cache = context.cache;
            isPrinting = context.isPrinting;
            isDifferentTabPositions = context.isDifferentTabPositions;
            keepLastSpace = context.keepLastLineSpace;

            paragraphs = new List<Paragraph>();
            RightToLeft = (context.format.FormatFlags & StringFormatFlags.DirectionRightToLeft) ==
                          StringFormatFlags.DirectionRightToLeft;

            // Dispose it
            format = StringFormat.GenericTypographic.Clone() as StringFormat;
            if (RightToLeft)
            {
                format.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
            }

            var tabs = context.format.GetTabStops (out var firstTab);
            format.SetTabStops (firstTab, tabs);
            format.Alignment = StringAlignment.Near;
            format.LineAlignment = StringAlignment.Near;
            format.Trimming = StringTrimming.None;
            format.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.None;

            //FFormat.DigitSubstitutionMethod = StringDigitSubstitute.User;
            //FFormat.DigitSubstitutionLanguage = 0;
            format.FormatFlags |=
                StringFormatFlags.NoClip | StringFormatFlags.FitBlackBox | StringFormatFlags.LineLimit;

            //FFormat.FormatFlags |= StringFormatFlags.NoFontFallback;

            backgrounds = new List<RectangleFColor>();
            underlines = new List<LineFColor>();
            strikeouts = new List<LineFColor>();

            //FDisplayRect.Width -= width_dotnet * scale;

            initalStyle = new StyleDescriptor (context.style, context.color, BaseLine.Normal, font,
                size * FontScale);
            using (var f = initalStyle.GetFont())
            {
                fontLineHeight = f.GetHeight (graphics.Graphics);
            }


            var saveFlags = format.FormatFlags;
            var saveTrimming = format.Trimming;

            // if word wrap is set, ignore trimming
            if (WordWrap)
            {
                format.Trimming = StringTrimming.Word;
            }

            SplitToParagraphs (text);
            AdjustParagraphLines();

            // restore original values
            displayRect = context.rect;
            format.FormatFlags = saveFlags;
            format.Trimming = saveTrimming;
        }

        public HtmlTextRenderer (string text, IGraphics g, FontFamily font, float size,
            FontStyle style, Color color, Color underlineColor, RectangleF rect, bool underlines,
            StringFormat format, HorizontalAlign horizontalAlign, VerticalAlign verticalAlign,
            ParagraphFormat paragraphFormat, bool forceJustify, float scale, float fontScale, InlineImageCache cache,
            bool isPrinting = false, bool isDifferentTabPositions = false)
        {
            this.cache = cache;
            this.Scale = scale;
            this.FontScale = fontScale;
            paragraphs = new List<Paragraph>();
            this.text = text;
            graphics = g;
            this.font = font;
            displayRect = rect;
            RightToLeft = (format.FormatFlags & StringFormatFlags.DirectionRightToLeft) ==
                          StringFormatFlags.DirectionRightToLeft;

            // Dispose it
            this.format = StringFormat.GenericTypographic.Clone() as StringFormat;
            if (RightToLeft)
            {
                this.format.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
            }

            var tabs = format.GetTabStops (out var firstTab);
            this.format.SetTabStops (firstTab, tabs);
            this.format.Alignment = StringAlignment.Near;
            this.format.LineAlignment = StringAlignment.Near;
            this.format.Trimming = StringTrimming.None;
            this.format.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.None;
            this.underlineColor = underlineColor;

            //FFormat.DigitSubstitutionMethod = StringDigitSubstitute.User;
            //FFormat.DigitSubstitutionLanguage = 0;
            this.format.FormatFlags |=
                StringFormatFlags.NoClip | StringFormatFlags.FitBlackBox | StringFormatFlags.LineLimit;

            //FFormat.FormatFlags |= StringFormatFlags.NoFontFallback;
            this.HorizontalAlign = horizontalAlign;
            this._verticalAlign = verticalAlign;
            this.ParagraphFormat = paragraphFormat;
            this.font = font;
            this.size = size;
            this.isPrinting = isPrinting;
            this.isDifferentTabPositions = isDifferentTabPositions;
            everUnderlines = underlines;

            backgrounds = new List<RectangleFColor>();
            this.underlines = new List<LineFColor>();
            strikeouts = new List<LineFColor>();

            //FDisplayRect.Width -= width_dotnet * scale;

            initalStyle = new StyleDescriptor (style, color, BaseLine.Normal, this.font, this.size * this.FontScale);
            using (var f = initalStyle.GetFont())
            {
                fontLineHeight = f.GetHeight (g.Graphics);
            }

            this.forceJustify = forceJustify;

            var saveFlags = this.format.FormatFlags;
            var saveTrimming = this.format.Trimming;

            // if word wrap is set, ignore trimming
            if (WordWrap)
            {
                this.format.Trimming = StringTrimming.Word;
            }

            SplitToParagraphs (text);
            AdjustParagraphLines();

            // restore original values
            displayRect = rect;
            this.format.FormatFlags = saveFlags;
            this.format.Trimming = saveTrimming;
        }

        #endregion Public Constructors

        #region Public Methods

        public void AddUnknownWord (List<CharWithIndex> w, Paragraph paragraph, StyleDescriptor style, int charIndex,
            ref Line line, ref Word word, ref float width)
        {
            if (w[0].Char == ' ')
            {
                if (word == null || word.Type == WordType.Normal)
                {
                    word = new Word (this, line, WordType.WhiteSpace);
                    line.Words.Add (word);
                }

                Run r = new RunText (this, word, style, w, width, charIndex);
                word.Runs.Add (r);
                width += r.Width;
                if (width > displayRect.Width)
                {
                    line = WrapLine (paragraph, line, charIndex, displayRect.Width, ref width);
                }
            }
            else
            {
                if (word == null || word.Type != WordType.Normal)
                {
                    word = new Word (this, line, WordType.Normal);
                    line.Words.Add (word);
                }

                Run r = new RunText (this, word, style, w, width, charIndex);
                word.Runs.Add (r);
                width += r.Width;
                if (width > displayRect.Width)
                {
                    line = WrapLine (paragraph, line, charIndex, displayRect.Width, ref width);
                }
            }
        }

        public float CalcHeight()
        {
            var charsFit = 0;
            return CalcHeight (out charsFit);
        }

        public float CalcHeight (out int charsFit)
        {
            charsFit = -1;
            float height = 0;
            var displayHeight = displayRect.Height;

            float lineSpacing = 0;

            foreach (var paragraph in paragraphs)
            {
                foreach (var line in paragraph.Lines)
                {
                    line.CalcMetrics();
                    height += line.Height;
                    if (charsFit < 0 && height > displayHeight)
                    {
                        charsFit = line.OriginalCharIndex;
                    }

                    height += lineSpacing = line.LineSpacing;
                }
            }

            if (!keepLastSpace) // It looks like TextProcessors keep this value for every line.
            {
                height -= lineSpacing;
            }

            if (charsFit < 0)
            {
                charsFit = text.Length;
            }

            return height;
        }

        public float CalcWidth()
        {
            float width = 0;

            foreach (var paragraph in paragraphs)
            {
                foreach (var line in paragraph.Lines)
                {
                    if (width < line.Width)
                    {
                        width = line.Width;
                    }
                }
            }

            return width;
        }

        #endregion Public Methods

        #region Internal Methods

        /// <summary>
        /// Returns splited string
        /// </summary>
        /// <param name="text">text for splitting</param>
        /// <param name="charactersFitted">index of first character of second string</param>
        /// <param name="result">second part of string</param>
        /// <param name="endOnEnter">returns true if ends on enter</param>
        /// <returns>first part of string</returns>
        internal static string BreakHtml (string text, int charactersFitted, out string result, out bool endOnEnter)
        {
            endOnEnter = false;
            Stack<SimpleFastReportHtmlElement> elements = new Stack<SimpleFastReportHtmlElement>();
            var reader = new SimpleFastReportHtmlReader (text);
            while (reader.IsNotEOF)
            {
                if (reader.Position >= charactersFitted)
                {
                    var firstPart = new StringBuilder();
                    if (reader.Character.Char == SOFT_ENTER)
                    {
                        firstPart.Append (text.Substring (0, reader.LastPosition));
                    }
                    else
                    {
                        firstPart.Append (text.Substring (0, reader.Position));
                    }

                    foreach (var el in elements)
                    {
                        var el2 = new SimpleFastReportHtmlElement (el.name, true);
                        firstPart.Append (el2.ToString());
                    }

                    SimpleFastReportHtmlElement[] arr = elements.ToArray();

                    var secondPart = new StringBuilder();
                    for (var i = arr.Length - 1; i >= 0; i--)
                    {
                        secondPart.Append (arr[i].ToString());
                    }

                    secondPart.Append (text.Substring (reader.Position));
                    endOnEnter = reader.Character.Char == '\n';
                    result = secondPart.ToString();
                    return firstPart.ToString();
                }

                if (!reader.Read())
                {
                    if (reader.Element.isEnd)
                    {
                        var enumIndex = 1;
                        using (Stack<SimpleFastReportHtmlElement>.Enumerator enumerator = elements.GetEnumerator())
                        {
                            while (enumerator.MoveNext())
                            {
                                var el = enumerator.Current;
                                if (el.name == reader.Element.name)
                                {
                                    for (var i = 0; i < enumIndex; i++)
                                    {
                                        elements.Pop();
                                    }

                                    break;
                                }
                                else
                                {
                                    enumIndex++;
                                }
                            }
                        }
                    }
                    else if (!reader.Element.IsSelfClosed)
                    {
                        elements.Push (reader.Element);
                    }
                }
            }

            result = "";
            return text;
        }

        internal void Draw()
        {
            // set clipping
            var state = graphics.Save();

            //RectangleF rect = new RectangleF(FDisplayRect.Location, SizeF.Add(FDisplayRect.Size, new SizeF(width_dotnet, 0)));
            //FGraphics.SetClip(rect, CombineMode.Intersect);
            graphics.SetClip (displayRect, CombineMode.Intersect);

            // reset alignment
            //StringAlignment saveAlign = FFormat.Alignment;
            //StringAlignment saveLineAlign = FFormat.LineAlignment;
            //FFormat.Alignment = StringAlignment.Near;
            //FFormat.LineAlignment = StringAlignment.Near;

            //if (FRightToLeft)
            //    foreach (RectangleFColor rect in FBackgrounds)
            //        using (Brush brush = new SolidBrush(rect.Color))
            //            FGraphics.FillRectangle(brush, rect.Left - rect.Width, rect.Top, rect.Width, rect.Height);
            //else
            foreach (var rect in backgrounds)
            {
                using (Brush brush = new SolidBrush (rect.Color))
                    graphics.FillRectangle (brush, rect.Left, rect.Top, rect.Width, rect.Height);
            }

            foreach (var p in paragraphs)
            {
                foreach (var l in p.Lines)
                {
                    //#if DEBUG
                    //                    FGraphics.DrawRectangle(Pens.Blue, FDisplayRect.Left, l.Top, FDisplayRect.Width, l.Height);
                    //#endif
                    foreach (var w in l.Words)
                    {
                        switch (w.Type)
                        {
                            case WordType.Normal:
                                foreach (var r in w.Runs)
                                {
                                    r.Draw();
                                }

                                break;
                        }
                    }
                }
            }

            //if (RightToLeft)
            //{
            //    foreach (LineFColor line in FUnderlines)
            //        using (Pen pen = new Pen(line.Color, line.Width))
            //            FGraphics.DrawLine(pen, 2 * line.Left - line.Right, line.Top, line.Left, line.Top);

            //    foreach (LineFColor line in FStrikeouts)
            //        using (Pen pen = new Pen(line.Color, line.Width))
            //            FGraphics.DrawLine(pen, 2 * line.Left - line.Right, line.Top, line.Left, line.Top);
            //}
            //else
            //{
            foreach (var line in underlines)
            {
                using (var pen = new Pen (line.Color, line.Width))
                    graphics.DrawLine (pen, line.Left, line.Top, line.Right, line.Top);
            }

            foreach (var line in strikeouts)
            {
                using (var pen = new Pen (line.Color, line.Width))
                    graphics.DrawLine (pen, line.Left, line.Top, line.Right, line.Top);
            }

            //}

            // restore alignment and clipping
            //FFormat.Alignment = saveAlign;
            //FFormat.LineAlignment = saveLineAlign;
            graphics.Restore (state);
        }

        #endregion Internal Methods

        #region Private Methods

        private void AdjustParagraphLines()
        {
            // calculate text height
            float height = 0;
            height = CalcHeight();

            // calculate Y offset
            var offsetY = displayRect.Top;
            if (_verticalAlign == VerticalAlign.Center)
            {
                offsetY += (displayRect.Height - height) / 2;
            }
            else if (_verticalAlign == VerticalAlign.Bottom)
            {
                offsetY += (displayRect.Height - height) - 1;
            }

            for (var i = 0; i < paragraphs.Count; i++)
            {
                var paragraph = paragraphs[i];
                paragraph.AlignLines (i == paragraphs.Count - 1 && forceJustify);

                // adjust line tops
                foreach (var line in paragraph.Lines)
                {
                    line.Top = offsetY;
                    line.MakeUnderlines();
                    line.MakeStrikeouts();
                    line.MakeBackgrounds();
                    offsetY += line.Height + line.LineSpacing;
                }
            }
        }

        private void CssStyle (StyleDescriptor style, Dictionary<string, string> dict)
        {
            if (dict == null)
            {
                return;
            }

            if (dict.TryGetValue ("font-size", out var tStr))
            {
                if (EndsWith (tStr, "px"))
                {
                    try
                    {
                        style.Size = FontScale * 0.75f *
                                     float.Parse (tStr.Substring (0, tStr.Length - 2), CultureInfo);
                    }
                    catch
                    {
                    }
                }
                else if (EndsWith (tStr, "pt"))
                {
                    try
                    {
                        style.Size = FontScale * float.Parse (tStr.Substring (0, tStr.Length - 2), CultureInfo);
                    }
                    catch
                    {
                    }
                }
                else if (EndsWith (tStr, "em"))
                {
                    try
                    {
                        style.Size *= float.Parse (tStr.Substring (0, tStr.Length - 2), CultureInfo);
                    }
                    catch
                    {
                    }
                }
            }

            if (dict.TryGetValue ("font-family", out tStr))
            {
                style.Font = new FontFamily (tStr);
            }

            if (dict.TryGetValue ("color", out tStr))
            {
                if (StartsWith (tStr, "#"))
                {
                    try
                    {
                        style.Color = Color.FromArgb ((int)(0xFF000000 + uint.Parse (tStr.Substring (1),
                            System.Globalization.NumberStyles.HexNumber)));
                    }
                    catch
                    {
                    }
                }
                else if (StartsWith (tStr, "rgba"))
                {
                    var i1 = tStr.IndexOf ('(');
                    var i2 = tStr.IndexOf (')');
                    string[] strs = tStr.Substring (i1 + 1, i2 - i1 - 1).Split (',');
                    if (strs.Length == 4)
                    {
                        float r, g, b, a;
                        try
                        {
                            r = float.Parse (strs[0], CultureInfo);
                            g = float.Parse (strs[1], CultureInfo);
                            b = float.Parse (strs[2], CultureInfo);
                            a = float.Parse (strs[3], CultureInfo);
                            style.Color = Color.FromArgb ((int)(a * 0xFF), (int)r, (int)g, (int)b);
                        }
                        catch
                        {
                        }
                    }
                }
                else if (StartsWith (tStr, "rgb"))
                {
                    var i1 = tStr.IndexOf ('(');
                    var i2 = tStr.IndexOf (')');
                    string[] strs = tStr.Substring (i1 + 1, i2 - i1 - 1).Split (',');
                    if (strs.Length == 3)
                    {
                        float r, g, b;
                        try
                        {
                            r = float.Parse (strs[0], CultureInfo);
                            g = float.Parse (strs[1], CultureInfo);
                            b = float.Parse (strs[2], CultureInfo);
                            style.Color = Color.FromArgb ((int)r, (int)g, (int)b);
                        }
                        catch
                        {
                        }
                    }
                }
                else
                {
                    style.Color = Color.FromName (tStr);
                }
            }

            if (dict.TryGetValue ("background-color", out tStr))
            {
                if (StartsWith (tStr, "#"))
                {
                    try
                    {
                        style.BackgroundColor = Color.FromArgb ((int)(0xFF000000 +
                                                                      uint.Parse (tStr.Substring (1),
                                                                          System.Globalization.NumberStyles
                                                                              .HexNumber)));
                    }
                    catch
                    {
                    }
                }
                else if (StartsWith (tStr, "rgba"))
                {
                    var i1 = tStr.IndexOf ('(');
                    var i2 = tStr.IndexOf (')');
                    string[] strs = tStr.Substring (i1 + 1, i2 - i1 - 1).Split (',');
                    if (strs.Length == 4)
                    {
                        float r, g, b, a;
                        try
                        {
                            r = float.Parse (strs[0], CultureInfo);
                            g = float.Parse (strs[1], CultureInfo);
                            b = float.Parse (strs[2], CultureInfo);
                            a = float.Parse (strs[3], CultureInfo);
                            style.BackgroundColor = Color.FromArgb ((int)(a * 0xFF), (int)r, (int)g, (int)b);
                        }
                        catch
                        {
                        }
                    }
                }
                else if (StartsWith (tStr, "rgb"))
                {
                    var i1 = tStr.IndexOf ('(');
                    var i2 = tStr.IndexOf (')');
                    string[] strs = tStr.Substring (i1 + 1, i2 - i1 - 1).Split (',');
                    if (strs.Length == 3)
                    {
                        float r, g, b;
                        try
                        {
                            r = float.Parse (strs[0], CultureInfo);
                            g = float.Parse (strs[1], CultureInfo);
                            b = float.Parse (strs[2], CultureInfo);
                            style.BackgroundColor = Color.FromArgb ((int)r, (int)g, (int)b);
                        }
                        catch
                        {
                        }
                    }
                }
                else
                {
                    style.BackgroundColor = Color.FromName (tStr);
                }
            }
        }

        private bool EndsWith (string str1, string str2)
        {
            var len1 = str1.Length;
            var len2 = str2.Length;
            if (len1 < len2)
            {
                return false;
            }

            switch (len2)
            {
                case 0: return true;
                case 1: return str1[len1 - 1] == str2[len2 - 1];
                case 2: return str1[len1 - 1] == str2[len2 - 1] && str1[len1 - 2] == str2[len2 - 2];
                case 3:
                    return str1[len1 - 1] == str2[len2 - 1] && str1[len1 - 2] == str2[len2 - 2] &&
                           str1[len1 - 3] == str2[len2 - 3];
                case 4:
                    return str1[len1 - 1] == str2[len2 - 1] && str1[len1 - 2] == str2[len2 - 2] &&
                           str1[len1 - 3] == str2[len2 - 3] && str1[len1 - 4] == str2[len2 - 4];
                default: return str1.EndsWith (str2);
            }
        }

        private float GetTabPosition (float pos)
        {
            var tabOffset = TabOffset;
            var tabSize = TabSize;
            var tabPosition = (int)((pos - tabOffset) / tabSize);
            if (pos < tabOffset)
            {
                return tabOffset;
            }

            return (tabPosition + 1) * tabSize + tabOffset;
        }

        private float GetTabPosition (float pos, int tabIndex)
        {
            float tabOffset = 0;
            float tabSize;
            var tabsPos = format.GetTabStops (out var firstTabOffset);
            if (tabIndex <= 1)
            {
                tabOffset = TabOffset;
                tabSize = TabSize;
            }
            else
            {
                for (var i = 0; i < tabIndex; i++)
                {
                    tabOffset += tabsPos[i];
                }

                tabSize = tabsPos[tabIndex];
            }

            var tabPosition = (int)((pos - tabOffset) / tabSize);
            if (pos < tabOffset)
            {
                return tabOffset;
            }

            return (tabPosition + 1) * tabSize + tabOffset;
        }

        private void SplitToParagraphs (string text)
        {
            Stack<SimpleFastReportHtmlElement> elements = new Stack<SimpleFastReportHtmlElement>();
            var reader = new SimpleFastReportHtmlReader (this.text);
            var currentWord = new List<CharWithIndex>();
            var width = ParagraphFormat.SkipFirstLineIndent ? 0 : ParagraphFormat.FirstLineIndent;
            var paragraph = new Paragraph (this);
            var charIndex = 0;
            var tabIndex = 0;
            var line = new Line (this, paragraph, charIndex);
            paragraph.Lines.Add (line);
            paragraphs.Add (paragraph);
            Word word = null;
            var style = new StyleDescriptor (initalStyle);

            //bool softReturn = false;
            //CharWithIndex softReturnChar = new CharWithIndex();

            while (reader.IsNotEOF)
            {
                if (reader.Read())
                {
                    switch (reader.Character.Char)
                    {
                        case ' ':
                            if (word == null)
                            {
                                word = new Word (this, line, WordType.WhiteSpace);
                                line.Words.Add (word);
                            }

                            if (word.Type == WordType.WhiteSpace)
                            {
                                currentWord.Add (reader.Character);
                            }
                            else
                            {
                                if (currentWord.Count > 0)
                                {
                                    Run r = new RunText (this, word, style, currentWord, width, charIndex);
                                    word.Runs.Add (r);
                                    currentWord.Clear();
                                    width += r.Width;
                                    if (width > displayRect.Width)
                                    {
                                        line = WrapLine (paragraph, line, charIndex, displayRect.Width, ref width);
                                    }
                                }

                                currentWord.Add (reader.Character);
                                word = new Word (this, line, WordType.WhiteSpace);
                                line.Words.Add (word);
                                charIndex = reader.LastPosition;
                            }

                            break;

                        case '\t':
                            if (word != null)
                            {
                                if (currentWord.Count > 0)
                                {
                                    Run r = new RunText (this, word, style, currentWord, width, charIndex);
                                    word.Runs.Add (r);
                                    currentWord.Clear();
                                    width += r.Width;
                                    if (width > displayRect.Width)
                                    {
                                        line = WrapLine (paragraph, line, charIndex, displayRect.Width, ref width);
                                    }
                                }
                            }
                            else
                            {
                                if (currentWord.Count > 0)
                                {
                                    AddUnknownWord (currentWord, paragraph, style, charIndex, ref line, ref word,
                                        ref width);
                                }
                            }

                            charIndex = reader.LastPosition;

                            word = new Word (this, line, WordType.Tab);


                            Run tabRun = new RunText (this, word, style,
                                new List<CharWithIndex> (new CharWithIndex[] { reader.Character }), width, charIndex);
                            word.Runs.Add (tabRun);
                            var width2 = GetTabPosition (width);
                            if (isDifferentTabPositions)
                            {
                                width2 = GetTabPosition (width, tabIndex);
                            }

                            if (width2 < width)
                            {
                                width2 = width;
                            }

                            if (line.Words.Count > 0 && width2 > displayRect.Width)
                            {
                                tabRun.Left = 0;
                                line = new Line (this, paragraph, charIndex);
                                tabIndex = 0;
                                paragraph.Lines.Add (line);
                                width = 0;
                                width2 = GetTabPosition (width);
                                if (isDifferentTabPositions)
                                {
                                    width2 = GetTabPosition (width, tabIndex);
                                }
                            }

                            tabIndex++;
                            line.Words.Add (word);
                            tabRun.Width = width2 - width;
                            width = width2;
                            word = null;
                            break;

                        case SOFT_ENTER: //soft enter
                            if (word != null)
                            {
                                if (currentWord.Count > 0)
                                {
                                    Run r = new RunText (this, word, style, currentWord, width, charIndex);
                                    word.Runs.Add (r);
                                    currentWord.Clear();
                                    width += r.Width;
                                    if (width > displayRect.Width)
                                    {
                                        line = WrapLine (paragraph, line, charIndex, displayRect.Width, ref width);
                                    }
                                }
                            }
                            else
                            {
                                if (currentWord.Count > 0)
                                {
                                    AddUnknownWord (currentWord, paragraph, style, charIndex, ref line, ref word,
                                        ref width);
                                }
                            }

                            charIndex = reader.Position;

                            //currentWord.Append(' ')
                            //RunText runText = new RunText(this, word, style, new List<CharWithIndex>(new CharWithIndex[] { reader.Character }), width, charIndex);
                            //runText.Width = 0;
                            //word.Runs.Add(runText);
                            line = new Line (this, paragraph, charIndex);
                            word = null;
                            width = 0;
                            currentWord.Clear();
                            paragraph.Lines.Add (line);
                            break;

                        case '\n':
                            if (word != null)
                            {
                                if (currentWord.Count > 0)
                                {
                                    Run r = new RunText (this, word, style, currentWord, width, charIndex);
                                    word.Runs.Add (r);
                                    currentWord.Clear();
                                    width += r.Width;
                                    if (width > displayRect.Width)
                                    {
                                        line = WrapLine (paragraph, line, charIndex, displayRect.Width, ref width);
                                    }
                                }
                            }
                            else
                            {
                                if (currentWord.Count > 0)
                                {
                                    AddUnknownWord (currentWord, paragraph, style, charIndex, ref line, ref word,
                                        ref width);
                                }
                            }

                            charIndex = reader.Position;

                            paragraph = new Paragraph (this);
                            paragraphs.Add (paragraph);
                            line = new Line (this, paragraph, charIndex);
                            word = null;
                            width = ParagraphFormat.FirstLineIndent;
                            paragraph.Lines.Add (line);
                            break;

                        case '\r': //ignore
                            break;

                        default:
                            if (word == null)
                            {
                                word = new Word (this, line, WordType.Normal);
                                line.Words.Add (word);
                            }

                            if (word.Type == WordType.Normal)
                            {
                                currentWord.Add (reader.Character);
                            }
                            else
                            {
                                if (currentWord.Count > 0)
                                {
                                    Run r = new RunText (this, word, style, currentWord, width, charIndex);
                                    word.Runs.Add (r);
                                    currentWord.Clear();
                                    width += r.Width;
                                    if (width > displayRect.Width)
                                    {
                                        line = WrapLine (paragraph, line, charIndex, displayRect.Width, ref width);
                                    }
                                }

                                currentWord.Add (reader.Character);
                                word = new Word (this, line, WordType.Normal);
                                line.Words.Add (word);
                                charIndex = reader.LastPosition;
                            }

                            break;
                    }
                }
                else
                {
                    var newStyle = new StyleDescriptor (initalStyle);
                    var element = reader.Element;

                    if (!element.IsSelfClosed)
                    {
                        if (element.isEnd)
                        {
                            var enumIndex = 1;
                            using (Stack<SimpleFastReportHtmlElement>.Enumerator enumerator = elements.GetEnumerator())
                            {
                                while (enumerator.MoveNext())
                                {
                                    var el = enumerator.Current;
                                    if (el.name == element.name)
                                    {
                                        for (var i = 0; i < enumIndex; i++)
                                        {
                                            elements.Pop();
                                        }

                                        break;
                                    }
                                    else
                                    {
                                        enumIndex++;
                                    }
                                }
                            }
                        }
                        else
                        {
                            elements.Push (element);
                        }

                        SimpleFastReportHtmlElement[] arr = elements.ToArray();
                        for (var i = arr.Length - 1; i >= 0; i--)
                        {
                            var el = arr[i];
                            switch (el.name)
                            {
                                case "b":
                                    newStyle.FontStyle |= FontStyle.Bold;
                                    break;

                                case "i":
                                    newStyle.FontStyle |= FontStyle.Italic;
                                    break;

                                case "u":
                                    newStyle.FontStyle |= FontStyle.Underline;
                                    break;

                                case "sub":
                                    newStyle.BaseLine = BaseLine.Subscript;
                                    break;

                                case "sup":
                                    newStyle.BaseLine = BaseLine.Superscript;
                                    break;

                                case "strike":
                                    newStyle.FontStyle |= FontStyle.Strikeout;
                                    break;

                                //case "font":
                                //    {
                                //        string color = null;
                                //        string face = null;
                                //        string size = null;
                                //        if (el.Attributes != null)
                                //        {
                                //            el.Attributes.TryGetValue("color", out color);
                                //            el.Attributes.TryGetValue("face", out face);
                                //            el.Attributes.TryGetValue("size", out size);
                                //        }

                                //        if (color != null)
                                //        {
                                //            if (color.StartsWith("\"") && color.EndsWith("\""))
                                //                color = color.Substring(1, color.Length - 2);
                                //            if (color.StartsWith("#"))
                                //            {
                                //                newStyle.Color = Color.FromArgb((int)(0xFF000000 + uint.Parse(color.Substring(1), System.Globalization.NumberStyles.HexNumber)));
                                //            }
                                //            else
                                //            {
                                //                newStyle.Color = Color.FromName(color);
                                //            }
                                //        }
                                //        if (face != null)
                                //            newStyle.Font = face;
                                //        if (size != null)
                                //        {
                                //            try
                                //            {
                                //                size = size.Trim(' ');
                                //                newStyle.Size = (float)Converter.FromString(typeof(float), size) * FFontScale;
                                //            }
                                //            catch
                                //            {
                                //                newStyle.Size = FSize * FFontScale;
                                //            }
                                //        }
                                //    }
                                //    break;
                            }

                            CssStyle (newStyle, el.Style);
                        }

                        if (currentWord.Count > 0)
                        {
                            AddUnknownWord (currentWord, paragraph, style, charIndex, ref line, ref word, ref width);
                            currentWord.Clear();
                            charIndex = reader.LastPosition;
                        }

                        style = newStyle;
                    }
                    else
                    {
                        switch (element.name)
                        {
                            case "img":
                                if (element.attributes != null && element.attributes.ContainsKey ("src"))
                                {
                                    float img_width = -1;
                                    float img_height = -1;

                                    if (element.attributes.TryGetValue ("width", out var tStr))
                                    {
                                        try
                                        {
                                            img_width = float.Parse (tStr,
                                                System.Globalization.CultureInfo.InstalledUICulture);
                                        }
                                        catch
                                        {
                                        }
                                    }

                                    if (element.attributes.TryGetValue ("height", out tStr))
                                    {
                                        try
                                        {
                                            img_height = float.Parse (tStr,
                                                System.Globalization.CultureInfo.InstalledUICulture);
                                        }
                                        catch
                                        {
                                        }
                                    }

                                    if (currentWord.Count > 0)
                                    {
                                        AddUnknownWord (currentWord, paragraph, style, charIndex, ref line, ref word,
                                            ref width);
                                        currentWord.Clear();
                                    }

                                    if (word == null || word.Type != WordType.Normal)
                                    {
                                        word = new Word (this, line, WordType.Normal);
                                        line.Words.Add (word);
                                        charIndex = reader.LastPosition;
                                    }

                                    Run r = new RunImage (this, word, element.attributes["src"], style, width,
                                        reader.LastPosition, img_width, img_height);
                                    word.Runs.Add (r);
                                    width += r.Width;
                                    if (width > displayRect.Width)
                                    {
                                        line = WrapLine (paragraph, line, charIndex, displayRect.Width, ref width);
                                    }
                                }

                                break;
                        }
                    }
                }
            }

            if (currentWord.Count > 0)
            {
                AddUnknownWord (currentWord, paragraph, style, charIndex, ref line, ref word, ref width);
            }
        }

        private bool StartsWith (string str1, string str2)
        {
            if (str1.Length < str2.Length)
            {
                return false;
            }

            switch (str2.Length)
            {
                case 0: return true;
                case 1: return str1[0] == str2[0];
                case 2: return str1[0] == str2[0] && str1[1] == str2[1];
                case 3: return str1[0] == str2[0] && str1[1] == str2[1] && str1[2] == str2[2];
                case 4: return str1[0] == str2[0] && str1[1] == str2[1] && str1[2] == str2[2] && str1[3] == str2[3];
                default: return str1.StartsWith (str2);
            }
        }

        /// <summary>
        /// Check the line, and if last word is able to move next line, move it.
        /// e.g. white space won't move to next line.
        /// If word is not moved return current line.
        /// else return new line
        /// </summary>
        /// <param name="paragraph">the paragraph for lines</param>
        /// <param name="line">the line with extra words</param>
        /// <param name="wordCharIndex">the index of start last word in this line</param>
        /// <param name="availableWidth">width to place words</param>
        /// <param name="newWidth">ref to current line width</param>
        /// <returns></returns>
        private Line WrapLine (Paragraph paragraph, Line line, int wordCharIndex, float availableWidth,
            ref float newWidth)
        {
            if (line.Words.Count == 0)
            {
                return line;
            }

            if (line.Words is [{ Type: WordType.Normal }])
            {
                var word = line.Words[0];
                var width = word.Runs.Count > 0 ? word.Runs[0].Left : 0;
                /* Foreach runs, while run in available space next run
                 * if run begger then space split run and generate new word and run
                 */
                var newWord = new Word (word.Renderer, line, word.Type);
                line.Words.Clear();
                line.Words.Add (newWord);
                foreach (var run in word.Runs)
                {
                    width += run.Width;
                    if (width <= availableWidth || availableWidth < 0)
                    {
                        newWord.Runs.Add (run);
                        run.Word = newWord;
                    }
                    else
                    {
                        var secondPart = run;
                        while (secondPart != null)
                        {
                            var firstPart = secondPart.Split (availableWidth - run.Left, out secondPart);
                            if (firstPart != null)
                            {
                                newWord.Runs.Clear();
                                newWord.Runs.Add (firstPart);
                                firstPart.Word = newWord;
                            }
                            else if (newWord.Runs.Count == 0)
                            {
                                newWord.Runs.Add (run);
                                run.Word = newWord;
                                secondPart = null;
                            }

                            if (secondPart != null)
                            {
                                line = new Line (line.Renderer, paragraph, secondPart.CharIndex);
                                paragraph.Lines.Add (line);
                                newWord = new Word (newWord.Renderer, line, newWord.Type);
                                line.Words.Add (newWord);
                                secondPart.Left = 0;
                                width = secondPart.Width;
                                secondPart.Word = newWord;
                                newWord.Runs.Add (secondPart);
                                if (width < availableWidth)
                                {
                                    secondPart = null;
                                }
                            }
                        }
                    }
                }

                return line;
            }
            else if (line.Words[line.Words.Count - 1].Type == WordType.WhiteSpace)
            {
                return line;
            }
            else
            {
                var lastWord = line.Words[line.Words.Count - 1];
                line.Words.RemoveAt (line.Words.Count - 1);
                var result = new Line (this, paragraph, wordCharIndex);
                paragraph.Lines.Add (result);
                newWidth = 0;
                result.Words.Add (lastWord);
                lastWord.Line = result;

                foreach (var r in lastWord.Runs)
                {
                    r.Left = newWidth;
                    newWidth += r.Width;
                }

                return result;
            }
        }

        #endregion Private Methods

        #region Public Enums

        public enum WordType
        {
            Normal,
            WhiteSpace,
            Tab,
        }

        #endregion Public Enums

        #region Internal Enums

        /// <summary>
        /// Represents character placement.
        /// </summary>
        public enum BaseLine
        {
            Normal,
            Subscript,
            Superscript
        }

        #endregion Internal Enums

        #region Public Structs

        public struct CharWithIndex
        {
            #region Public Fields

            public char Char;
            public int Index;

            #endregion Public Fields

            #region Public Constructors

            public CharWithIndex (char v, int fPosition)
            {
                Char = v;
                Index = fPosition;
            }

            #endregion Public Constructors
        }

#if READONLY_STRUCTS
        public readonly struct LineFColor
#else
        public struct LineFColor
#endif
        {
            #region Public Fields

            public readonly Color Color;
            public readonly float Left;
            public readonly float Right;
            public readonly float Top;
            public readonly float Width;

            #endregion Public Fields

            #region Public Constructors

            public LineFColor (float left, float top, float right, float width, Color color)
            {
                Left = left;
                Top = top;
                Right = right;
                Width = width;
                Color = color;
            }

            public LineFColor (float left, float top, float right, float width, byte R, byte G, byte B)
                : this (left, top, right, width, Color.FromArgb (R, G, B))
            {
            }

            public LineFColor (float left, float top, float right, float width, byte R, byte G, byte B, byte A)
                : this (left, top, right, width, Color.FromArgb (A, R, G, B))
            {
            }

            public LineFColor (float left, float top, float right, float width, int R, int G, int B)
                : this (left, top, right, width, Color.FromArgb (R, G, B))
            {
            }

            public LineFColor (float left, float top, float right, float width, int R, int G, int B, int A)
                : this (left, top, right, width, Color.FromArgb (A, R, G, B))
            {
            }

            #endregion Public Constructors
        }

#if READONLY_STRUCTS
        public readonly struct RectangleFColor
#else
        public struct RectangleFColor
#endif
        {
            #region Public Fields

            public readonly Color Color;
            public readonly float Height;
            public readonly float Left;
            public readonly float Top;
            public readonly float Width;

            #endregion Public Fields

            #region Public Constructors

            public RectangleFColor (float left, float top, float width, float height, Color color)
            {
                Left = left;
                Top = top;
                Width = width;
                Height = height;
                Color = color;
            }

            public RectangleFColor (float left, float top, float width, float height, byte R, byte G, byte B)
                : this (left, top, width, height, Color.FromArgb (R, G, B))
            {
            }

            public RectangleFColor (float left, float top, float width, float height, byte R, byte G, byte B, byte A)
                : this (left, top, width, height, Color.FromArgb (A, R, G, B))
            {
            }

            public RectangleFColor (float left, float top, float width, float height, int R, int G, int B)
                : this (left, top, width, height, Color.FromArgb (R, G, B))
            {
            }

            public RectangleFColor (float left, float top, float width, float height, int R, int G, int B, int A)
                : this (left, top, width, height, Color.FromArgb (A, R, G, B))
            {
            }

            #endregion Public Constructors
        }

        #endregion Public Structs

        #region Public Classes

        public class Line
        {
            #region Private Fields

            private float top;

            #endregion Private Fields

            #region Public Properties

            public float BaseLine { get; set; }

            public float Height { get; set; }

            public HorizontalAlign HorizontalAlign { get; private set; }

            public float LineSpacing { get; set; }

            public int OriginalCharIndex { get; set; }

            public Paragraph Paragraph { get; set; }

            public HtmlTextRenderer Renderer { get; }


            public float Top
            {
                get => top;
                set
                {
                    top = value;
                    foreach (var w in Words)
                    {
                        foreach (var r in w.Runs)
                        {
                            float shift = 0;
                            if (r.Style.BaseLine == HtmlTextRenderer.BaseLine.Subscript)
                            {
                                shift += r.Height * 0.45f;
                            }
                            else if (r.Style.BaseLine == HtmlTextRenderer.BaseLine.Superscript)
                            {
                                shift -= r.BaseLine - r.Height * 0.15f;
                            }

                            r.Top = top + BaseLine - r.BaseLine + shift;
                        }
                    }
                }
            }

            public float Width { get; private set; }

            public List<Word> Words { get; }

            #endregion Public Properties

            #region Public Constructors

            public Line (HtmlTextRenderer renderer, Paragraph paragraph, int charIndex)
            {
                Words = new List<Word>();
                this.Renderer = renderer;
                this.Paragraph = paragraph;
                OriginalCharIndex = charIndex;
            }

            #endregion Public Constructors

            #region Public Methods

            public override string ToString()
            {
                return string.Format ("Words[{0}]", Words.Count);
            }

            #endregion Public Methods

            #region Internal Methods

            internal void AlignWords (HorizontalAlign align)
            {
                HorizontalAlign = align;
                var width = CalcWidth();
                var left = Words.Count > 0 && Words[0].Runs.Count > 0 ? Words[0].Runs[0].Left : 0;
                width += left;
                this.Width = width;
                switch (align)
                {
                    case HorizontalAlign.Left:
                        break;

                    case HorizontalAlign.Right:
                    {
                        var delta = Renderer.displayRect.Width - width;
                        foreach (var w in Words)
                        {
                            foreach (var r in w.Runs)
                            {
                                r.Left += delta;
                            }
                        }
                    }
                        break;

                    case HorizontalAlign.Center:
                    {
                        var delta = (Renderer.displayRect.Width - width) / 2f;
                        foreach (var w in Words)
                        {
                            foreach (var r in w.Runs)
                            {
                                r.Left += delta;
                            }
                        }
                    }
                        break;

                    case HorizontalAlign.Justify:
                    {
                        var spaces = 0;
                        var tab_index = -1;
                        var isWordExistAfterTab = true;
                        for (var i = 0; i < Words.Count - 1; i++)
                        {
                            if (isWordExistAfterTab)
                            {
                                if (Words[i].Type == WordType.WhiteSpace)
                                {
                                    foreach (var r in Words[i].Runs)
                                    {
                                        if (r is RunText runText)
                                        {
                                            spaces += runText.Text.Length;
                                        }
                                    }
                                }
                            }
                            else if (Words[i].Type == WordType.Normal)
                            {
                                isWordExistAfterTab = true;
                            }

                            if (Words[i].Type == WordType.Tab)
                            {
                                spaces = 0;
                                tab_index = i;
                                isWordExistAfterTab = false;
                            }
                        }

                        if (spaces > 0)
                        {
                            var space_width = (Renderer.displayRect.Width - width) / spaces;

                            for (var i = 0; i < Words.Count; i++)
                            {
                                var w = Words[i];
                                if (w.Type == WordType.WhiteSpace)
                                {
                                    foreach (var r in w.Runs)
                                    {
                                        if (i > tab_index && r is RunText runText)
                                        {
                                            runText.Width += space_width * runText.Text.Length;
                                        }

                                        r.Left = left;
                                        left += r.Width;
                                    }
                                }
                                else
                                {
                                    foreach (var r in w.Runs)
                                    {
                                        r.Left = left;
                                        left += r.Width;
                                    }
                                }
                            }
                        }
                    }

                        break;
                }

                if (Renderer.RightToLeft)
                {
                    var rectRight = Renderer.displayRect.Right;
                    foreach (var w in Words)
                    {
                        foreach (var r in w.Runs)
                        {
                            r.Left = rectRight - r.Left;
                        }
                    }
                }
                else
                {
                    var rectLeft = Renderer.displayRect.Left;
                    foreach (var w in Words)
                    {
                        foreach (var r in w.Runs)
                        {
                            r.Left += rectLeft;
                        }
                    }
                }
            }

            internal void CalcMetrics()
            {
                BaseLine = 0;
                foreach (var word in Words)
                {
                    word.CalcMetrics();
                    BaseLine = Math.Max (BaseLine, word.BaseLine);
                }

                Height = Renderer.fontLineHeight;
                float decent = 0;
                foreach (var word in Words)
                {
                    decent = Math.Max (decent, word.Descent);
                }

                if (BaseLine + decent > 0.01)
                {
                    Height = BaseLine + decent;
                }

                switch (Renderer.ParagraphFormat.LineSpacingType)
                {
                    case LineSpacingType.AtLeast:
                        if (Height < Renderer.ParagraphFormat.LineSpacing)
                        {
                            LineSpacing = Renderer.ParagraphFormat.LineSpacing - Height;
                        }

                        break;

                    case LineSpacingType.Single:
                        break;

                    case LineSpacingType.Multiple:
                        LineSpacing = Height * (Renderer.ParagraphFormat.LineSpacingMultiple - 1);
                        break;

                    case LineSpacingType.Exactly:
                        LineSpacing = Renderer.ParagraphFormat.LineSpacing - Height;
                        break;
                }
            }

            internal void MakeBackgrounds()
            {
                var list = Renderer.backgrounds;
                if (Renderer.RightToLeft)
                {
                    foreach (var word in Words)
                    {
                        foreach (var run in word.Runs)
                        {
                            if (run.Style.BackgroundColor.A > 0)
                            {
                                list.Add (new RectangleFColor (
                                        run.Left - run.Width, top, run.Width, Height, run.Style.BackgroundColor
                                    ));
                            }
                        }
                    }
                }
                else
                {
                    foreach (var word in Words)
                    {
                        foreach (var run in word.Runs)
                        {
                            if (run.Style.BackgroundColor.A > 0)
                            {
                                list.Add (new RectangleFColor (
                                        run.Left, top, run.Width, Height, run.Style.BackgroundColor
                                    ));
                            }
                        }
                    }
                }
            }

            internal void MakeEverUnderlines()
            {
                OwnHashSet<StyleDescriptor> styles = new OwnHashSet<StyleDescriptor>();
                float size = 0;
                float underline = 0;
                foreach (var word in Words)
                {
                    foreach (var run in word.Runs)
                    {
                        if (!styles.Contains (run.Style))
                        {
                            styles.Add (run.Style);
                            size += run.Style.Size;
                            underline += run.Descent / 2;
                        }
                    }
                }

                if (styles.Count == 0 || BaseLine <= 0.01)
                {
                    using (var ff = Renderer.initalStyle.GetFont())
                    {
                        float lineSpace = ff.FontFamily.GetLineSpacing (Renderer.initalStyle.FontStyle);
                        float ascent = ff.FontFamily.GetCellAscent (Renderer.initalStyle.FontStyle);
                        BaseLine = Height * ascent / lineSpace;
                        var FDescent = Height - BaseLine;
                        underline = FDescent / 2;
                        size = ff.Size;
                    }
                }
                else
                {
                    size /= styles.Count;
                    underline /= styles.Count;
                }

                var fixScale = Renderer.Scale / Renderer.FontScale;

                Renderer.underlines.Add (new LineFColor (
                        Renderer.displayRect.Left, Top + BaseLine + underline, Renderer.displayRect.Right,
                        size * 0.1f * fixScale, Renderer.underlineColor
                    ));
            }

            internal void MakeStrikeouts()
            {
                var lines = Renderer.strikeouts;
                var fixScale = Renderer.Scale / Renderer.FontScale;
                if (Renderer.RightToLeft)
                {
                    foreach (var word in Words)
                    {
                        foreach (var r in word.Runs)
                        {
                            if ((r.Style.FontStyle & FontStyle.Strikeout) == FontStyle.Strikeout)
                            {
                                lines.Add (new LineFColor (
                                    r.Left - r.Width, r.Top + r.BaseLine / 3f * 2f, r.Left, r.Style.Size * 0.1f * fixScale,
                                    r.Style.Color));
                            }
                        }
                    }
                }
                else
                {
                    foreach (var word in Words)
                    {
                        foreach (var r in word.Runs)
                        {
                            if ((r.Style.FontStyle & FontStyle.Strikeout) == FontStyle.Strikeout)
                            {
                                lines.Add (new LineFColor (
                                    r.Left, r.Top + r.BaseLine / 3f * 2f, r.Left + r.Width, r.Style.Size * 0.1f * fixScale,
                                    r.Style.Color));
                            }
                        }
                    }
                }
            }

            internal void MakeUnderlines()
            {
                if (Renderer.everUnderlines)
                {
                    MakeEverUnderlines();
                    return;
                }

                List<List<Run>> runs = new List<List<Run>>();
                List<Run> currentRuns = null;

                foreach (var word in Words)
                {
                    foreach (var run in word.Runs)
                    {
                        if ((run.Style.FontStyle & FontStyle.Underline) == FontStyle.Underline)
                        {
                            if (currentRuns == null)
                            {
                                currentRuns = new List<Run>();
                                runs.Add (currentRuns);
                            }

                            currentRuns.Add (run);
                        }
                        else
                        {
                            currentRuns = null;
                        }
                    }
                }

                var unerlines = Renderer.underlines;
                var fixScale = Renderer.Scale / Renderer.FontScale;

                foreach (List<Run> cRuns in runs)
                {
                    OwnHashSet<StyleDescriptor> styles = new OwnHashSet<StyleDescriptor>();
                    float size = 0;
                    float underline = 0;
                    foreach (var r in cRuns)
                    {
                        if (!styles.Contains (r.Style))
                        {
                            styles.Add (r.Style);
                            size += r.Style.Size;
                            underline += r.Descent / 2;
                        }
                    }

                    size /= styles.Count;
                    underline /= styles.Count;

                    if (Renderer.RightToLeft)
                    {
                        foreach (var r in cRuns)
                        {
                            unerlines.Add (new LineFColor (
                                r.Left - r.Width, r.Top + r.BaseLine + underline, r.Left, size * 0.1f * fixScale,
                                r.Style.Color));
                        }
                    }
                    else
                    {
                        foreach (var r in cRuns)
                        {
                            unerlines.Add (new LineFColor (
                                r.Left, r.Top + r.BaseLine + underline, r.Left + r.Width, size * 0.1f * fixScale,
                                r.Style.Color));
                        }
                    }
                }
            }

            #endregion Internal Methods

            #region Private Methods

            private float CalcWidth()
            {
                float width = 0;
                foreach (var w in Words)
                {
                    foreach (var r in w.Runs)
                    {
                        width += r.Width;
                    }
                }

                var lastWord = Words.Count > 0 ? Words[Words.Count - 1] : null;
                if (lastWord is { Type: WordType.WhiteSpace })
                {
                    foreach (var r in lastWord.Runs)
                    {
                        width -= r.Width;
                    }
                }

                return width;
            }

            #endregion Private Methods
        }

        public class Paragraph
        {
            #region Private Fields

            #endregion Private Fields

            #region Public Properties

            public List<Line> Lines { get; }

            public HtmlTextRenderer Renderer { get; }

            #endregion Public Properties

            #region Public Constructors

            public Paragraph (HtmlTextRenderer renderer)
            {
                Lines = new List<Line>();
                this.Renderer = renderer;
            }

            #endregion Public Constructors

            #region Public Methods

            public override string ToString()
            {
                if (Lines.Count == 0)
                {
                    return "Lines[0]";
                }

                var sb = new StringBuilder();
                sb.AppendFormat ("Lines[{0}]", Lines.Count);
                sb.Append ("{");
                foreach (var line in Lines)
                {
                    sb.Append (line);
                    sb.Append (",");
                }

                sb.Append ("}");
                return sb.ToString();
            }

            #endregion Public Methods

            #region Internal Methods

            internal void AlignLines (bool forceJustify)
            {
                for (var i = 0; i < Lines.Count; i++)
                {
                    var align = Renderer.HorizontalAlign;
                    if (align == HorizontalAlign.Justify && i == Lines.Count - 1 && !forceJustify)
                    {
                        align = HorizontalAlign.Left;
                    }

                    Lines[i].AlignWords (align);
                }
            }

            #endregion Internal Methods
        }

        public abstract class Run
        {
            #region Protected Fields

            protected float baseLine;
            protected int charIndex;
            protected float descent;
            protected float height;
            protected float left;
            protected HtmlTextRenderer renderer;
            protected StyleDescriptor style;

            protected float top;

            //protected float FUnderline;
            //protected float FUnderlineSize;
            protected float width;
            protected Word word;

            #endregion Protected Fields

            #region Public Properties

            public float BaseLine
            {
                get => baseLine;
                set => baseLine = value;
            }

            public int CharIndex => charIndex;

            public float Descent
            {
                get => descent;
                set => descent = value;
            }

            public float Height
            {
                get => height;
                set => height = value;
            }

            public float Left
            {
                get => left;
                set => left = value;
            }

            public HtmlTextRenderer Renderer => renderer;

            public StyleDescriptor Style => style;

            public float Top
            {
                get => top;
                set => top = value;
            }

            //public float Underline
            //{
            //    get { return FUnderline; }
            //    set { FUnderline = value; }
            //}

            //public float UnderlineSize
            //{
            //    get { return FUnderlineSize; }
            //    set { FUnderlineSize = value; }
            //}

            public float Width
            {
                get => width;
                set => width = value;
            }

            public Word Word
            {
                get => word;
                set => word = value;
            }

            #endregion Public Properties

            #region Public Constructors

            public Run (HtmlTextRenderer renderer, Word word, StyleDescriptor style, float left, int charIndex)
            {
                this.renderer = renderer;
                this.word = word;
                this.style = style;
                this.left = left;
                this.charIndex = charIndex;
            }

            #endregion Public Constructors

            //public virtual void DrawBack(float top, float height)
            //{
            //    if (FStyle.BackgroundColor.A > 0)
            //    {
            //        using (Brush brush = GetBackgroundBrush())
            //            FRenderer.FGraphics.FillRectangle(brush, Left, top, Width, height);
            //    }
            //}

            #region Public Methods

            public abstract void Draw();

            public abstract Run Split (float availableWidth, out Run secondPart);

            #endregion Public Methods

            #region Protected Methods

            protected Brush GetBackgroundBrush()
            {
                return new SolidBrush (style.BackgroundColor);
            }

            #endregion Protected Methods

            //public virtual void Draw(bool drawContents)
            //{
            //    if ((FStyle.FontStyle & FontStyle.Underline) == FontStyle.Underline)
            //    {
            //        if (!FRenderer.FUnderLines)
            //        {
            //            float top = Top + FUnderline;
            //            using (Pen pen = new Pen(FStyle.Color, FUnderlineSize * 0.1f))
            //                if (FRenderer.FRightToLeft)
            //                    FRenderer.FGraphics.DrawLine(pen, Left - Width, top, Left, top);
            //                else
            //                    FRenderer.FGraphics.DrawLine(pen, Left, top, Left + Width, top);
            //        }
            //    }
            //    if ((FStyle.FontStyle & FontStyle.Strikeout) == FontStyle.Strikeout)
            //    {
            //        float top = Top + FBaseLine / 3 * 2;
            //        using (Pen pen = new Pen(FStyle.Color, FStyle.Size * 0.1f))
            //            if (FRenderer.FRightToLeft)
            //                FRenderer.FGraphics.DrawLine(pen, Left - Width, top, Left, top);
            //            else
            //                FRenderer.FGraphics.DrawLine(pen, Left, top, Left + Width, top);
            //    }
            //}
        }

        public class RunImage : Run
        {
            #region Private Fields

            private string src;

            #endregion Private Fields

            #region Public Properties

            public Image Image { get; }

            #endregion Public Properties

            #region Public Constructors

            public RunImage (HtmlTextRenderer renderer, Word word, string src, StyleDescriptor style, float left,
                int charIndex, float img_width, float img_height) : base (renderer, word, style, left, charIndex)
            {
                this.style = new StyleDescriptor (style);
                this.src = src;

                //disable for exports because img tag not support strikeouts and underlines
                this.style.FontStyle &= ~(FontStyle.Strikeout | FontStyle.Underline);
                Image = InlineImageCache.Load (Renderer.cache, src);
                Width = Image.Width * Renderer.Scale;
                Height = Image.Height * Renderer.Scale;
                if (img_height > 0)
                {
                    if (img_width > 0)
                    {
                        Width = img_width * Renderer.Scale;
                        Height = img_height * Renderer.Scale;
                    }
                    else
                    {
                        Width *= img_height / Image.Height;
                        Height = img_height * Renderer.Scale;
                    }
                }
                else if (img_width > 0)
                {
                    Width = img_width * Renderer.Scale;
                    Height *= img_width / Image.Width;
                }

                baseLine = Height;
                using (var ff = style.GetFont())
                {
                    var height = ff.GetHeight (Renderer.graphics.Graphics);
                    float lineSpace = ff.FontFamily.GetLineSpacing (style.FontStyle);
                    float descent = ff.FontFamily.GetCellDescent (style.FontStyle);
                    this.descent = height * descent / lineSpace;
                }
            }

            #endregion Public Constructors

            #region Public Methods

            public override void Draw()

                //public override void Draw(bool drawContents)
            {
                //if (drawContents)
                if (Image != null)
                {
                    if (renderer.RightToLeft)
                    {
                        renderer.graphics.DrawImage (Image, new RectangleF (Left - Width, Top, Width, Height));
                    }
                    else
                    {
                        renderer.graphics.DrawImage (Image, new RectangleF (Left, Top, Width, Height));
                    }
                }

                //base.Draw(drawContents);
            }

            public override Run Split (float availableWidth, out Run secondPart)
            {
                secondPart = this;
                return null;
            }

            #endregion Public Methods

            #region Internal Methods

            public Bitmap GetBitmap (out float width, out float height)
            {
                width = 1;
                height = 1;
                if (Image == null)
                {
                    return new Bitmap (1, 1);
                }

                width = Image.Width;
                height = Image.Height;
                float x = 0;
                float y = 0;

                var scaleX = width / Width;
                var scaleY = height / Height;

                if (left < renderer.displayRect.Left)
                {
                    x = -((renderer.displayRect.Left - left) * scaleX);
                    width += x;
                }

                if (top < renderer.displayRect.Top)
                {
                    y = -((renderer.displayRect.Top - top) * scaleY);
                    height += y;
                }

                if (left + this.width > renderer.displayRect.Right)
                {
                    width -= ((left + this.width - renderer.displayRect.Right) * scaleX);
                }

                if (top + this.height > renderer.displayRect.Bottom)
                {
                    height -= ((top + this.height - renderer.displayRect.Bottom) * scaleY);
                }

                if (width < 1)
                {
                    width = 1;
                }

                if (height < 1)
                {
                    height = 1;
                }

                var bmp = new Bitmap ((int)width, (int)height);
                using (var g = Graphics.FromImage (bmp))
                    g.DrawImage (Image, new PointF (x, y));
                width /= scaleX;
                height /= scaleY;
                return bmp;
            }

            #endregion Internal Methods

            //public override void ToHtml(FastString sb, bool download)
            //{
            //    if(download)
            //    {
            //        if(FImage!=null)
            //        {
            //            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            //            {
            //                try
            //                {
            //                    using (Bitmap bmp = new Bitmap(FImage.Width, FImage.Height))
            //                    {
            //                        using (Graphics g = Graphics.FromImage(bmp))
            //                        {
            //                            g.DrawImage(FImage, Point.Empty);
            //                        }
            //                        bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            //                    }
            //                    ms.Flush();
            //                    sb.Append("<img src=\"data:image/png;base64,").Append(Convert.ToBase64String(ms.ToArray()))
            //                        .Append("\" width=\"").Append(FWidth.ToString(CultureInfo)).Append("\" height=\"").Append(FHeight.ToString(CultureInfo)).Append("\"/>");
            //                }
            //                catch(Exception e)
            //                {
            //                }
            //            }
            //        }
            //    }else  if(!String.IsNullOrEmpty(FSrc))
            //    {
            //        if (FImage != null)
            //        {
            //            sb.Append("<img src=\"").Append(FSrc).Append("\" width=\"").Append(FWidth.ToString(CultureInfo)).Append("\" height=\"").Append(FHeight.ToString(CultureInfo)).Append("\"/>");
            //        }
            //        else
            //        {
            //            sb.Append("<img src=\"").Append(FSrc).Append("\"/>");
            //        }
            //    }

            //}
        }

        public class RunText : Run
        {
            #region Private Fields

            private List<CharWithIndex> chars;

            #endregion Private Fields

            #region Public Properties

            public string Text { get; }

            #endregion Public Properties

            #region Public Constructors

            public RunText (HtmlTextRenderer renderer, Word word, StyleDescriptor style, List<CharWithIndex> text,
                float left, int charIndex) : base (renderer, word, style, left, charIndex)
            {
                using (var ff = style.GetFont())
                {
                    chars = new List<CharWithIndex> (text);

                    this.Text = GetString (text);

                    if (ff.FontFamily.Name is "Wingdings" or "Webdings")
                    {
                        this.Text = WingdingsToUnicodeConverter.Convert (this.Text);
                    }

                    if (this.Text.Length > 0)
                    {
                        this.charIndex = text[0].Index;
                        if (word.Type == WordType.WhiteSpace)
                        {
                            //using (Font f = new Font("Consolas", 10))
                            width = CalcSpaceWidth (this.Text, ff);
                        }
                        else
                        {
                            width = Renderer.graphics.MeasureString (this.Text, ff, int.MaxValue, this.renderer.format)
                                .Width;
                        }
                    }

                    height = ff.GetHeight (Renderer.graphics.Graphics);
                    float lineSpace = ff.FontFamily.GetLineSpacing (style.FontStyle);
                    float ascent = ff.FontFamily.GetCellAscent (style.FontStyle);
                    baseLine = height * ascent / lineSpace;
                    descent = height - baseLine;
                    if (style.BaseLine == HtmlTextRenderer.BaseLine.Subscript)
                    {
                        descent += height * 0.45f;
                    }
                }
            }

            #endregion Public Constructors

            #region Public Methods

            public float CalcSpaceWidth (string text, Font ff)
            {
                return Renderer.graphics.MeasureString ("1" + text + "2", ff, int.MaxValue, renderer.format).Width
                       - Renderer.graphics.MeasureString ("12", ff, int.MaxValue, renderer.format).Width;
            }

            public override void Draw()

                //public override void Draw(bool drawContents)
            {
                using (var font = style.GetFont())
                using (var brush = GetBrush())
                {
                    //if (drawContents)
                    //{
                    //#if DEBUG
                    //                    SizeF size = FRenderer.FGraphics.MeasureString(FText, font, int.MaxValue, FRenderer.FFormat);
                    //                    if (FRenderer.RightToLeft)
                    //                        FRenderer.FGraphics.DrawRectangle(Pens.Red, Left - size.Width, Top, size.Width, size.Height);
                    //                    else
                    //                        FRenderer.FGraphics.DrawRectangle(Pens.Red, Left, Top, size.Width, size.Height);
                    //#endif
                    renderer.graphics.DrawString (Text, font, brush, Left, Top, renderer.format);
                }

                //}
                //base.Draw(drawContents);
            }

            public Brush GetBrush()
            {
                return new SolidBrush (Style.Color);
            }

            public override Run Split (float availableWidth, out Run secondPart)
            {
                var size = chars.Count;
                if (size == 0)
                {
                    secondPart = this;
                    return null;
                }

                var from = 0;
                var point = size / 2;
                var to = size;
                Run r = null;
                while (to - from > 1)
                {
                    var list = new List<CharWithIndex>();
                    for (var i = 0; i < point; i++)
                    {
                        list.Add (chars[i]);
                    }

                    r = new RunText (renderer, word, style, list, left, charIndex);
                    if (r.Width > availableWidth)
                    {
                        if (point == 1)
                        {
                            // Single char width is less than availableWidth
                            // Give up splitting
                            secondPart = null;
                            return this;
                        }

                        to = point;
                        point = (to + from) / 2;
                    }
                    else
                    {
                        from = point;
                        point = (to + from) / 2;
                    }
                }

                if (to < 2)
                {
                    secondPart = this;
                    return null;
                }
                else
                {
                    var list = new List<CharWithIndex>();
                    for (var i = point; i < size; i++)
                    {
                        list.Add (chars[i]);
                    }

                    secondPart = new RunText (renderer, word, style, list, left + r.Width, charIndex);
                    list.Clear();
                    for (var i = 0; i < point; i++)
                    {
                        list.Add (chars[i]);
                    }

                    r = new RunText (renderer, word, style, list, left, charIndex);
                    return r;
                }
            }

            #endregion Public Methods

            #region Private Methods

            private string GetString (List<CharWithIndex> str)
            {
                renderer.cacheString.Clear();
                foreach (var ch in str)
                {
                    renderer.cacheString.Append (ch.Char);
                }

                return renderer.cacheString.ToString();
            }

            #endregion Private Methods

            //public override void ToHtml(FastString sb, bool download)
            //{
            //    //if (FWord.Type == WordType.Tab)
            //    //    sb.Append("<span style=\"display:inline-block;min-width:").Append((FWidth * 0.99f).ToString(CultureInfo)).Append("px;\">");
            //    foreach(char ch in Text)
            //    {
            //        switch (ch)
            //        {
            //            case '"':
            //                sb.Append("&quot;");
            //                break;
            //            case '&':
            //                sb.Append("&amp;");
            //                break;
            //            case '<':
            //                sb.Append("&lt;");
            //                break;
            //            case '>':
            //                sb.Append("&gt;");
            //                break;
            //            case '\t':
            //                sb.Append("&Tab;");
            //                break;
            //            default:
            //                sb.Append(ch);
            //                break;
            //        }
            //    }
            //    //if (FWord.Type == WordType.Tab)
            //    //    sb.Append("</span>");
            //}
        }

        public class Word
        {
            #region Private Fields

            #endregion Private Fields

            #region Public Properties

            public float BaseLine { get; private set; }

            public float Descent { get; private set; }

            public float Height { get; private set; }

            public Line Line { get; set; }

            public HtmlTextRenderer Renderer { get; }

            public List<Run> Runs { get; }

            public WordType Type { get; set; }

            #endregion Public Properties

            #region Public Constructors

            public Word (HtmlTextRenderer renderer, Line line)
            {
                this.Renderer = renderer;
                Runs = new List<Run>();
                this.Line = line;
            }

            public Word (HtmlTextRenderer renderer, Line line, WordType type)
            {
                this.Renderer = renderer;
                Runs = new List<Run>();
                this.Line = line;
                this.Type = type;
            }

            #endregion Public Constructors

            #region Internal Methods

            internal void CalcMetrics()
            {
                BaseLine = 0;
                Descent = 0;
                foreach (var run in Runs)
                {
                    BaseLine = Math.Max (BaseLine, run.BaseLine);
                    Descent = Math.Max (Descent, run.Descent);
                }

                Height = BaseLine + Descent;
            }

            #endregion Internal Methods
        }

        #endregion Public Classes

        #region Internal Classes

        internal class SimpleFastReportHtmlElement
        {
            #region Public Fields

            public Dictionary<string, string> attributes;
            public bool isSelfClosed;
            public bool isEnd;
            public string name;

            #endregion Public Fields

            #region Private Fields

            private Dictionary<string, string> style;

            #endregion Private Fields

            #region Public Properties

            public bool IsSelfClosed
            {
                get
                {
                    switch (name)
                    {
                        case "img":
                        case "br":
                            return true;

                        default:
                            return isSelfClosed;
                    }
                }
                set => isSelfClosed = value;
            }

            /// <summary>
            /// Be care generates dictionary only one time
            /// </summary>
            public Dictionary<string, string> Style
            {
                get
                {
                    if (style == null && attributes != null && attributes.ContainsKey ("style"))
                    {
                        var styleString = attributes["style"];
                        style = new Dictionary<string, string> (StringComparer.OrdinalIgnoreCase);
                        foreach (var kv in styleString.Split (new char[] { ';' },
                                     StringSplitOptions.RemoveEmptyEntries))
                        {
                            string[] strs = kv.Split (':');
                            if (strs.Length == 2)
                            {
                                style[strs[0]] = strs[1];
                            }
                        }
                    }

                    return style;
                }
            }

            #endregion Public Properties

            #region Public Constructors

            public SimpleFastReportHtmlElement (string name)
            {
                this.name = name;
            }

            public SimpleFastReportHtmlElement (string name, Dictionary<string, string> attributes)
            {
                this.name = name;
                this.attributes = attributes;
            }

            public SimpleFastReportHtmlElement (string name, bool isEnd)
            {
                this.name = name;
                this.isEnd = isEnd;
            }

            public SimpleFastReportHtmlElement (string name, bool isBegin, Dictionary<string, string> attributes)
            {
                this.name = name;
                isEnd = isBegin;
                this.attributes = attributes;
            }

            public SimpleFastReportHtmlElement (string name, bool isBegin, bool isSelfClosed)
            {
                this.name = name;
                isEnd = isBegin;
                IsSelfClosed = isSelfClosed;
            }

            public SimpleFastReportHtmlElement (string name, bool isBegin, bool isSelfClosed,
                Dictionary<string, string> attributes)
            {
                this.name = name;
                isEnd = isBegin;
                IsSelfClosed = isSelfClosed;
                this.attributes = attributes;
            }

            #endregion Public Constructors

            #region Public Methods

            public override string ToString()
            {
                var sb = new StringBuilder();
                sb.Append ("<");
                if (isEnd)
                {
                    sb.Append ("/");
                }

                sb.Append (name);
                if (attributes != null)
                {
                    foreach (KeyValuePair<string, string> kv in attributes)
                    {
                        sb.Append (" ");
                        sb.Append (kv.Key);
                        sb.Append ("=\"");
                        sb.Append (kv.Value);
                        sb.Append ("\"");
                    }
                }

                if (IsSelfClosed)
                {
                    sb.Append ('/');
                }

                sb.Append (">");
                return sb.ToString();
            }

            #endregion Public Methods
        }

        internal class SimpleFastReportHtmlReader
        {
            #region Private Fields

            private CharWithIndex @char;
            private int position;
            private string substring;
            private string text;

            #endregion Private Fields

            #region Public Properties

            public CharWithIndex Character => @char;

            public SimpleFastReportHtmlElement Element { get; private set; }

            public bool IsEOF => position >= text.Length;

            public bool IsNotEOF => position < text.Length;

            public int LastPosition { get; private set; }

            public int Position
            {
                get => position;
                set => position = value;
            }

            #endregion Public Properties

            #region Public Constructors

            public SimpleFastReportHtmlReader (string text)
            {
                this.text = text;
            }

            #endregion Public Constructors

            #region Public Methods

            public static bool IsCanBeCharacterInTagName (char c)
            {
                if (c == ':')
                {
                    return true;
                }

                if (c is >= 'A' and <= 'Z')
                {
                    return true;
                }

                if (c == '_')
                {
                    return true;
                }

                if (c is >= 'a' and <= 'z')
                {
                    return true;
                }

                if (c == '-')
                {
                    return true; //
                }

                if (c == '.')
                {
                    return true; //
                }

                if (c is >= '0' and <= '9')
                {
                    return true; //
                }

                if (c == '\u00B7')
                {
                    return true; //
                }

                if (c is >= '\u00C0' and <= '\u00D6')
                {
                    return true;
                }

                if (c is >= '\u00D8' and <= '\u00F6')
                {
                    return true;
                }

                if (c is >= '\u00F8' and <= '\u02FF')
                {
                    return true;
                }

                if (c is >= '\u0300' and <= '\u036F')
                {
                    return true; //
                }

                if (c is >= '\u0370' and <= '\u037D')
                {
                    return true;
                }

                if (c is >= '\u037F' and <= '\u1FFF')
                {
                    return true;
                }

                if (c is >= '\u200C' and <= '\u200D')
                {
                    return true;
                }

                if (c is >= '\u203F' and <= '\u2040')
                {
                    return true; //
                }

                if (c is >= '\u2070' and <= '\u218F')
                {
                    return true;
                }

                if (c is >= '\u2C00' and <= '\u2FEF')
                {
                    return true;
                }

                if (c is >= '\u3001' and <= '\uD7FF')
                {
                    return true;
                }

                if (c is >= '\uF900' and <= '\uFDCF')
                {
                    return true;
                }

                if (c is >= '\uFDF0' and <= '\uFFFD')
                {
                    return true;
                }

                return false;
            }

            public static bool IsCanBeFirstCharacterInTagName (char c)
            {
                if (c == ':')
                {
                    return true;
                }

                if (c is >= 'A' and <= 'Z')
                {
                    return true;
                }

                if (c == '_')
                {
                    return true;
                }

                if (c is >= 'a' and <= 'z')
                {
                    return true;
                }

                if (c is >= '\u00C0' and <= '\u00D6')
                {
                    return true;
                }

                if (c is >= '\u00D8' and <= '\u00F6')
                {
                    return true;
                }

                if (c is >= '\u00F8' and <= '\u02FF')
                {
                    return true;
                }

                if (c is >= '\u0370' and <= '\u037D')
                {
                    return true;
                }

                if (c is >= '\u037F' and <= '\u1FFF')
                {
                    return true;
                }

                if (c is >= '\u200C' and <= '\u200D')
                {
                    return true;
                }

                if (c is >= '\u2070' and <= '\u218F')
                {
                    return true;
                }

                if (c is >= '\u2C00' and <= '\u2FEF')
                {
                    return true;
                }

                if (c is >= '\u3001' and <= '\uD7FF')
                {
                    return true;
                }

                if (c is >= '\uF900' and <= '\uFDCF')
                {
                    return true;
                }

                if (c is >= '\uFDF0' and <= '\uFFFD')
                {
                    return true;
                }

                return false;
            }

            /// <summary>
            /// Return true if read char
            /// </summary>
            /// <returns></returns>
            public bool Read()
            {
                LastPosition = position;
                switch ((@char = new CharWithIndex (text[position], position)).Char)
                {
                    case '&':
                        if (Converter.FromHtmlEntities (text, ref position, out substring))
                        {
                            @char.Char = substring[0];
                        }

                        position++;
                        return true;

                    case '<':
                        Element = GetElement (text, ref position);
                        position++;
                        if (Element != null)
                        {
                            switch (Element.name)
                            {
                                case "br":
                                    @char = new CharWithIndex ('\n', LastPosition);
                                    return true;

                                default:
                                    return false;
                            }
                        }

                        return true;
                }

                position++;
                return true;
            }

            #endregion Public Methods

            #region Private Methods

            private SimpleFastReportHtmlElement GetElement (string line, ref int index)
            {
                var to = line.Length - 1;
                var i = index + 1;
                var closed = false;
                if (i <= to)
                {
                    if (closed = line[i] == '/')
                    {
                        i++;
                    }
                }

                if (i <= to)
                {
                    if (!IsCanBeFirstCharacterInTagName (line[i]))
                    {
                        return null;
                    }
                }

                for (i++; i <= to && line[i] != ' ' && line[i] != '>' && line[i] != '/'; i++)
                {
                    if (!IsCanBeCharacterInTagName (line[i]))
                    {
                        return null;
                    }
                }

                if (i <= to)
                {
                    var tagName = line.Substring (index + (closed ? 2 : 1), i - index - (closed ? 2 : 1));
                    Dictionary<string, string> attrs = null;
                    if (!IsAvailableTagName (tagName))
                    {
                        return null;
                    }

                    if (line[i] == ' ')
                    {
                        //read attributes
                        for (; i <= to && line[i] != '>' && line[i] != '/'; i++)
                        {
                            for (; i <= to && line[i] == ' '; i++)
                            {
                                ;
                            }

                            if (line[i] == '>' || line[i] == '/')
                            {
                                i--;
                            }
                            else
                            {
                                if (!IsCanBeFirstCharacterInTagName (line[i]))
                                {
                                    return null;
                                }

                                var attrNameStartIndex = i;
                                for (i++; i <= to && line[i] != '='; i++)
                                {
                                    if (!IsCanBeFirstCharacterInTagName (line[i]))
                                    {
                                        return null;
                                    }
                                }

                                var attrNameEndIndex = i; //index of =
                                i++;
                                if (i <= to && line[i] == '"')
                                {
                                    //begin attr
                                    var attrValueStartIndex = i + 1;
                                    for (i++; i <= to && line[i] != '"'; i++)
                                    {
                                        switch (line[i])
                                        {
                                            case '<': return null;
                                            case '>': return null;
                                        }
                                    }

                                    if (i <= to)
                                    {
                                        var attrName = line.Substring (attrNameStartIndex,
                                            attrNameEndIndex - attrNameStartIndex);
                                        var attrValue = line.Substring (attrValueStartIndex,
                                            i - attrValueStartIndex);
                                        if (attrs == null)
                                        {
                                            attrs = new Dictionary<string, string> (StringComparer.OrdinalIgnoreCase);
                                        }

                                        attrs[attrName] = attrValue;
                                    }
                                }
                            }
                        }
                    }

                    if (i <= to)
                    {
                        if (line[i] == '>')
                        {
                            index = i;
                            return new SimpleFastReportHtmlElement (tagName, closed, false, attrs);
                        }

                        if (line[i] == '/' && i < to && line[i + 1] == '>')
                        {
                            index = i + 1;
                            return new SimpleFastReportHtmlElement (tagName, closed, true, attrs);
                        }
                    }
                }

                return null;
            }

            private bool IsAvailableTagName (string tagName)
            {
                switch (tagName)
                {
                    case "b":
                    case "br":
                    case "i":
                    case "u":
                    case "sub":
                    case "sup":
                    case "img":
                    //case "font":
                    case "strike":
                    case "span":
                        return true;
                }

                return false;
            }

            #endregion Private Methods
        }

        /// <summary>
        /// Represents a style used in HtmlTags mode. Color does not affect the equals function.
        /// </summary>
        public class StyleDescriptor
        {
            #region Private Fields

            private static readonly Color DefaultColor = Color.Transparent;
            private Color backgroundColor;
            private Color color;

            #endregion Private Fields

            #region Public Properties

            public Color BackgroundColor
            {
                get => backgroundColor;
                set => backgroundColor = value;
            }

            public BaseLine BaseLine { get; set; }

            public Color Color
            {
                get => color;
                set => color = value;
            }

            public FontFamily Font { get; set; }

            public FontStyle FontStyle { get; set; }

            public float Size { get; set; }

            #endregion Public Properties

            #region Public Constructors

            public StyleDescriptor (FontStyle fontStyle, Color color, BaseLine baseLine, FontFamily font, float size)
            {
                this.FontStyle = fontStyle;
                this.color = color;
                this.BaseLine = baseLine;
                this.Font = font;
                this.Size = size;
                backgroundColor = DefaultColor;
            }

            public StyleDescriptor (StyleDescriptor styleDescriptor)
            {
                FontStyle = styleDescriptor.FontStyle;
                color = styleDescriptor.color;
                BaseLine = styleDescriptor.BaseLine;
                Font = styleDescriptor.Font;
                Size = styleDescriptor.Size;
                backgroundColor = styleDescriptor.backgroundColor;
            }

            #endregion Public Constructors

            #region Public Methods

            public override bool Equals (object obj)
            {
                var descriptor = obj as StyleDescriptor;
                return descriptor != null &&
                       BaseLine == descriptor.BaseLine &&
                       Font == descriptor.Font &&
                       FontStyle == descriptor.FontStyle &&
                       Size == descriptor.Size;
            }

            /// <summary>
            /// returns true if objects realy equals
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public bool FullEquals (StyleDescriptor obj)
            {
                return obj != null && GetHashCode() == obj.GetHashCode() &&
                       Equals (obj) &&
                       color.Equals (obj.color) &&
                       backgroundColor.Equals (obj.backgroundColor);
            }

            public Font GetFont()
            {
                var fontSize = Size;
                if (BaseLine != BaseLine.Normal)
                {
                    fontSize *= 0.6f;
                }

                var fontStyle = FontStyle;

                fontStyle = fontStyle & ~FontStyle.Underline & ~FontStyle.Strikeout;
                return new Font (Font, fontSize, fontStyle);
            }

            public override int GetHashCode()
            {
                var hashCode = -1631016721;
                unchecked
                {
                    hashCode = hashCode * -1521134295 + BaseLine.GetHashCode();
                    hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode (Font.Name);
                    hashCode = hashCode * -1521134295 + FontStyle.GetHashCode();
                    hashCode = hashCode * -1521134295 + Size.GetHashCode();
                }

                return hashCode;
            }

            public void ToHtml (FastString sb, bool close)
            {
                var fontsize = Size / DrawUtils.ScreenDpiFX;
                if (close)
                {
                    sb.Append ("</span>");

                    if ((FontStyle & FontStyle.Strikeout) == FontStyle.Strikeout)
                    {
                        sb.Append ("</strike>");
                    }

                    if ((FontStyle & FontStyle.Underline) == FontStyle.Underline)
                    {
                        sb.Append ("</u>");
                    }

                    if ((FontStyle & FontStyle.Italic) == FontStyle.Italic)
                    {
                        sb.Append ("</i>");
                    }

                    if ((FontStyle & FontStyle.Bold) == FontStyle.Bold)
                    {
                        sb.Append ("</b>");
                    }

                    switch (BaseLine)
                    {
                        case BaseLine.Subscript:
                            sb.Append ("</sub>");
                            break;
                        case BaseLine.Superscript:
                            sb.Append ("</sup>");
                            break;
                    }
                }
                else
                {
                    switch (BaseLine)
                    {
                        case BaseLine.Subscript:
                            sb.Append ("<sub>");
                            break;
                        case BaseLine.Superscript:
                            sb.Append ("<sup>");
                            break;
                    }

                    if ((FontStyle & FontStyle.Bold) == FontStyle.Bold)
                    {
                        sb.Append ("<b>");
                    }

                    if ((FontStyle & FontStyle.Italic) == FontStyle.Italic)
                    {
                        sb.Append ("<i>");
                    }

                    if ((FontStyle & FontStyle.Underline) == FontStyle.Underline)
                    {
                        sb.Append ("<u>");
                    }

                    if ((FontStyle & FontStyle.Strikeout) == FontStyle.Strikeout)
                    {
                        sb.Append ("<strike>");
                    }

                    sb.Append ("<span style=\"");
                    if (backgroundColor.A > 0)
                    {
                        sb.Append (string.Format (CultureInfo, "background-color:rgba({0},{1},{2},{3});",
                            backgroundColor.R, backgroundColor.G, backgroundColor.B,
                            ((float)backgroundColor.A) / 255f));
                    }

                    if (color.A > 0)
                    {
                        sb.Append (string.Format (CultureInfo, "color:rgba({0},{1},{2},{3});", color.R, color.G,
                            color.B, ((float)color.A) / 255f));
                    }

                    if (Font != null)
                    {
                        sb.Append ("font-family:");
                        sb.Append (Font.Name);
                        sb.Append (";");
                    }

                    if (fontsize > 0)
                    {
                        sb.Append ("font-size:");
                        sb.Append (fontsize.ToString (CultureInfo));
                        sb.Append ("pt;");
                    }

                    sb.Append ("\">");
                }
            }

            #endregion Public Methods
        }

        private class OwnHashSet<T>
        {
#if DOTNET_4
            private HashSet<T> internalHashSet;
            public int Count { get { return internalHashSet.Count; } }
#else
            private Dictionary<T, object> internalDictionary;
            private object FHashSetObject;
            public int Count => internalDictionary.Count;
#endif

            public OwnHashSet()
            {
#if DOTNET_4
                internalHashSet = new HashSet<T>();
#else
                internalDictionary = new Dictionary<T, object>();
                FHashSetObject = new object();
#endif
            }

            public void Clear()
            {
#if DOTNET_4
                internalHashSet.Clear();
#else
                internalDictionary.Clear();
#endif
            }

            public bool Contains (T value)
            {
#if DOTNET_4
                return internalHashSet.Contains(value);
#else
                return internalDictionary.ContainsKey (value);
#endif
            }

            public void Add (T value)
            {
#if DOTNET_4
                internalHashSet.Add(value);
#else
                internalDictionary.Add (value, FHashSetObject);
#endif
            }
        }

        #endregion Internal Classes

        #region IDisposable Support

        private bool disposedValue;

        protected virtual void Dispose (bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    format.Dispose();
                    format = null;
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose (true);
        }

        #endregion IDisposable Support
    }

    /// <summary>
    /// Class that converts strings with Wingdings characters to Unicode strings.
    /// </summary>
    public static class WingdingsToUnicodeConverter
    {
        /// <summary>
        /// Converts string with Wingdings characters to its Unicode analog.
        /// </summary>
        /// <param name="str">The string that should be converted.</param>
        /// <returns></returns>
        public static string Convert (string str)
        {
            var chars = str.ToCharArray();
            for (var i = 0; i < chars.Length; i++)
            {
                if (chars[i] >= 0x20 && chars[i] <= 0xFF)
                {
                    chars[i] = (char)(0xF000 + chars[i]);
                }
            }

            return new string (chars);
        }
    }
}
