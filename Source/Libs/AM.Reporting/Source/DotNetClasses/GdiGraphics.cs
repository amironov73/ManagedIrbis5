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
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;

#endregion

#nullable enable

namespace AM.Reporting
{
    /// <summary>
    /// Drawing objects to a standard Graphics or Bitmap
    /// </summary>
    public class GdiGraphics : IGraphics
    {
        private readonly bool haveToDispose;

        #region Properties

        public Graphics Graphics { get; private set; }

        float IGraphics.DpiX => Graphics.DpiX;
        float IGraphics.DpiY => Graphics.DpiY;

        TextRenderingHint IGraphics.TextRenderingHint
        {
            get => Graphics.TextRenderingHint;
            set => Graphics.TextRenderingHint = value;
        }

        InterpolationMode IGraphics.InterpolationMode
        {
            get => Graphics.InterpolationMode;
            set => Graphics.InterpolationMode = value;
        }

        SmoothingMode IGraphics.SmoothingMode
        {
            get => Graphics.SmoothingMode;
            set => Graphics.SmoothingMode = value;
        }

        System.Drawing.Drawing2D.Matrix IGraphics.Transform
        {
            get => Graphics.Transform;
            set => Graphics.Transform = value;
        }

        GraphicsUnit IGraphics.PageUnit
        {
            get => Graphics.PageUnit;
            set => Graphics.PageUnit = value;
        }

        bool IGraphics.IsClipEmpty => Graphics.IsClipEmpty;

        Region IGraphics.Clip
        {
            get => Graphics.Clip;
            set => Graphics.Clip = value;
        }

        CompositingQuality IGraphics.CompositingQuality
        {
            get => Graphics.CompositingQuality;
            set => Graphics.CompositingQuality = value;
        }

        #endregion

        public GdiGraphics (Image image)
            : this (Graphics.FromImage (image), true)
        {
        }

        public GdiGraphics (Graphics graphics, bool haveToDispose)
        {
            this.Graphics = graphics;
            this.haveToDispose = haveToDispose;
        }


        #region IDisposable Support

        private bool disposedValue; // To detect redundant calls

        protected virtual void Dispose (bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (Graphics != null && haveToDispose)
                    {
                        Graphics.Dispose();
                    }

                    Graphics = null;

                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~ImageGraphicsRenderer() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose (true);

            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }

        #endregion

        #region Draw and measure text

        public void DrawString (string text, Font drawFont, Brush brush, float left, float top)
        {
            Graphics.DrawString (text, drawFont, brush, left, top);
        }

        public void DrawString (string text, Font drawFont, Brush brush, RectangleF rectangleF)
        {
            Graphics.DrawString (text, drawFont, brush, rectangleF);
        }

        public void DrawString (string text, Font font, Brush textBrush, RectangleF textRect, StringFormat format)
        {
            Graphics.DrawString (text, font, textBrush, textRect, format);
        }

        public void DrawString (string text, Font font, Brush brush, float left, float top, StringFormat format)
        {
            Graphics.DrawString (text, font, brush, left, top, format);
        }

        void IGraphics.DrawString (string s, Font font, Brush brush, PointF point, StringFormat format)
        {
            Graphics.DrawString (s, font, brush, point, format);
        }

        public Region[] MeasureCharacterRanges (string text, Font font, RectangleF rect, StringFormat format)
        {
            return Graphics.MeasureCharacterRanges (text, font, rect, format);
        }

        public SizeF MeasureString (string text, Font font, SizeF size)
        {
            return Graphics.MeasureString (text, font, size);
        }

        public SizeF MeasureString (string text, Font font, int width, StringFormat format)
        {
            return Graphics.MeasureString (text, font, width, format);
        }

        public void MeasureString (string text, Font font, SizeF size, StringFormat format, out int charsFit,
            out int linesFit)
        {
            Graphics.MeasureString (text, font, size, format, out charsFit, out linesFit);
        }

        public SizeF MeasureString (string text, Font drawFont)
        {
            return Graphics.MeasureString (text, drawFont);
        }

        public SizeF MeasureString (string text, Font font, SizeF layoutArea, StringFormat format)
        {
            return Graphics.MeasureString (text, font, layoutArea, format);
        }

        #endregion

        #region Draw images

        public void DrawImage (Image image, float x, float y)
        {
            Graphics.DrawImage (image, x, y);
        }

        public void DrawImage (Image image, RectangleF destRect, RectangleF srcRect, GraphicsUnit unit)
        {
            Graphics.DrawImage (image, destRect, srcRect, unit);
        }

        public void DrawImage (Image image, RectangleF rect)
        {
            Graphics.DrawImage (image, rect);
        }

        public void DrawImage (Image image, float x, float y, float width, float height)
        {
            Graphics.DrawImage (image, x, y, width, height);
        }

        public void DrawImage (Image image, PointF[] points)
        {
            Graphics.DrawImage (image, points);
        }

        public void DrawImage (Image image, Rectangle destRect, int srcX, int srcY, int srcWidth, int srcHeight,
            GraphicsUnit srcUnit, ImageAttributes imageAttr)
        {
            Graphics.DrawImage (image, destRect, srcX, srcY, srcWidth, srcHeight, srcUnit, imageAttr);
        }

        public void DrawImage (Image image, Rectangle destRect, float srcX, float srcY, float srcWidth, float srcHeight,
            GraphicsUnit srcUnit, ImageAttributes imageAttrs)
        {
            Graphics.DrawImage (image, destRect, srcX, srcY, srcWidth, srcHeight, srcUnit, imageAttrs);
        }

        public void DrawImageUnscaled (Image image, Rectangle rect)
        {
            Graphics.DrawImageUnscaled (image, rect);
        }

        #endregion

        #region Draw geometry

        public void DrawArc (Pen pen, float x, float y, float width, float height, float startAngle, float sweepAngle)
        {
            Graphics.DrawArc (pen, x, y, width, height, startAngle, sweepAngle);
        }

        public void DrawCurve (Pen pen, PointF[] points, int offset, int numberOfSegments, float tension)
        {
            Graphics.DrawCurve (pen, points, offset, numberOfSegments, tension);
        }

        public void DrawEllipse (Pen pen, float left, float top, float width, float height)
        {
            Graphics.DrawEllipse (pen, left, top, width, height);
        }

        public void DrawEllipse (Pen pen, RectangleF rect)
        {
            Graphics.DrawEllipse (pen, rect);
        }

        public void DrawLine (Pen pen, float x1, float y1, float x2, float y2)
        {
            Graphics.DrawLine (pen, x1, y1, x2, y2);
        }

        public void DrawLine (Pen pen, PointF p1, PointF p2)
        {
            Graphics.DrawLine (pen, p1, p2);
        }

        public void DrawLines (Pen pen, PointF[] points)
        {
            Graphics.DrawLines (pen, points);
        }

        public void DrawPath (Pen outlinePen, GraphicsPath path)
        {
            Graphics.DrawPath (outlinePen, path);
        }

        public void DrawPie (Pen pen, float x, float y, float width, float height, float startAngle, float sweepAngle)
        {
            Graphics.DrawPie (pen, x, y, width, height, startAngle, sweepAngle);
        }

        public void DrawPolygon (Pen pen, PointF[] points)
        {
            Graphics.DrawPolygon (pen, points);
        }

        public void DrawPolygon (Pen pen, Point[] points)
        {
            Graphics.DrawPolygon (pen, points);
        }

        public void DrawRectangle (Pen pen, float left, float top, float width, float height)
        {
            Graphics.DrawRectangle (pen, left, top, width, height);
        }

        public void DrawRectangle (Pen pen, Rectangle rect)
        {
            Graphics.DrawRectangle (pen, rect);
        }

        public void PathAddRectangle (GraphicsPath path, RectangleF rect)
        {
            path.AddRectangle (rect);
        }

        #endregion

        #region Fill geometry

        public void FillEllipse (Brush brush, float x, float y, float dx, float dy)
        {
            Graphics.FillEllipse (brush, x, y, dx, dy);
        }

        public void FillEllipse (Brush brush, RectangleF rect)
        {
            Graphics.FillEllipse (brush, rect);
        }

        public void FillPath (Brush brush, GraphicsPath path)
        {
            Graphics.FillPath (brush, path);
        }

        public void FillPie (Brush brush, float x, float y, float width, float height, float startAngle,
            float sweepAngle)
        {
            Graphics.FillPie (brush, x, y, width, height, startAngle, sweepAngle);
        }

        public void FillPolygon (Brush brush, PointF[] points)
        {
            Graphics.FillPolygon (brush, points);
        }

        public void FillPolygon (Brush brush, Point[] points)
        {
            Graphics.FillPolygon (brush, points);
        }

        public void FillRectangle (Brush brush, RectangleF rect)
        {
            Graphics.FillRectangle (brush, rect);
        }

        public void FillRectangle (Brush brush, float left, float top, float width, float height)
        {
            Graphics.FillRectangle (brush, left, top, width, height);
        }

        public void FillRegion (Brush brush, Region region)
        {
            Graphics.FillRegion (brush, region);
        }

        #endregion

        #region Fill And Draw

        public void FillAndDrawPath (Pen pen, Brush brush, GraphicsPath path)
        {
            FillPath (brush, path);
            DrawPath (pen, path);
        }

        public void FillAndDrawEllipse (Pen pen, Brush brush, RectangleF rect)
        {
            FillEllipse (brush, rect);
            DrawEllipse (pen, rect);
        }

        public void FillAndDrawEllipse (Pen pen, Brush brush, float left, float top, float width, float height)
        {
            FillEllipse (brush, left, top, width, height);
            DrawEllipse (pen, left, top, width, height);
        }

        public void FillAndDrawPolygon (Pen pen, Brush brush, Point[] points)
        {
            FillPolygon (brush, points);
            DrawPolygon (pen, points);
        }

        public void FillAndDrawPolygon (Pen pen, Brush brush, PointF[] points)
        {
            FillPolygon (brush, points);
            DrawPolygon (pen, points);
        }

        public void FillAndDrawRectangle (Pen pen, Brush brush, float left, float top, float width, float height)
        {
            FillRectangle (brush, left, top, width, height);
            DrawRectangle (pen, left, top, width, height);
        }

        #endregion

        #region Transform

        public void MultiplyTransform (System.Drawing.Drawing2D.Matrix matrix, MatrixOrder order)
        {
            Graphics.MultiplyTransform (matrix, order);
        }

        public void RotateTransform (float angle)
        {
            Graphics.RotateTransform (angle);
        }

        public void ScaleTransform (float scaleX, float scaleY)
        {
            Graphics.ScaleTransform (scaleX, scaleY);
        }

        public void TranslateTransform (float left, float top)
        {
            Graphics.TranslateTransform (left, top);
        }

        #endregion

        #region State

        public void Restore (IGraphicsState state)
        {
            if (state is ImageGraphicsRendererState rendererState)
            {
                Graphics.Restore (rendererState.GraphicsState);
            }
        }

        public IGraphicsState Save()
        {
            return new ImageGraphicsRendererState (Graphics.Save());
        }

        #endregion

        #region Clip

        public bool IsVisible (RectangleF rect)
        {
            return Graphics.IsVisible (rect);
        }

        public void ResetClip()
        {
            Graphics.ResetClip();
        }

        public void SetClip (RectangleF rect)
        {
            Graphics.SetClip (rect);
        }

        public void SetClip (RectangleF rect, CombineMode combineMode)
        {
            Graphics.SetClip (rect, combineMode);
        }

        public void SetClip (GraphicsPath path, CombineMode combineMode)
        {
            Graphics.SetClip (path, combineMode);
        }

        #endregion

        public class ImageGraphicsRendererState : IGraphicsState
        {
            public GraphicsState GraphicsState { get; }

            public ImageGraphicsRendererState (GraphicsState state)
            {
                GraphicsState = state;
            }
        }

        public static GdiGraphics FromImage (Image image)
        {
            return new GdiGraphics (image);
        }

        public static GdiGraphics FromGraphics (Graphics graphics)
        {
            return new GdiGraphics (graphics, false);
        }

        public static GdiGraphics FromHdc (nint hdc)
        {
            return FromGraphics (Graphics.FromHdc (hdc));
        }
    }
}
