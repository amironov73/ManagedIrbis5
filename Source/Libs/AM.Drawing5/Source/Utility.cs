// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* Utility.cs -- сборник простых вспомогательных методов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;

#endregion

#nullable enable

namespace AM.Drawing
{
    /// <summary>
    /// Сборник простых вспомогательных методов.
    /// </summary>
    public static class Utility
    {
        #region Private members

        /// <summary>
        /// For comparing with zero.
        /// </summary>
        private const double Tolerance = 0.001;

        /// <summary>
        /// Нормализация компонента цвета.
        /// </summary>
        private static int _Normalize
            (
                float component
            )
        {
            return Math.Max(0, Math.Min(255, (int)component));
        }

        private static int _ToRGB1
            (
                float rm1,
                float rm2,
                float rh
            )
        {
            if (rh > 360.0f)
            {
                rh -= 360.0f;
            }
            else if (rh < 0.0f)
            {
                rh += 360.0f;
            }

            if (rh < 60.0f)
            {
                rm1 = rm1 + (rm2 - rm1) * rh / 60.0f;
            }
            else if (rh < 180.0f)
            {
                rm1 = rm2;
            }
            else if (rh < 240.0f)
            {
                rm1 = rm1 + (rm2 - rm1) * (240.0f - rh) / 60.0f;
            }

            return _Normalize(rm1 * 255);
        }

        private static void _DrawPath
            (
                Graphics g,
                Pen? pen,
                GraphicsPath path
            )
        {
            if (pen != null)
            {
                g.DrawPath(pen, path);
            }
        }

        private static PointF _LastPoint
            (
                GraphicsPath path
            )
        {
            PointF[] points = path.PathPoints;
            return points[^1];
        }

        private static ColorMatrix _Blend(float val)
        {
            float[][] matrixItems =
            {
                new float[] {1, 0, 0, 0, 0},
                new float[] {0, 1, 0, 0, 0},
                new float[] {0, 0, 1, 0, 0},
                new float[] {0, 0, 0, val, 0},
                new float[] {0, 0, 0, 0, 1}
            };
            return new ColorMatrix(matrixItems);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Смешение двух цветов в заданной пропорции.
        /// </summary>
        /// <param name="color1">Первый цвет.</param>
        /// <param name="color2">Второй цвет.</param>
        /// <param name="amount">Доля второго цвета (число от 0 до 1,
        /// 0 - остается только первый цвет, 1 - остается только второй
        /// цвет).</param>
        /// <returns>Смешанный цвет.</returns>
        public static Color Blend
            (
                Color color1,
                Color color2,
                float amount
            )
        {
            var amount1 = 1f - amount;
            var red = _Normalize(color1.R * amount1 + color2.R * amount);
            var green = _Normalize(color1.G * amount1 + color2.G * amount);
            var blue = _Normalize(color1.B * amount1 + color2.B * amount);

            return Color.FromArgb(red, green, blue);
        }

        /// <summary>
        /// Затемнение цвета (на самом деле лишь подмешивание черного цвета).
        /// </summary>
        /// <param name="color">Затемняемый цвет.</param>
        /// <param name="amount">Степень затемнения (доля черного цвета,
        /// число от 0 до 1).</param>
        /// <returns>Затемненный цвет.</returns>
        public static Color Darken
            (
                Color color,
                float amount
            )
        {
            return Blend(color, Color.Black, amount);
        }

        /// <summary>
        /// Создает цвет из компонент "Hue/Luminance/Saturation".
        /// </summary>
        public static Color FromHls
            (
                float hue,
                float luminance,
                float saturation
            )
        {
            //Debug.Assert((hue >= 0f) && (hue <= 360f));
            //Debug.Assert((luminance >= 0f) && (luminance <= 1f));
            //Debug.Assert((saturation >= 0f) && (saturation <= 1f));

            int red, green, blue;
            if (Math.Abs(saturation) < Tolerance)
            {
                red = _Normalize(luminance * 255.0F);
                green = red;
                blue = red;
            }
            else
            {
                float rm2;

                if (luminance <= 0.5f)
                {
                    rm2 = luminance + luminance * saturation;
                }
                else
                {
                    rm2 = luminance + saturation
                        - luminance * saturation;
                }
                var rm1 = 2.0f * luminance - rm2;
                red = _ToRGB1(rm1, rm2, hue + 120.0f);
                green = _ToRGB1(rm1, rm2, hue);
                blue = _ToRGB1(rm1, rm2, hue - 120.0f);
            }

            return Color.FromArgb(red, green, blue);
        }

        /// <summary>
        /// Осветление цвета (на самом деле лишь подмешнивание белого цвета).
        /// </summary>
        /// <param name="color">Осветляемый цвет.</param>
        /// <param name="amount">Степень осветления (доля белого цвета,
        /// число от 0 до 1).</param>
        /// <returns>Осветленный цвет.</returns>
        public static Color Lighten
            (
                Color color,
                float amount
            )
        {
            return Blend(color, Color.White, amount);
        }

        /// <summary>
        /// Convert color to Blue-Green-Red representation.
        /// </summary>
        public static int ToBgr
            (
                Color color
            )
        {
            return unchecked((((color.B << 8) + color.G) << 8) + color.R);
        }

        /// <summary>
        /// Returns a color based on XAML color string.
        /// </summary>
        /// <param name="colorString">The color string.
        /// Any format used in XAML should work.</param>
        /// <returns>Parsed color</returns>
        /// <remarks>
        /// Borrowed from UWPCommunityToolkit:
        /// https://github.com/Microsoft/UWPCommunityToolkit/blob/dev/Microsoft.Toolkit.Uwp/Helpers/ColorHelper.cs
        /// </remarks>
        public static Color ToColor
            (
                this string colorString
            )
        {
            if (colorString[0] == '#')
            {
                uint temp;
                byte a, r, g, b;

                switch (colorString.Length)
                {
                    case 9:
                        temp = Convert.ToUInt32(colorString.Substring(1), 16);
                        a = (byte)(temp >> 24);
                        r = (byte)((temp >> 16) & 0xff);
                        g = (byte)((temp >> 8) & 0xff);
                        b = (byte)(temp & 0xff);

                        return Color.FromArgb(a, r, g, b);

                    case 7:
                        temp = Convert.ToUInt32(colorString.Substring(1), 16);
                        r = (byte)((temp >> 16) & 0xff);
                        g = (byte)((temp >> 8) & 0xff);
                        b = (byte)(temp & 0xff);

                        return Color.FromArgb(255, r, g, b);

                    case 5:
                        temp = Convert.ToUInt16(colorString.Substring(1), 16);
                        a = (byte)(temp >> 12);
                        r = (byte)((temp >> 8) & 0xf);
                        g = (byte)((temp >> 4) & 0xf);
                        b = (byte)(temp & 0xf);
                        a = (byte)(a << 4 | a);
                        r = (byte)(r << 4 | r);
                        g = (byte)(g << 4 | g);
                        b = (byte)(b << 4 | b);

                        return Color.FromArgb(a, r, g, b);

                    case 4:
                        temp = Convert.ToUInt16(colorString.Substring(1), 16);
                        r = (byte)((temp >> 8) & 0xf);
                        g = (byte)((temp >> 4) & 0xf);
                        b = (byte)(temp & 0xf);
                        r = (byte)(r << 4 | r);
                        g = (byte)(g << 4 | g);
                        b = (byte)(b << 4 | b);

                        return Color.FromArgb(255, r, g, b);

                    default:
                        throw new FormatException
                            (
                                string.Format
                                (
                                    "The {0} string passed in the colorString "
                                    + "argument is not a recognized Color format.",
                                    colorString
                                )
                            );
                }
            }

            if (
                    colorString.Length > 3
                    && colorString[0] == 's'
                    && colorString[1] == 'c'
                    && colorString[2] == '#'
                )
            {
                var values = colorString.Split(',');

                if (values.Length == 4)
                {
                    var scA = double.Parse(values[0].Substring(3));
                    var scR = double.Parse(values[1]);
                    var scG = double.Parse(values[2]);
                    var scB = double.Parse(values[3]);

                    return Color.FromArgb
                        (
                            (byte)(scA * 255),
                            (byte)(scR * 255),
                            (byte)(scG * 255),
                            (byte)(scB * 255)
                        );
                }

                if (values.Length == 3)
                {
                    var scR = double.Parse(values[0].Substring(3));
                    var scG = double.Parse(values[1]);
                    var scB = double.Parse(values[2]);

                    return Color.FromArgb
                        (
                            255,
                            (byte)(scR * 255),
                            (byte)(scG * 255),
                            (byte)(scB * 255)
                        );
                }

                throw new FormatException
                    (
                        string.Format
                        (
                            "The {0} string passed in the colorString "
                            + "argument is not a recognized Color format "
                            + "(sc#[scA,]scR,scG,scB).",
                            colorString
                        )
                    );
            }

            throw new FormatException
                (
                    string.Format
                    (
                        "The {0} string passed in the colorString argument "
                        + "is not a recognized Color.",
                        colorString
                    )
                );
        }

        /// <summary>
        /// Converts <see cref="Font"/> to a <see cref="String"/>
        /// (e. g. for XML serialization).
        /// </summary>
        public static string FontToString
            (
                Font font
            )
        {
            var result = string.Format
                (
                    "{0}|{1}|{2}|{3}|{4}|{5}|{6}",
                    font.Name,
                    font.Size,
                    font.Unit,
                    font.Bold,
                    font.Italic,
                    font.Underline,
                    font.Strikeout
                );

            return result;
        }

        /// <summary>
        /// Restores <see cref="Font"/> from a <see cref="String"/>
        /// (e. g. during XML deserialization).
        /// </summary>
        public static Font StringToFont
            (
                string fontSpecification
            )
        {
            var parts = fontSpecification.Split('|');
            if (parts.Length != 7)
            {
                throw new ArgumentException("fontSpecification");
            }
            var fontName = parts[0];
            var emSize = float.Parse(parts[1]);
            var unit = (GraphicsUnit)Enum.Parse(typeof(GraphicsUnit), parts[2]);
            var bold = bool.Parse(parts[3]);
            var italic = bool.Parse(parts[4]);
            var underline = bool.Parse(parts[5]);
            var strikeout = bool.Parse(parts[6]);
            var style = FontStyle.Regular;
            if (bold)
            {
                style |= FontStyle.Bold;
            }
            if (italic)
            {
                style |= FontStyle.Italic;
            }
            if (underline)
            {
                style |= FontStyle.Underline;
            }
            if (strikeout)
            {
                style |= FontStyle.Strikeout;
            }
            var result = new Font
                (
                    fontName,
                    emSize,
                    style,
                    unit
                );

            return result;
        }

        /// <summary>
        /// Get codec info.
        /// </summary>
        /// <param name="mimeType">For example "image/jpeg".
        /// </param>
        /// <returns>ImageCodeInfo or null.</returns>
        public static ImageCodecInfo? GetCodecInfo
            (
                string mimeType
            )
        {
            var codecs = ImageCodecInfo.GetImageEncoders();
            foreach (var codec in codecs)
            {
                if (codec.MimeType == mimeType)
                {
                    return codec;
                }
            }

            return null;
        }

        /// <summary>
        /// Сохраняет картинку в памяти.
        /// </summary>
        public static byte[] SaveToMemory
            (
                Image image,
                ImageFormat format
            )
        {
            var memory = new MemoryStream();
            image.Save(memory, format);

            return memory.ToArray();
        }

        /// <summary>
        /// Загружает картинку из памяти.
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static Image LoadFromMemory(byte[] bytes)
        {
            Stream memory = new MemoryStream(bytes, false);
            return Image.FromStream(memory);
        }

        /// <summary>
        /// Загружаем картинку из файла.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        /// <remarks>Этот нехитрый трюк нужен, чтобы не блокировать
        /// файл, как это обычно делает System.Drawing.Image.FromFile().
        /// </remarks>
        public static Image LoadFromFile
            (
                string fileName
            )
        {
            var memory = new MemoryStream
                (
                    File.ReadAllBytes(fileName),
                    false
                );

            return Image.FromStream(memory);
        }

        /// <summary>
        /// Загружает картинку из ресурсов .NET.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="resourceName"></param>
        /// <returns></returns>
        public static Image LoadFromResource
            (
                Assembly assembly,
                string resourceName
            )
        {
            using var stream = assembly.GetManifestResourceStream(resourceName);

            return Image.FromStream(stream);
        }

        /// <summary>
        /// Загружает картинку из ресурсов .NET.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="resourceName"></param>
        /// <returns></returns>
        public static Image LoadFromResource
            (
                Type type,
                string resourceName
            )
        {
            return LoadFromResource(type.Assembly, resourceName);
        }

        /// <summary>
        /// Пропорционально масштабирует изображение так, чтобы оно
        /// вписывалось в указанные размеры.
        /// </summary>
        public static Image ProportionalResize
            (
                Image image,
                int width,
                int height
            )
        {
            double imageHeight = image.Height;
            double imageWidth = image.Width;
            double windowHeight = width;
            double windowWidth = height;
            var imageAspect = imageWidth / imageHeight;
            var panelAspect = windowWidth / windowHeight;
            var superAspect = imageAspect / panelAspect;
            var ratio = (superAspect > 1.0)
                ? windowWidth / imageWidth
                : windowHeight / imageHeight;
            imageWidth *= ratio;
            imageHeight *= ratio;
            var result = new Bitmap(image, (int)imageWidth,
                (int)imageHeight);

            return result;
        }

        /// <summary>
        /// Save bitmap in JPEG file with given quality.
        /// </summary>
        public static void SaveJpeg
            (
                Image img,
                string fileName,
                long quality
            )
        {
            var ici = GetCodecInfo("image/jpeg");
            var par0 = new EncoderParameter(Encoder.Quality,
                quality);
            var parms = new EncoderParameters(1);
            parms.Param = new EncoderParameter[] {
                par0
            };
            img.Save(fileName, ici, parms);
        }

        /// <summary>
        /// Получение копии рисунка с исправленной гаммой.
        /// </summary>
        public static Image ReGamma
            (
                Image image,
                float gamma
            )
        {
            return ReGamma
                (
                    image,
                    new Rectangle
                    (
                        0,
                        0,
                        image.Width,
                        image.Height
                    ),
                    gamma
                );
        }

        /// <summary>
        /// Получение копии рисунка с исправленной гаммой.
        /// </summary>
        public static Image ReGamma
            (
                Image image,
                Rectangle dstRect,
                float gamma
            )
        {
            Image result = new Bitmap(image.Width, image.Height);
            using (var g = Graphics.FromImage(result))
            using (var attr = new ImageAttributes())
            {
                attr.SetGamma(gamma, ColorAdjustType.Bitmap);
                g.DrawImage(image, dstRect,
                    0f, 0f, image.Width, image.Height,
                    GraphicsUnit.Pixel, attr);
            }

            return result;
        }

        /// <summary>
        /// Смешение рисунков.
        /// </summary>
        /// <param name="bitmap1"></param>
        /// <param name="bitmap2"></param>
        /// <param name="amount">Степень проявления второго рисунка:
        /// от 0.0f до 1.0f.</param>
        /// <returns></returns>
        /// <remarks>Результирующий рисунок имеет размер первого.
        /// </remarks>
        public static Bitmap BlendImages
            (
                Bitmap bitmap1,
                Bitmap bitmap2,
                float amount
            )
        {
            Bitmap result = new Bitmap(bitmap1);
            using (Graphics g = Graphics.FromImage(result))
            {
                Rectangle r = new Rectangle(0,
                                              0,
                                              bitmap1.Width,
                                              bitmap1.Height);
                DrawSemitransparentImage(g, bitmap2, r, amount);
            }

            return result;
        }

        /// <summary>
        /// Creates the rounded rectangle path.
        /// </summary>
        /// <param name="r">The r.</param>
        /// <param name="s">The s.</param>
        /// <returns></returns>
        public static GraphicsPath CreateRoundedRectanglePath
            (
                Rectangle r,
                Size s
            )
        {
            int height = s.Height;
            int height2 = height / 2;
            int width = s.Width;
            int width2 = width / 2;

            GraphicsPath result = new GraphicsPath();
            result.AddLine(r.Left + width2, r.Top, r.Right - width2, r.Top);
            result.AddArc(r.Right - width, r.Top, width, height, 270, 90);
            result.AddLine(r.Right, r.Top + height2, r.Right, r.Bottom - height2);
            result.AddArc(r.Right - width, r.Bottom - height, width, height, 0, 90);
            result.AddLine(r.Right - width2, r.Bottom, r.Left + width2, r.Bottom);
            result.AddArc(r.Left, r.Bottom - height, width, height, 90, 90);
            result.AddLine(r.Left, r.Bottom - height2, r.Left, r.Top + height2);
            result.AddArc(r.Left, r.Top, width, height, 180, 90);
            //result.CloseFigure ();

            return result;
        }

        /// <summary>
        /// Рисует трехмерную "коробку".
        /// </summary>
        public static void DrawBox3D
            (
                Graphics g,
                Rectangle rect,
                Size size,
                Brush? mainBrush,
                Brush? auxBrush,
                Pen? pen
            )
        {
            if (!ReferenceEquals(mainBrush, null))
            {
                g.FillRectangle(mainBrush, rect);
            }
            Point[] top = Points
                (
                    rect.Left,
                    rect.Top,
                    rect.Left + size.Width,
                    rect.Top - size.Height,
                    rect.Right + size.Width,
                    rect.Top - size.Height,
                    rect.Right,
                    rect.Top
                );
            Point[] right = Points
                (
                    rect.Right,
                    rect.Top,
                    rect.Right + size.Width,
                    rect.Top - size.Height,
                    rect.Right + size.Width,
                    rect.Bottom - size.Height,
                    rect.Right,
                    rect.Bottom
                );
            if (!ReferenceEquals(auxBrush, null))
            {
                g.FillPolygon(auxBrush, top);
                g.FillPolygon(auxBrush, right);
            }
            if (!ReferenceEquals(pen, null))
            {
                g.DrawRectangle(pen, rect);
                g.DrawPolygon(pen, top);
                g.DrawPolygon(pen, right);
            }
        }

        /// <summary>
        /// Рисует конус.
        /// </summary>
        public static void DrawCone
            (
                Graphics g,
                Rectangle rect,
                int cap,
                Brush? brush,
                Pen? pen
            )
        {
            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddLines(Points
                    (
                        rect.Left,
                        rect.Bottom,
                        rect.Left + rect.Width / 2,
                        rect.Top,
                        rect.Right,
                        rect.Bottom
                    ));
                path.AddArc
                    (
                        rect.Left,
                        rect.Bottom - cap,
                        rect.Width,
                        cap * 2,
                        0,
                        180
                    );
                if (!ReferenceEquals(brush, null))
                {
                    using (Region region = new Region(path))
                    {
                        g.FillRegion(brush, region);
                    }
                }
                if (!ReferenceEquals(pen, null))
                {
                    g.DrawPath(pen, path);
                }
            }
        }

        /// <summary>
        /// Закрашивает и обводит замкнутую кривую линию.
        /// </summary>
        public static void DrawClosedCurve
            (
                Graphics g,
                Brush? brush,
                Pen? pen,
                params int[] points
            )
        {
            Point[] pts = Points(points);
            if (!ReferenceEquals(brush, null))
            {
                g.FillClosedCurve(brush, pts);
            }
            if (!ReferenceEquals(pen, null))
            {
                g.DrawClosedCurve(pen, pts);
            }
        }

        /// <summary>
        /// Рисует цилиндр.
        /// </summary>
        public static void DrawCylinder
            (
                Graphics g,
                Rectangle rect,
                int cap,
                Brush? mainBrush,
                Brush? auxBrush,
                Pen? pen
            )
        {
            Rectangle capRect = new Rectangle
                (
                    rect.Left,
                    rect.Top - cap,
                    rect.Width,
                    cap * 2
                );
            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddLines(Points
                    (
                        rect.Left,
                        rect.Bottom,
                        rect.Left,
                        rect.Top,
                        rect.Right,
                        rect.Top,
                        rect.Right,
                        rect.Bottom
                    ));
                path.AddArc
                    (
                        rect.Left,
                        rect.Bottom - cap,
                        rect.Width,
                        cap * 2,
                        0,
                        180
                    );
                if (!ReferenceEquals(mainBrush, null))
                {
                    using (Region region = new Region(path))
                    {
                        g.FillRegion(mainBrush, region);
                    }
                }
                if (!ReferenceEquals(pen, null))
                {
                    g.DrawPath(pen, path);
                }
                if (!ReferenceEquals(auxBrush, null))
                {
                    g.FillEllipse(auxBrush, capRect);
                }
                if (!ReferenceEquals(pen, null))
                {
                    g.DrawEllipse(pen, capRect);
                }
            }
        }

        /// <summary>
        /// Рисование цилиндрика.
        /// </summary>
        public static void DrawCylinder3D
            (
                Graphics g,
                Rectangle rectangle,
                int capHeight,
                Color color,
                Pen pen
            )
        {
            using (LinearGradientBrush mainBrush =
                new LinearGradientBrush(rectangle, color, color, 0f))
            {
                using (Brush capBrush = new SolidBrush(
                    Utility.Darken(color, 0.3f)))
                {
                    ColorBlend blend = new ColorBlend
                    {
                        Colors = new[]
                        {
                            color,
                            Utility.Lighten(color, 0.8f),
                            color,
                            Utility.Darken(color, 0.6f)
                        },
                        Positions = new[]
                        {
                            0f,
                            0.15f,
                            0.50f,
                            1f
                        }
                    };
                    mainBrush.InterpolationColors = blend;
                    DrawCylinder
                        (
                        g,
                        rectangle,
                        capHeight,
                        mainBrush,
                        capBrush,
                        null
                        );
                }
            }
        }

        /// <summary>
        /// Рисование с учетом гаммы.
        /// </summary>
        public static void DrawImageWithGamma
            (
                Graphics g,
                Image image,
                Rectangle rectangle,
                float gamma
            )
        {
            using (var attributes = new ImageAttributes())
            {
                attributes.SetGamma(gamma);
                g.DrawImage
                    (
                        image,
                        rectangle,
                        0,
                        0,
                        image.Width,
                        image.Height,
                        GraphicsUnit.Pixel,
                        attributes
                    );
            }
        }

        /// <summary>
        /// Рисует линию.
        /// </summary>
        public static void DrawLines
            (
                Graphics g,
                Pen pen,
                params int[] points
            )
        {
            g.DrawLines(pen, Points(points));
        }

        /// <summary>
        /// Закрашивает и обводит многоугольник.
        /// </summary>
        public static void DrawPolygon
            (
                Graphics g,
                Brush? brush,
                Pen? pen,
                params int[] points
            )
        {
            Point[] arrayOfPoints = Points(points);
            if (!ReferenceEquals(brush, null))
            {
                g.FillPolygon(brush, arrayOfPoints);
            }
            if (!ReferenceEquals(pen, null))
            {
                g.DrawPolygon(pen, arrayOfPoints);
            }
        }

        /// <summary>
        /// Рисует прямоугольник со скругленными углами.
        /// </summary>
        public static void DrawRoundRectangle
            (
                Graphics g,
                Pen pen,
                Rectangle rect,
                Size corners
            )
        {
            int width1 = corners.Width;
            int height1 = corners.Height;
            int width2 = width1 / 2;
            int height2 = height1 / 2;

            // Верхний сегмент
            g.DrawLine(pen,
                         rect.Left + width2,
                         rect.Top,
                         rect.Right - width2,
                         rect.Top);
            // Верхний правый
            g.DrawArc(pen,
                        rect.Right - width1,
                        rect.Top,
                        width1,
                        height1,
                        270,
                        90);
            // Правый сегмент
            g.DrawLine(pen,
                         rect.Right,
                         rect.Top + height2,
                         rect.Right,
                         rect.Bottom - height2);
            // Правый нижний
            g.DrawArc(pen,
                        rect.Right - width1,
                        rect.Bottom - height1,
                        width1,
                        height1,
                        0,
                        90);
            // Нижний сегмент
            g.DrawLine(pen,
                         rect.Left + width2,
                         rect.Bottom,
                         rect.Right - width2,
                         rect.Bottom);
            // Левый нижний
            g.DrawArc(pen,
                        rect.Left,
                        rect.Bottom - height1,
                        width1,
                        height1,
                        90,
                        90);
            // Левый сегмент
            g.DrawLine(pen,
                         rect.Left,
                         rect.Top + height2,
                         rect.Left,
                         rect.Bottom - height2);
            // Левый верхний
            g.DrawArc(pen, rect.Left, rect.Top, width1, height1, 180, 90);
        }

        /// <summary>
        /// Полупрозрачное рисование.
        /// </summary>
        public static void DrawSemitransparentImage
            (
                Graphics g,
                Image image,
                Rectangle rectangle,
                float amount
            )
        {
            ColorMatrix m = _Blend(amount);
            using (ImageAttributes attributes
                = new ImageAttributes())
            {
                attributes.SetColorMatrix
                    (
                        m,
                        ColorMatrixFlag.Default,
                        ColorAdjustType.Bitmap
                    );
                g.DrawImage
                    (
                        image,
                        rectangle,
                        0,
                        0,
                        image.Width,
                        image.Height,
                        GraphicsUnit.Pixel,
                        attributes
                    );
            }
        }

        /// <summary>
        /// Закрашивает прямоугольник со скругленными углами.
        /// </summary>
        public static void FillRoundRectangle
            (
                Graphics g,
                Brush brush,
                Rectangle rect,
                Size corners
            )
        {
            int width1 = corners.Width;
            int height1 = corners.Height;
            int width2 = width1 / 2;
            int height2 = height1 / 2;

            GraphicsPath path = new GraphicsPath();
            // Верхний сегмент
            path.AddLine(rect.Left + width2,
                           rect.Top,
                           rect.Right - width2,
                           rect.Top);
            // Верхний правый
            path.AddArc(rect.Right - width1,
                          rect.Top,
                          width1,
                          height1,
                          270,
                          90);
            // Правый сегмент
            path.AddLine(rect.Right,
                           rect.Top + height2,
                           rect.Right,
                           rect.Bottom - height2);
            // Правый нижний
            path.AddArc(rect.Right - width1,
                          rect.Bottom - height1,
                          width1,
                          height1,
                          0,
                          90);
            // Нижний сегмент
            path.AddLine(rect.Left + width2,
                           rect.Bottom,
                           rect.Right - width2,
                           rect.Bottom);
            // Левый нижний
            path.AddArc(rect.Left,
                          rect.Bottom - height1,
                          width1,
                          height1,
                          90,
                          90);
            // Левый сегмент
            path.AddLine(rect.Left,
                           rect.Top + height2,
                           rect.Left,
                           rect.Bottom - height2);
            // Левый верхний
            path.AddArc(rect.Left, rect.Top, width1, height1, 180, 90);
            path.CloseFigure();
            Region region = new Region(path);
            g.FillRegion(brush, region);
            region.Dispose();
            path.Dispose();
        }

        /// <summary>
        /// Вписывает текст в прямоугольник, заполняя его полностью.
        /// </summary>
        public static void FitString
            (
                Graphics g,
                string text,
                Rectangle rect,
                Font font,
                Brush brush,
                Pen pen
            )
        {
            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddString
                    (
                    text,
                    font.FontFamily,
                    (int)font.Style,
                    font.Size,
                    new Point(0, 0),
                        TextFormat.NearNear
                    );
                RectangleF textRect = path.GetBounds();
                PointF[] points = new PointF[]
                    {
                        new PointF ( rect.Left, rect.Top ),
                        new PointF ( rect.Right, rect.Top ),
                        new PointF ( rect.Left, rect.Bottom )
                    };
                using (new GraphicsStateSaver(g))
                {
                    g.Transform = new Matrix(textRect, points);
                    if (brush != null)
                    {
                        g.FillPath(brush, path);
                    }
                    if (pen != null)
                    {
                        g.DrawPath(pen, path);
                    }
                }
            }
        }

        /// <summary>
        /// Выводит строку с толстой обводкой.
        /// </summary>
        public static void HollowString
            (
                Graphics g,
                string text,
                Rectangle rect,
                Font font,
                Brush brush,
                Pen pen,
                int width,
                StringFormat format
            )
        {
            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddString(text,
                                 font.FontFamily,
                                 (int)font.Style,
                                 font.Size,
                                 rect,
                                 format);
                using (Pen tempPen = new Pen(Color.Black, width))
                {
                    tempPen.LineJoin = LineJoin.Round;
                    path.Widen(tempPen);
                }
                using (new GraphicsStateSaver(g))
                {
                    using (Region clipRegion = new Region(rect))
                    {
                        g.Clip = clipRegion;
                        if (pen != null)
                        {
                            g.DrawPath(pen, path);
                        }
                        if (brush != null)
                        {
                            g.FillPath(brush, path);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Нормализация угла, выраженного в градусах (приведение
        /// к интервалу от 0 до 360 градусов).
        /// </summary>
        public static float NormalizeAngle
            (
                float angle
            )
        {
            while (angle > 360f)
            {
                angle -= 360f;
            }
            while (angle < 0f)
            {
                angle += 360f;
            }

            return angle;
        }

        /// <summary>
        /// Рисует текст с ободкой.
        /// </summary>
        public static void PaintString
            (
                Graphics g,
                string text,
                Rectangle rect,
                Font font,
                Brush brush,
                Pen pen,
                StringFormat format
            )
        {
            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddString(text,
                                 font.FontFamily,
                                 (int)font.Style,
                                 font.Size,
                                 rect,
                                 format);
                using (new GraphicsStateSaver(g))
                {
                    using (Region clipRegion = new Region(rect))
                    {
                        g.Clip = clipRegion;
                        if (brush != null)
                        {
                            g.FillPath(brush, path);
                        }
                        if (pen != null)
                        {
                            g.DrawPath(pen, path);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Создает массив точек.
        /// </summary>
        public static Point[] Points
            (
                params int[] values
            )
        {
            //Debug.Assert((values.Length % 2) == 0);
            Point[] result = new Point[values.Length / 2];
            for (int i = 0,
                      j = 0;
                  i < result.Length;
                  i++)
            {
                result[i].X = values[j++];
                result[i].Y = values[j++];
            }
            return result;
        }

        /// <summary>
        /// Создает массив точек.
        /// </summary>
        public static PointF[] Points
            (
                params float[] values
            )
        {
            //Debug.Assert((values.Length % 2) == 0);
            PointF[] result = new PointF[values.Length / 2];
            for (int i = 0,
                      j = 0;
                  i < result.Length;
                  i++)
            {
                result[i].X = values[j++];
                result[i].Y = values[j++];
            }
            return result;
        }

        /// <summary>
        /// Преобразует систему координат в очень простую: точка (0,0)
        /// расположена точно в центре, от нее до любой границы ровно 1000
        /// попугаев.
        /// </summary>
        public static void UniformCoordinateSystem
            (
                Graphics g,
                Size size
            )
        {
            g.TranslateTransform
                (
                    (float)size.Width / 2,
                    (float)size.Height / 2
                );
            float inches = Math.Min(size.Width / g.DpiX, size.Height / g.DpiY);
            g.ScaleTransform(inches * g.DpiX / 2000, inches * g.DpiY / 2000);
        }

        /// <summary>
        /// Рисование шарика.
        /// </summary>
        public static void DrawSphere
            (
                Graphics g,
                Rectangle rectangle,
                Color color,
                Pen pen
            )
        {
            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddEllipse(rectangle);
                using (PathGradientBrush brush =
                    new PathGradientBrush(path))
                {
                    Color lightColor = Utility.Lighten(color, 0.8f);
                    Color darkColor = Utility.Darken(color, 0.6f);
                    var blend = new ColorBlend
                    {
                        Colors = new[]
                        {
                            darkColor,
                            color,
                            lightColor
                        },
                        Positions = new[]
                        {
                            0f,
                            0.25f,
                            1f
                        }
                    };
                    brush.InterpolationColors = blend;
                    brush.CenterPoint = new PointF
                        (
                        rectangle.Left + rectangle.Width * 0.3f,
                        rectangle.Top + rectangle.Width * 0.3f
                        );
                    g.FillPath(brush, path);
                }
                _DrawPath(g, pen, path);
            }
        }

        /// <summary>
        /// Рисование куска торта.
        /// </summary>
        public static void DrawPieSlice
            (
                Graphics g,
                Rectangle rectangle,
                float startAngle,
                float sweepAngle,
                int height,
                Color color,
                Pen pen
            )
        {
            // Мы не хотим морочиться с отрицательными углами.
            if (sweepAngle < 0)
            {
                startAngle += sweepAngle;
                sweepAngle = -sweepAngle;
            }
            startAngle = NormalizeAngle(startAngle);
            float endAngle = NormalizeAngle(startAngle + sweepAngle);
            Rectangle bottom = rectangle;
            bottom.Y += height;
            float rx = rectangle.Width / 2f;
            float ry = rectangle.Height / 2f;
            float cx = rectangle.Left + rx;
            float cy = rectangle.Top + ry;
            Color lighterColor = Utility.Lighten(color, 0.5f);
            Color darkerColor = Utility.Darken(color, 0.5f);

            // Шрифт нужен лишь для отладочной печати
            //using ( Font font = new Font ( "Arial", 8f ) )
            using (Brush mainBrush = new SolidBrush(color))
            {
                using (Brush lighterBrush = new SolidBrush(lighterColor))
                {
                    using (Brush darkerBrush = new SolidBrush(darkerColor))
                    {
                        using (LinearGradientBrush borderBrush =
                            new LinearGradientBrush(rectangle,
                                                      lighterColor,
                                                      darkerColor,
                                                      0f))
                        {
                            using (GraphicsPath arcPath = new GraphicsPath())
                            {
                                ColorBlend blend = new ColorBlend(3)
                                {
                                    Colors = new[]
                                    {
                                        color,
                                        lighterColor,
                                        color,
                                        darkerColor
                                    },
                                    Positions = new[]
                                    {
                                        0f,
                                        0.15f,
                                        0.50f,
                                        1f
                                    }
                                };
                                borderBrush.InterpolationColors = blend;

                                arcPath.AddPie(rectangle, startAngle, sweepAngle);
                                PointF[] pathPoints = arcPath.PathPoints;
                                PointF topCenter = new PointF(cx, cy);
                                PointF bottomCenter = new PointF(cx, cy + height);
                                PointF topStart = pathPoints[1];
                                PointF bottomStart = topStart;
                                bottomStart.Y += height;
                                PointF topEnd = pathPoints[pathPoints.Length - 1];
                                PointF bottomEnd = topEnd;
                                bottomEnd.Y += height;

                                PointF[] startEdge = new PointF[5];
                                startEdge[0] = topCenter;
                                startEdge[1] = topStart;
                                startEdge[2] = bottomStart;
                                startEdge[3] = bottomCenter;
                                startEdge[4] = topCenter;

                                PointF[] sweepEdge = new PointF[5];
                                sweepEdge[0] = topCenter;
                                sweepEdge[1] = topEnd;
                                sweepEdge[2] = bottomEnd;
                                sweepEdge[3] = bottomCenter;
                                sweepEdge[4] = topCenter;

                                if ((endAngle > 270)
                                     || (endAngle < 90))
                                {
                                    Brush edgeBrush = (endAngle < 90)
                                                          ? lighterBrush
                                                          : darkerBrush;
                                    using (GraphicsPath path = new GraphicsPath())
                                    {
                                        path.AddLines(sweepEdge);
                                        g.FillPath(edgeBrush, path);
                                        _DrawPath(g, pen, path);
                                    }
                                }

                                if ((startAngle > 90)
                                     && (startAngle < 270))
                                {
                                    Brush edgeBrush = (startAngle > 180)
                                                          ? lighterBrush
                                                          : darkerBrush;
                                    using (GraphicsPath path = new GraphicsPath())
                                    {
                                        path.AddLines(startEdge);
                                        g.FillPath(edgeBrush, path);
                                        _DrawPath(g, pen, path);
                                    }
                                }

                                float angle1 = startAngle;
                                if (angle1 > endAngle)
                                {
                                    angle1 = 0f;
                                }
                                float angle2 = Math.Min(endAngle, 180f);
                                if ((angle1 <= 180)
                                     || (angle2 <= 180)
                                    )
                                {
                                    using (GraphicsPath path = new GraphicsPath())
                                    {
                                        // ReSharper disable CompareOfFloatsByEqualityOperator
                                        if (angle2 == endAngle) //-V3024
                                        // ReSharper restore CompareOfFloatsByEqualityOperator
                                        {
                                            path.AddLine(topEnd, bottomEnd);
                                        }
                                        else
                                        {
                                            path.AddLine(cx - rx,
                                                           cy,
                                                           cx - rx,
                                                           cy + height);
                                        }
                                        float delta = angle2 - angle1;
                                        if (angle1 > angle2)
                                        {
                                            delta = angle2;
                                        }
                                        delta = -delta;
                                        path.AddArc(bottom, angle2, delta);
                                        PointF bottomLast = _LastPoint(path);
                                        PointF topLast = bottomLast;
                                        topLast.Y -= height;
                                        path.AddLine(bottomLast, topLast);
                                        g.FillPath(borderBrush, path);
                                        _DrawPath(g, pen, path);
                                    }
                                }

                                // Случай, когда нужны два бортика.
                                if ((startAngle < 180f)
                                     && (endAngle < startAngle))
                                {
                                    using (GraphicsPath path = new GraphicsPath())
                                    {
                                        float delta = 180f - startAngle;
                                        path.AddLine(topStart, bottomStart);
                                        path.AddArc(bottom, startAngle, delta);
                                        path.AddLine(cx - rx, cy + height, cx - rx, cy);
                                        g.FillPath(borderBrush, path);
                                        _DrawPath(g, pen, path);
                                    }
                                }

                                g.FillPath(mainBrush, arcPath);
                                _DrawPath(g, pen, arcPath);

                                // Отладка (интересно, все-таки, что же получилось ;)
                                //				g.DrawString ( startAngle.ToString (), font, Brushes.Red,
                                //					pathPoints [ 1 ], TextFormat.CenterCenter );
                                //				g.DrawString ( endAngle.ToString (), font, Brushes.SteelBlue,
                                //					pathPoints [ pathPoints.Length - 1 ],
                                //					TextFormat.CenterCenter );
                            }
                        }
                    }
                }
            }
        }

        #endregion
    }
}
