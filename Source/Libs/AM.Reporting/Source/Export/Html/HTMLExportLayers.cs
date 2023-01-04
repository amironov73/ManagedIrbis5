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
using System.Drawing;
using System.IO;

using AM.Reporting.Table;
using AM.Reporting.Utils;

using System.Windows.Forms;

using AM.Reporting.Export;

#endregion

#nullable enable

namespace AM.Reporting.Export.Html
{
    public partial class HTMLExport : ExportBase
    {
        private bool doPageBreak;

        private string GetStyle()
        {
            return "position:absolute;";
        }

        private string GetStyle (Font Font, Color TextColor, Color FillColor,
            bool RTL, HorizontalAlign HAlign, Border Border, bool WordWrap, float LineHeight, float Width, float Height,
            bool Clip)
        {
            var style = new FastString (256);

            if (Font != null)
            {
                if (Zoom != 1)
                {
                    using (var newFont = new Font (Font.FontFamily, Font.Size * Zoom, Font.Style, Font.Unit,
                               Font.GdiCharSet, Font.GdiVerticalFont))
                        HTMLFontStyle (style, newFont, LineHeight);
                }
                else
                {
                    HTMLFontStyle (style, Font, LineHeight);
                }
            }

            style.Append ("text-align:");
            if (HAlign == HorizontalAlign.Left)
            {
                style.Append (RTL ? "right" : "left");
            }
            else if (HAlign == HorizontalAlign.Right)
            {
                style.Append (RTL ? "left" : "right");
            }
            else if (HAlign == HorizontalAlign.Center)
            {
                style.Append ("center");
            }
            else
            {
                style.Append ("justify");
            }

            style.Append (";");

            if (WordWrap)
            {
                style.Append ("word-wrap:break-word;");
            }

            if (Clip)
            {
                style.Append ("overflow:hidden;");
            }

            style.Append ("position:absolute;color:").Append (ExportUtils.HTMLColor (TextColor))
                .Append (";background-color:")
                .Append (FillColor.A == 0 ? "transparent" : ExportUtils.HTMLColor (FillColor)).Append (";")
                .Append (RTL ? "direction:rtl;" : string.Empty);

            var newBorder = Border;
            HTMLBorder (style, newBorder);
            style.Append ("width:").Append (Px (Math.Abs (Width) * Zoom)).Append ("height:")
                .Append (Px (Math.Abs (Height) * Zoom));
            return style.ToString();
        }

        private int UpdateCSSTable (ReportComponentBase obj)
        {
            string style;
            if (obj is TextObject textObj)
            {
                style = GetStyle (textObj.Font, textObj.TextColor, textObj.FillColor,
                    textObj.RightToLeft, textObj.HorizontalAlign, textObj.Border, textObj.WordWrap, textObj.LineHeight,
                    textObj.Width, textObj.Height, textObj.Clip);
            }
            else if (obj is HtmlObject htmlObj)
            {
                style = GetStyle (DrawUtils.DefaultTextObjectFont, Color.Black, htmlObj.FillColor,
                    false, HorizontalAlign.Left, htmlObj.Border, true, 0, htmlObj.Width, htmlObj.Height, false);
            }
            else
            {
                style = GetStyle (null, Color.White, obj.FillColor, false, HorizontalAlign.Center, obj.Border, false, 0,
                    obj.Width, obj.Height, false);
            }

            return UpdateCSSTable (style);
        }

        private int UpdateCSSTable (string style)
        {
            var i = cssStyles.IndexOf (style);
            if (i == -1)
            {
                i = cssStyles.Count;
                cssStyles.Add (style);
            }

            return i;
        }

        private void ExportPageStylesLayers (FastString styles, int PageNumber)
        {
            PrintPageStyle (styles);
            if (prevStyleListIndex < cssStyles.Count)
            {
                styles.AppendLine (HTMLGetStylesHeader());
                for (var i = prevStyleListIndex; i < cssStyles.Count; i++)
                {
                    styles.Append (HTMLGetStyleHeader (i, PageNumber)).Append (cssStyles[i]).AppendLine ("}");
                }

                styles.AppendLine (HTMLGetStylesFooter());
            }
        }

        private string GetStyleTag (int index)
        {
            return string.Format ("class=\"{0}s{1}\"",
                    StylePrefix,
                    index.ToString()
                );
        }

        private void Layer (FastString Page, ReportComponentBase obj,
            float Left, float Top, float Width, float Height, FastString Text, string style, FastString addstyletag)
        {
            if (Page != null && obj != null)
            {
                string onclick = null;

                if (!string.IsNullOrEmpty (ReportID))
                {
                    if (!string.IsNullOrEmpty (obj.ClickEvent) || obj.HasClickListeners())
                    {
                        onclick = "click";
                    }

                    if (obj is CheckBoxObject { Editable: true })
                    {
                        onclick = "checkbox_click";
                    }

                    if (obj is TextObject { Editable: true })
                    {
                        onclick = "text_edit";
                    }
                }

                // we need to adjust left, top, width and height values because borders take up space in html elements
                HTMLBorderWidthValues (obj, out var borderLeft, out var borderTop, out var borderRight, out var borderBottom);

                var href = GetHref (obj);

                if (!string.IsNullOrEmpty (href))
                {
                    Page.Append (href);
                }

                Page.Append ("<div ").Append (style).Append (" style=\"")
                    .Append (onclick != null || !string.IsNullOrEmpty (href) ? "cursor:pointer;" : "").Append ("left:")
                    .Append (Px ((leftMargin + Left) * Zoom - borderLeft / 2f)).Append ("top:")
                    .Append (Px ((topMargin + Top) * Zoom - borderTop / 2f)).Append ("width:")
                    .Append (Px (Width * Zoom - borderRight / 2f - borderLeft / 2f)).Append ("height:")
                    .Append (Px (Height * Zoom - borderBottom / 2f - borderTop / 2f));

                if (addstyletag != null)
                {
                    Page.Append (addstyletag);
                }

                Page.Append ("\"");

                if (onclick != null)
                {
                    var eventParam = string.Format ("{0},{1},{2},{3}",
                        obj.Name,
                        CurPage,
                        obj.AbsLeft.ToString ("#0"),
                        obj.AbsTop.ToString ("#0"));

                    Page.Append (" onclick=\"")
                        .AppendFormat (OnClickTemplate, ReportID, onclick, eventParam)
                        .Append ("\"");
                }

                Page.Append (">");
                if (Text == null)
                {
                    Page.Append (NBSP);
                }
                else
                {
                    Page.Append (Text);
                }

                Page.AppendLine ("</div>");
                if (!string.IsNullOrEmpty (href))
                {
                    Page.Append ("</a>");
                }
            }
        }

        private string EncodeURL (string value)
        {
#if CROSSPLATFORM || COREWIN
            return System.Net.WebUtility.UrlEncode(value);
#else
            return ExportUtils.HtmlURL (value);
#endif
        }

        private string GetHref (ReportComponentBase obj)
        {
            var href = string.Empty;

            href = GetHrefAdvMatrixButton (obj, href);

            if (!string.IsNullOrEmpty (obj.Hyperlink.Value))
            {
                var hrefStyle = string.Empty;

                if (obj is TextObject textObject)
                {
                    hrefStyle = string.Format ("style=\"color:{0}{1}\"",
                            ExportUtils.HTMLColor (textObject.TextColor),
                            !textObject.Font.Underline ? ";text-decoration:none" : string.Empty
                        );
                }

                var url = EncodeURL (obj.Hyperlink.Value);
                if (obj.Hyperlink.Kind == HyperlinkKind.URL)
                {
                    href = string.Format (
                        "<a {0} href=\"{1}\"" + (obj.Hyperlink.OpenLinkInNewTab ? "target=\"_blank\"" : "") + ">",
                        hrefStyle, obj.Hyperlink.Value);
                }
                else if (obj.Hyperlink.Kind == HyperlinkKind.DetailReport)
                {
                    url = string.Format ("{0},{1},{2}",
                        EncodeURL (obj.Name), // object name for security reasons
                        EncodeURL (obj.Hyperlink.ReportParameter),
                        EncodeURL (obj.Hyperlink.Value));
                    var onClick = string.Format (OnClickTemplate, ReportID, "detailed_report", url);
                    href = string.Format ("<a {0} href=\"#\" onclick=\"{1}\">", hrefStyle, onClick);
                }
                else if (obj.Hyperlink.Kind == HyperlinkKind.DetailPage)
                {
                    url = string.Format ("{0},{1},{2}",
                        EncodeURL (obj.Name),
                        EncodeURL (obj.Hyperlink.ReportParameter),
                        EncodeURL (obj.Hyperlink.Value));
                    var onClick = string.Format (OnClickTemplate, ReportID, "detailed_page", url);
                    href = string.Format ("<a {0} href=\"#\" onclick=\"{1}\">", hrefStyle, onClick);
                }
                else if (SinglePage)
                {
                    if (obj.Hyperlink.Kind == HyperlinkKind.Bookmark)
                    {
                        href = string.Format ("<a {0} href=\"#{1}\">", hrefStyle, url);
                    }
                    else if (obj.Hyperlink.Kind == HyperlinkKind.PageNumber)
                    {
                        href = string.Format ("<a {0} href=\"#PageN{1}\">", hrefStyle, url);
                    }
                }
                else
                {
                    var onClick = string.Empty;
                    if (obj.Hyperlink.Kind == HyperlinkKind.Bookmark)
                    {
                        onClick = string.Format (OnClickTemplate, ReportID, "bookmark", url);
                    }
                    else if (obj.Hyperlink.Kind == HyperlinkKind.PageNumber)
                    {
                        onClick = string.Format (OnClickTemplate, ReportID, "goto", url);
                    }

                    if (onClick != string.Empty)
                    {
                        href = string.Format ("<a {0} href=\"#\" onclick=\"{1}\">", hrefStyle, onClick);
                    }
                }
            }

            return href;
        }

        private FastString GetSpanText (TextObjectBase obj, FastString text,
            float top, float width,
            float ParagraphOffset)
        {
            var style = new FastString();
            style.Append ("display:block;border:0;width:").Append (Px (width * Zoom));
            if (ParagraphOffset != 0)
            {
                style.Append ("text-indent:").Append (Px (ParagraphOffset * Zoom));
            }

            if (obj.Padding.Left != 0)
            {
                style.Append ("padding-left:").Append (Px ((obj.Padding.Left) * Zoom));
            }

            if (obj.Padding.Right != 0)
            {
                style.Append ("padding-right:").Append (Px (obj.Padding.Right * Zoom));
            }

            if (top != 0)
            {
                style.Append ("margin-top:").Append (Px (top * Zoom));
            }

            // we need to apply border width in order to position our div perfectly
            if (HTMLBorderWidthValues (obj, out var borderLeft, out var borderTop, out var borderRight, out var borderBottom))
            {
                style.Append ("position:absolute;")
                    .Append ("left:").Append (Px (-1 * borderLeft / 2f))
                    .Append ("top:").Append (Px (-1 * borderTop / 2f));
            }

            var result = new FastString (128);
            result.Append ("<div ").Append (GetStyleTag (UpdateCSSTable (style.ToString()))).Append (">").Append (text)
                .Append ("</div>");

            return result;
        }

        private void LayerText (FastString Page, TextObject obj)
        {
            float top = 0;

            if (obj.Font.FontFamily.Name is "Wingdings" or "Webdings")
            {
                obj.Text = WingdingsToUnicodeConverter.Convert (obj.Text);
            }

            switch (obj.TextRenderType)
            {
                case TextRenderType.HtmlParagraph:

                    using (var htmlTextRenderer = obj.GetHtmlTextRenderer (Zoom, Zoom))
                    {
                        if (obj.VerticalAlign == VerticalAlign.Center)
                        {
                            top = (obj.Height - htmlTextRenderer.CalcHeight()) / 2;
                        }
                        else if (obj.VerticalAlign == VerticalAlign.Bottom)
                        {
                            top = obj.Height - htmlTextRenderer.CalcHeight();
                        }

                        var sb = GetHtmlParagraph (htmlTextRenderer);

                        LayerBack (Page, obj,
                            GetSpanText (obj, sb,
                                top + obj.Padding.Top,
                                obj.Width - obj.Padding.Horizontal,
                                obj.ParagraphOffset));
                    }

                    break;
                default:
                    if (obj.VerticalAlign != VerticalAlign.Top)
                    {
                        var g = htmlMeasureGraphics;
                        using (var f = new Font (obj.Font.FontFamily, obj.Font.Size * DrawUtils.ScreenDpiFX,
                                   obj.Font.Style))
                        {
                            var textRect = new RectangleF (obj.AbsLeft + obj.Padding.Left,
                                obj.AbsTop + obj.Padding.Top,
                                obj.Width - obj.Padding.Left - obj.Padding.Right,
                                obj.Height - obj.Padding.Top - obj.Padding.Bottom);
                            var format = obj.GetStringFormat (Report.GraphicCache, 0);
                            Brush textBrush = Report.GraphicCache.GetBrush (obj.TextColor);
                            var renderer = new AdvancedTextRenderer (obj.Text, g, f, textBrush, null,
                                textRect, format, obj.HorizontalAlign, obj.VerticalAlign, obj.LineHeight, obj.Angle,
                                obj.FontWidthRatio,
                                obj.ForceJustify, obj.Wysiwyg, obj.HasHtmlTags, false, Zoom, Zoom,
                                obj.InlineImageCache);
                            if (renderer.Paragraphs.Count > 0)
                            {
                                if (renderer.Paragraphs[0].Lines.Count > 0)
                                {
                                    var height = renderer.Paragraphs[0].Lines[0].CalcHeight();
                                    if (height > obj.Height)
                                    {
                                        top = -(height - obj.Height) / 2;
                                    }
                                    else
                                    {
                                        top = renderer.Paragraphs[0].Lines[0].Top - obj.AbsTop;
                                        height = renderer.CalcHeight();

                                        if (obj.VerticalAlign == VerticalAlign.Center)
                                        {
                                            top = (obj.Height - height - obj.Padding.Bottom + obj.Padding.Top) / 2;

                                            if (top < 0)
                                            {
                                                if (obj.Height > height)
                                                {
                                                    top = (obj.Height - height - obj.Padding.Bottom + obj.Padding.Top) /
                                                          2;
                                                }
                                                else
                                                {
                                                    top = (height - obj.Height - obj.Padding.Bottom + obj.Padding.Top) /
                                                          2;
                                                }
                                            }
                                        }
                                        else if (obj.VerticalAlign == VerticalAlign.Bottom)
                                        {
                                            // (float)(Math.Round(obj.Font.Size * 96 / 72) / 2
                                            // necessary to compensate for paragraph offset error in GetSpanText method below
                                            top = obj.Height - height - obj.Padding.Bottom -
                                                  (float)(Math.Round (obj.Font.Size * 96 / 72) / 2);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    LayerBack (Page, obj,
                        GetSpanText (obj,
                            ExportUtils.HtmlString (obj.Text, obj.TextRenderType,
                                Px (Math.Round (obj.Font.Size * 96 / 72))),
                            top,
                            obj.Width - obj.Padding.Horizontal,
                            obj.ParagraphOffset));
                    break;
            }
        }

        private FastString GetHtmlParagraph (HtmlTextRenderer renderer)
        {
            var sb = new FastString();

            foreach (var paragraph in renderer.Paragraphs)
            {
                foreach (var line in paragraph.Lines)
                {
                    if (sb == null)
                    {
                        sb = new FastString();
                    }

                    sb.Append ("<span style=\"");
                    sb.Append ("display:block;");
                    if (line.Top + line.Height > renderer.DisplayRect.Bottom)
                    {
                        sb.Append ("height:")
                            .Append (Math.Max (renderer.DisplayRect.Bottom - line.Top, 0)
                                .ToString (HtmlTextRenderer.CultureInfo)).Append ("px;");
                    }
                    else
                    {
                        //sb.Append("height:").Append(line.Height.ToString(HtmlTextRenderer.CultureInfo)).Append("px;");
                        if (line.LineSpacing > 0)
                        {
                            sb.Append ("margin-bottom:").Append (line.LineSpacing.ToString (HtmlTextRenderer.CultureInfo))
                                .Append ("px;");
                        }
                    }

                    sb.Append ("overflow:hidden;");
                    sb.Append ("line-height:").Append (line.Height.ToString (HtmlTextRenderer.CultureInfo)).Append ("px;");
                    if (line.HorizontalAlign == HorizontalAlign.Justify)
                    {
                        sb.Append ("text-align-last:justify;");
                    }
                    else
                    {
                        sb.Append ("white-space:pre;");
                    }

                    sb.Append ("\">");
                    HtmlTextRenderer.StyleDescriptor styleDesc = null;
                    float prevWidth = 0;
                    foreach (var word in line.Words)
                    {
                        foreach (var run in word.Runs)
                        {
                            if (!run.Style.FullEquals (styleDesc))
                            {
                                if (styleDesc != null)
                                {
                                    styleDesc.ToHtml (sb, true);
                                }

                                styleDesc = run.Style;
                                styleDesc.ToHtml (sb, false);
                            }

                            if (run is HtmlTextRenderer.RunText runText)
                            {
                                foreach (var ch in runText.Text)
                                {
                                    switch (ch)
                                    {
                                        case '"':
                                            sb.Append ("&quot;");
                                            break;
                                        case '&':
                                            sb.Append ("&amp;");
                                            break;
                                        case '<':
                                            sb.Append ("&lt;");
                                            break;
                                        case '>':
                                            sb.Append ("&gt;");
                                            break;
                                        case '\t':
                                            if (word.Type == HtmlTextRenderer.WordType.Tab)
                                            {
                                                if (Layers)
                                                {
                                                    sb.Append (
                                                        $"<span style=\"tab-size: {Math.Round (prevWidth + run.Width)}px;\">&Tab;</span>");
                                                }
                                                else
                                                {
                                                    sb.Append (
                                                        $"<span style=\"tab-size: {Math.Round (run.Left + run.Width)}px;\">&Tab;</span>");
                                                }
                                            }
                                            else
                                            {
                                                sb.Append ("&Tab;");
                                            }

                                            break;
                                        default:
                                            sb.Append (ch);
                                            break;
                                    }
                                }
                            }
                            else if (run is HtmlTextRenderer.RunImage runImage)
                            {
                                using (var ms = new MemoryStream())
                                {
                                    try
                                    {
                                        float w, h;
                                        using (var bmp = runImage.GetBitmap (out w, out h))
                                        {
                                            bmp.Save (ms, System.Drawing.Imaging.ImageFormat.Png);
                                        }

                                        ms.Flush();
                                        sb.Append ("<img src=\"data:image/png;base64,")
                                            .Append (Convert.ToBase64String (ms.ToArray()))
                                            .Append ("\" width=\"").Append (w.ToString (HtmlTextRenderer.CultureInfo))
                                            .Append ("\" height=\"").Append (h.ToString (HtmlTextRenderer.CultureInfo))
                                            .Append ("\"/>");
                                    }
                                    catch (Exception /*e*/)
                                    {
                                    }
                                }
                            }

                            prevWidth += run.Width;

                            //run.ToHtml(sb, true);
                        }
                    }

                    if (styleDesc != null)
                    {
                        styleDesc.ToHtml (sb, true);
                    }
                    else
                    {
                        sb.Append ("<br/>");
                    }

                    sb.Append ("</span>");
                }
            }

            return sb;
        }

        private void LayerHtml (FastString Page, HtmlObject obj)
        {
            LayerBack (Page, obj,
                GetSpanText (obj, new FastString (obj.Text),
                    obj.Padding.Top,
                    obj.Width - obj.Padding.Horizontal,
                    0));
        }

        private string GetLayerPicture (ReportComponentBase obj, out float Width, out float Height)
        {
            var result = string.Empty;
            Width = 0;
            Height = 0;

            if (obj != null)
            {
                if (Pictures)
                {
                    var PictureStream = new MemoryStream();
                    var FPictureFormat = System.Drawing.Imaging.ImageFormat.Bmp;
                    if (ImageFormat == ImageFormat.Png)
                    {
                        FPictureFormat = System.Drawing.Imaging.ImageFormat.Png;
                    }
                    else if (ImageFormat == ImageFormat.Jpeg)
                    {
                        FPictureFormat = System.Drawing.Imaging.ImageFormat.Jpeg;
                    }
                    else if (ImageFormat == ImageFormat.Gif)
                    {
                        FPictureFormat = System.Drawing.Imaging.ImageFormat.Gif;
                    }

                    Width = obj.Width == 0 ? obj.Border.LeftLine.Width : obj.Width;
                    Height = obj.Height == 0 ? obj.Border.TopLine.Width : obj.Height;

                    if (Math.Abs (Width) * Zoom < 1 && Zoom > 0)
                    {
                        Width = 1 / Zoom;
                    }

                    if (Math.Abs (Height) * Zoom < 1 && Zoom > 0)
                    {
                        Height = 1 / Zoom;
                    }

                    var zoom = HighQualitySVG ? 3 : 1;

                    using (System.Drawing.Image image =
                           new Bitmap (
                                   (int)(Math.Abs (Math.Round (Width * Zoom * zoom))),
                                   (int)(Math.Abs (Math.Round (Height * Zoom * zoom)))
                               ))
                    {
                        using (var g = Graphics.FromImage (image))
                        {
#if MSCHART
                            if (obj is TextObjectBase || obj is AM.Reporting.MSChart.MSChartObject)
                                g.Clear(GetClearColor(obj));
#else
                            if (obj is TextObjectBase)
                            {
                                g.Clear (GetClearColor (obj));
                            }
#endif

                            var Left = Width > 0 ? obj.AbsLeft : obj.AbsLeft + Width;
                            var Top = Height > 0 ? obj.AbsTop : obj.AbsTop + Height;

                            float dx = 0;
                            float dy = 0;
                            g.TranslateTransform ((-Left - dx) * Zoom * zoom, (-Top - dy) * Zoom * zoom);

                            var oldLines = obj.Border.Lines;
                            obj.Border.Lines = BorderLines.None;
                            obj.Draw (new FRPaintEventArgs (g, Zoom * zoom, Zoom * zoom, Report.GraphicCache));
                            obj.Border.Lines = oldLines;
                        }

                        using (var b = new Bitmap (
                                       (int)(Math.Abs (Math.Round (Width * Zoom))),
                                       (int)(Math.Abs (Math.Round (Height * Zoom)))
                                   ))
                        {
                            using (var gr = Graphics.FromImage (b))
                            {
                                gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                                gr.DrawImage (image, 0, 0, (int)Math.Abs (Width) * Zoom, (int)Math.Abs (Height) * Zoom);
                            }

                            if (FPictureFormat == System.Drawing.Imaging.ImageFormat.Jpeg)
                            {
                                ExportUtils.SaveJpeg (b, PictureStream, 95);
                            }
                            else
                            {
                                b.Save (PictureStream, FPictureFormat);
                            }
                        }
                    }

                    PictureStream.Position = 0;

                    var hash = string.Empty;
                    if (obj is PictureObject pictureObject)
                    {
                        var pic = pictureObject;
                        if (pic.Image != null)
                        {
#if MONO
                            using (MemoryStream picStr = new MemoryStream())
                            {

                                ImageHelper.Save(pic.Image, picStr);
                                using(StreamWriter picWriter = new StreamWriter(picStr))
                                {
                                    picWriter.Write(pic.Width);
                                    picWriter.Write(pic.Height);
                                    picWriter.Write(pic.Angle);
                                    picWriter.Write(pic.Transparency);
                                    picWriter.Write(pic.TransparentColor.ToArgb());
                                    picWriter.Write(pic.CanShrink);
                                    picWriter.Write(pic.CanGrow);
                                    hash = Crypter.ComputeHash(picStr);
                                }
                            }
#else

                            hash = Crypter.ComputeHash (PictureStream);
                            PictureStream.Position = 0;
#endif
                        }
                    }
                    else
                    {
                        hash = Crypter.ComputeHash (PictureStream);
                    }

                    result = HTMLGetImage (0, 0, 0, hash, true, null, PictureStream, false);
                }
            }

            return result;
        }

        // Method to get background color of parent to fix an issue with blurred rotated text. g.Clear(Color.Transparent) - reason.
        private Color GetClearColor (ReportComponentBase obj)
        {
            var color = Color.Transparent;
            var tempObj = obj;

            if (obj is { Parent: BandBase, Band.Fill.IsTransparent: true })
            {
                color = Color.White;
            }
            else if (obj.Parent is BandBase)
            {
                color = obj.Band.FillColor;
            }
            else
            {
                var i = 0;
                while (tempObj is { Fill.IsTransparent: true, Parent: not BandBase } && i < 10)
                {
                    i++;
                    tempObj = tempObj.Parent as ReportComponentBase;

                    if (tempObj is { Fill.IsTransparent: false })
                    {
                        color = tempObj.FillColor;
                        break;
                    }

                    if (tempObj?.Parent is BandBase)
                    {
                        color = Color.White;
                    }
                }
            }

            return color;
        }

        private void LayerPicture (FastString Page, ReportComponentBase obj, FastString text)
        {
            if (Pictures)
            {
                var styleindex = UpdateCSSTable (obj);
                var old_text = string.Empty;

                if (IsMemo (obj))
                {
                    old_text = (obj as TextObject).Text;
                    (obj as TextObject).Text = string.Empty;
                }

                var pic = GetLayerPicture (obj, out var Width, out var Height);

                if (IsMemo (obj))
                {
                    (obj as TextObject).Text = old_text;
                }

                var picStyleBuilder = new FastString ("background: url('")
                    .Append (pic).Append ("') no-repeat !important;-webkit-print-color-adjust:exact;");

                var picStyleIndex = UpdateCSSTable (picStyleBuilder.ToString());


                var style = string.Format ("class=\"{0}s{1} {0}s{2}\"",
                    StylePrefix,
                    styleindex.ToString(), picStyleIndex.ToString());

                //FastString addstyle = new FastString(128);
                //addstyle.Append(" background: url('").Append(pic).Append("') no-repeat !important;-webkit-print-color-adjust:exact;");

                //if (String.IsNullOrEmpty(text))
                //    text = NBSP;

                var x = Width > 0 ? obj.AbsLeft : (obj.AbsLeft + Width);
                var y = Height > 0 ? hPos + obj.AbsTop : (hPos + obj.AbsTop + Height);

                Layer (Page, obj, x, y, Width, Height, text, style, null);
            }
        }

        private void LayerShape (FastString Page, ShapeObject obj, FastString text)
        {
            var addstyle = new FastString (64);

            addstyle.Append (GetStyle());

            addstyle.Append ("background: url('" + GetLayerPicture (obj, out var Width, out var Height) +
                             "');no-repeat !important;-webkit-print-color-adjust:exact;");

            var x = obj.Width > 0 ? obj.AbsLeft : (obj.AbsLeft + obj.Width);
            var y = obj.Height > 0 ? hPos + obj.AbsTop : (hPos + obj.AbsTop + obj.Height);
            Layer (Page, obj, x, y, obj.Width, obj.Height, text, null, addstyle);
        }

        private void LayerBack (FastString Page, ReportComponentBase obj, FastString text)
        {
            if (obj.Border.Shadow)
            {
                using (var shadow = new TextObject())
                {
                    shadow.Left = obj.AbsLeft + obj.Border.ShadowWidth + obj.Border.LeftLine.Width;
                    shadow.Top = obj.AbsTop + obj.Height + obj.Border.BottomLine.Width;
                    shadow.Width = obj.Width + obj.Border.RightLine.Width;
                    shadow.Height = obj.Border.ShadowWidth + obj.Border.BottomLine.Width;
                    shadow.FillColor = obj.Border.ShadowColor;
                    shadow.Border.Lines = BorderLines.None;
                    LayerBack (Page, shadow, null);

                    shadow.Left = obj.AbsLeft + obj.Width + obj.Border.RightLine.Width;
                    shadow.Top = obj.AbsTop + obj.Border.ShadowWidth + obj.Border.TopLine.Width;
                    shadow.Width = obj.Border.ShadowWidth + obj.Border.RightLine.Width;
                    shadow.Height = obj.Height;
                    LayerBack (Page, shadow, null);
                }
            }

            if (obj is not PolyLineObject)
            {
                if (obj.Fill is SolidFill)
                {
                    Layer (Page, obj, obj.AbsLeft, hPos + obj.AbsTop, obj.Width, obj.Height, text,
                        GetStyleTag (UpdateCSSTable (obj)), null);
                }
                else
                {
                    LayerPicture (Page, obj, text);
                }
            }
        }

        private void LayerTable (FastString Page, FastString CSS, TableBase table)
        {
            float y = 0;
            for (var i = 0; i < table.RowCount; i++)
            {
                float x = 0;
                for (var j = 0; j < table.ColumnCount; j++)
                {
                    if (!table.IsInsideSpan (table[j, i]))
                    {
                        var textcell = table[j, i];

                        textcell.Left = x;
                        textcell.Top = y;

                        // custom draw
                        var e = new CustomDrawEventArgs
                        {
                            report = Report,
                            reportObject = textcell,
                            layers = Layers,
                            zoom = Zoom,
                            left = textcell.AbsLeft,
                            top = hPos + textcell.AbsTop,
                            width = textcell.Width,
                            height = textcell.Height
                        };

                        OnCustomDraw (e);
                        if (e.done)
                        {
                            CSS.Append (e.css);
                            Page.Append (e.html);
                        }
                        else
                        {
                            if (textcell is TextObject { TextOutline.Enabled: false } textObject &&
                                IsMemo (textcell))
                            {
                                LayerText (Page, textObject);
                            }
                            else
                            {
                                LayerBack (Page, textcell as ReportComponentBase, null);
                                LayerPicture (Page, textcell as ReportComponentBase, null);
                            }
                        }
                    }

                    x += (table.Columns[j]).Width;
                }

                y += (table.Rows[i]).Height;
            }
        }

        private bool IsMemo (ReportComponentBase Obj)
        {
            if (Obj is TextObject aObj)
            {
                return (aObj.Angle == 0) && aObj is { FontWidthRatio: 1, TextOutline.Enabled: false, Underlines: false };
            }

            return false;
        }

        private void Watermark (FastString Page, ReportPage page, bool drawText)
        {
            using (var pictureWatermark = new PictureObject())
            {
                pictureWatermark.Left = 0;
                pictureWatermark.Top = 0;

                pictureWatermark.Width = (ExportUtils.GetPageWidth (page) - page.LeftMargin - page.RightMargin) *
                                         Units.Millimeters;
                pictureWatermark.Height = (ExportUtils.GetPageHeight (page) - page.TopMargin - page.BottomMargin) *
                                          Units.Millimeters;

                pictureWatermark.SizeMode = PictureBoxSizeMode.Normal;
                pictureWatermark.Image = new Bitmap ((int)pictureWatermark.Width, (int)pictureWatermark.Height);

                using (var g = Graphics.FromImage (pictureWatermark.Image))
                {
                    g.Clear (Color.Transparent);
                    if (drawText)
                    {
                        page.Watermark.DrawText (new FRPaintEventArgs (g, 1f, 1f, Report.GraphicCache),
                            new RectangleF (0, 0, pictureWatermark.Width, pictureWatermark.Height), Report, true);
                    }
                    else
                    {
                        page.Watermark.DrawImage (new FRPaintEventArgs (g, 1f, 1f, Report.GraphicCache),
                            new RectangleF (0, 0, pictureWatermark.Width, pictureWatermark.Height), Report, true);
                    }

                    pictureWatermark.Transparency = page.Watermark.ImageTransparency;
                    LayerBack (Page, pictureWatermark, null);
                    LayerPicture (Page, pictureWatermark, null);
                }
            }
        }

        private void ExportHTMLPageLayeredBegin (HTMLData d)
        {
            if (!SinglePage && !WebMode)
            {
                cssStyles.Clear();
            }

            css = new FastString();
            htmlPage = new FastString();

            var reportPage = d.page;

            if (reportPage != null)
            {
                maxWidth = ExportUtils.GetPageWidth (reportPage) * Units.Millimeters;
                maxHeight = ExportUtils.GetPageHeight (reportPage) * Units.Millimeters;


                if (EnableMargins)
                {
                    leftMargin = reportPage.LeftMargin * Units.Millimeters;
                    topMargin = reportPage.TopMargin * Units.Millimeters;
                }
                else
                {
                    maxWidth -= (reportPage.LeftMargin + reportPage.RightMargin) * Units.Millimeters;
                    maxHeight -= (reportPage.TopMargin + reportPage.BottomMargin) * Units.Millimeters;
                    leftMargin = 0;
                    topMargin = 0;
                }

                currentPage = d.PageNumber - 1;

                ExportHTMLPageStart (htmlPage, d.PageNumber, d.CurrentPage);

                doPageBreak = (SinglePage && PageBreaks);

                htmlPage.Append (HTMLGetAncor ((d.PageNumber).ToString()));

                if (doPageBreak && d.PageNumber > 1)
                {
                    htmlPage.Append ("<div style=\"break-after:page\"></div>");
                }

                pageStyleName = "frpage" + currentPage;
                htmlPage.Append ("<div ").Append (doPageBreak ? "class=\"" + pageStyleName + "\"" : string.Empty)
                    .Append (" style=\"position:relative;")
                    .Append (" width:").Append (Px (maxWidth * Zoom + 3))
                    .Append (" height:").Append (Px (maxHeight * Zoom));

                if (reportPage.Fill is SolidFill fill)
                {
                    htmlPage.Append (" background-color:")
                        .Append (fill.IsTransparent ? "transparent" : ExportUtils.HTMLColor (fill.Color));
                    if (ExportMode == ExportType.WebPrint)
                    {
                        htmlPage.Append (
                            "color-adjust: exact !important; print-color-adjust: exact !important; -webkit-print-color-adjust: exact !important;");
                    }
                }
                else
                {
                    // to-do for picture background
                }

                htmlPage.Append ("\">");

                if (reportPage.Watermark is { Enabled: true, ShowImageOnTop: false })
                {
                    Watermark (htmlPage, reportPage, false);
                }

                if (reportPage.Watermark is { Enabled: true, ShowTextOnTop: false })
                {
                    Watermark (htmlPage, reportPage, true);
                }
            }
        }

        private void ExportHTMLPageLayeredEnd (HTMLData d)
        {
            // to do
            if (d.page.Watermark is { Enabled: true, ShowImageOnTop: true })
            {
                Watermark (htmlPage, d.page, false);
            }

            if (d.page.Watermark is { Enabled: true, ShowTextOnTop: true })
            {
                Watermark (htmlPage, d.page, true);
            }

            ExportPageStylesLayers (css, d.PageNumber);

            if (SinglePage)
            {
                hPos = 0;
                prevStyleListIndex = cssStyles.Count;
            }

            htmlPage.Append ("</div>");

            ExportHTMLPageFinal (css, htmlPage, d, maxWidth, maxHeight);
        }

        private void ExportBandLayers (BandBase band)
        {
            LayerBack (htmlPage, band, null);
            foreach (Base c in band.ForEachAllConvectedObjects (this))
            {
                if (ExportMode == ExportType.WebPreview)
                {
                    SetExportableAdvMatrix (c);
                }

                if (c is ReportComponentBase { Exportable: true } @base)
                {
                    var obj = @base;

                    // custom draw
                    var e = new CustomDrawEventArgs
                    {
                        report = Report,
                        reportObject = obj,
                        layers = Layers,
                        zoom = Zoom,
                        left = obj.AbsLeft,
                        top = hPos + obj.AbsTop,
                        width = obj.Width,
                        height = obj.Height
                    };

                    OnCustomDraw (e);
                    if (e.done)
                    {
                        css.Append (e.css);
                        htmlPage.Append (e.html);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty (obj.Bookmark))
                        {
                            htmlPage.Append ("<a name=\"").Append (obj.Bookmark).Append ("\"></a>");
                        }

                        if (obj is CellularTextObject textObject)
                        {
                            obj = textObject.GetTable();
                        }

                        if (obj is TableCell)
                        {
                            continue;
                        }
                        else if (obj is TableBase table)
                        {
                            if (table is { ColumnCount: > 0, RowCount: > 0 })
                            {
                                using (var tableback = new TextObject())
                                {
                                    tableback.Border = table.Border;
                                    tableback.Fill = table.Fill;
                                    tableback.FillColor = table.FillColor;
                                    tableback.Left = table.AbsLeft;
                                    tableback.Top = table.AbsTop;
                                    float tableWidth = 0;
                                    float tableHeight = 0;

                                    for (var i = 0; i < table.ColumnCount; i++)
                                    {
                                        tableWidth += table[i, 0].Width;
                                    }

                                    for (var i = 0; i < table.RowCount; i++)
                                    {
                                        tableHeight += table.Rows[i].Height;
                                    }

                                    tableback.Width = (tableWidth < table.Width) ? tableWidth : table.Width;
                                    tableback.Height = tableHeight;
                                    LayerText (htmlPage, tableback);
                                }

                                LayerTable (htmlPage, css, table);
                            }
                        }
                        else if (IsMemo (obj))
                        {
                            LayerText (htmlPage, obj as TextObject);
                        }
                        else if (obj is HtmlObject htmlObject)
                        {
                            LayerHtml (htmlPage, htmlObject);
                        }
                        else if (obj is BandBase)
                        {
                            LayerBack (htmlPage, obj, null);
                        }
                        else if (obj is LineObject)
                        {
                            LayerPicture (htmlPage, obj, null);
                        }
                        else if (obj is ShapeObject shapeObject)
                        {
                            LayerShape (htmlPage, shapeObject, null);
                        }
                        else if (HasExtendedExport (obj))
                        {
                            ExtendExport (htmlPage, obj, null);
                        }
                        else
                        {
                            LayerBack (htmlPage, obj, null);
                            LayerPicture (htmlPage, obj, null);
                        }
                    }
                }
            }
        }
    }
}
